namespace eAnt.eDonkey
{
    using eAnt.Types;
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Text;

    internal class CWebCacheClientConnection : CConnection
    {
        // Methods
        public CWebCacheClientConnection(uint IP, ushort port, CClient in_Client) : base(IP, port, in_Client)
        {
            this.m_HeaderReceived = false;
            this.m_Headers = new StringCollection();
            this.m_blockBytesReceived = 0;
            this.m_ErrorNotified = false;
            if (!this.IsProxyConnection)
            {
                CKernel.WebCache.IncConcurrentTransfers();
            }
        }

        private string BuildClientHeader(string URL, string hostPort, string remoteSlaveKey, uint webCacheDownloadId)
        {
            string text1 = "GET " + URL + " HTTP/1.1\r\n";
            text1 = text1 + "Host: " + hostPort + "\r\n";
            text1 = text1 + "Cache-Control: max-age=0\r\n";
            text1 = text1 + "Proxy-Connection: keep-alive\r\n";
            string text2 = text1;
            string[] textArray1 = new string[6] { text2, "Pragma: IDs=", webCacheDownloadId.ToString(), "|", remoteSlaveKey, "\r\n" } ;
            text1 = string.Concat(textArray1);
            object obj1 = text1;
            object[] objArray1 = new object[6];
            objArray1[0] = obj1;
            objArray1[1] = "User-Agent: Ant";
            uint num1 = 2;
            objArray1[2] = num1.ToString();
            objArray1[3] = ".";
            objArray1[4] = 7;
            objArray1[5] = "\r\n";
            text1 = string.Concat(objArray1);
            return (text1 + "\r\n");
        }

        private string BuildDirectProxyHeader(string URL, string hostPort)
        {
            string text1 = "GET " + URL + " HTTP/1.1\r\n";
            text1 = text1 + "Host: " + hostPort + "\r\n";
            text1 = text1 + "Cache-Control: only-if-cached\r\n";
            text1 = text1 + "Connection: close\r\nProxy-Connection: close\r\n";
            object obj1 = text1;
            object[] objArray1 = new object[6];
            objArray1[0] = obj1;
            objArray1[1] = "User-Agent: Ant";
            uint num1 = 2;
            objArray1[2] = num1.ToString();
            objArray1[3] = ".";
            objArray1[4] = 7;
            objArray1[5] = "\r\n";
            text1 = string.Concat(objArray1);
            return (text1 + "\r\n");
        }

        public bool GetURL(string hostPort, string remoteSlaveKey, uint start, uint end, string filehash, uint webCacheDownloadId)
        {
            string text2;
            if (this.m_HeaderReceived)
            {
                return false;
            }
            string[] textArray1 = new string[9] { "http://", hostPort, "/encryptedData/", start.ToString(), "-", end.ToString(), "/", filehash, ".htm" } ;
            string text1 = string.Concat(textArray1);
            this.blockStart = start;
            this.blockEnd = end;
            if (this.IsProxyConnection)
            {
                text2 = this.BuildDirectProxyHeader(text1, hostPort);
            }
            else
            {
                text2 = this.BuildClientHeader(text1, hostPort, remoteSlaveKey, webCacheDownloadId);
            }
            CConnection.stSendingPacket packet1 = new CConnection.stSendingPacket();
            packet1.Packet = Encoding.Default.GetBytes(text2);
            packet1.Freed = !base.Connected;
            this.m_SendPacketList.Add(packet1);
            base.m_ResetTimeOut();
            if (!base.Connected)
            {
                CLog.Log(Constants.Log.Verbose, "Opening new connection");
                base.Connect();
            }
            else
            {
                CLog.Log(Constants.Log.Verbose, "Pidiendo bloque, ya conectado");
                base.FreeDataBlock();
            }
            return true;
        }

        private bool m_ProcessHeaders()
        {
            if (this.m_Headers[0].IndexOf("200") < 0)
            {
                return false;
            }
            foreach (string text1 in this.m_Headers)
            {
                if (text1.ToLower().IndexOf("content-length:") < 0)
                {
                    continue;
                }
                try
                {
                    uint num1 = Convert.ToUInt32(text1.Substring(text1.IndexOf(':') + 1));
                    if (num1 == ((this.blockEnd - this.blockStart) + 1))
                    {
                        continue;
                    }
                    CLog.Log(Constants.Log.Verbose, "Content length mistmatch");
                    return false;
                }
                catch
                {
                    CLog.Log(Constants.Log.Verbose, "Error reading content length");
                    continue;
                }
            }
            return true;
        }

        protected override bool m_ProcessReceptionStream(ref bool handleConnection)
        {
            if (!this.m_HeaderReceived)
            {
                this.m_ReceptionStream.Seek((long) 0, SeekOrigin.Begin);
                StreamReader reader1 = new StreamReader(this.m_ReceptionStream);
                this.m_Headers.Clear();
                try
                {
                    int num1 = 0;
                    string text1 = reader1.ReadLine();
                    while (text1 != "")
                    {
                        this.m_Headers.Add(text1);
                        num1 += (text1.Length + 2);
                        text1 = reader1.ReadLine();
                    }
                    if (text1.Length == 0)
                    {
                        if (!this.m_ProcessHeaders())
                        {
                            this.OnConnectionFail(400);
                            return false;
                        }
                        num1 += 2;
                        this.m_HeaderReceived = true;
                        byte[] buffer1 = new byte[this.m_ReceptionStream.Length - num1];
                        this.m_ReceptionStream.Seek((long) num1, SeekOrigin.Begin);
                        this.m_ReceptionStream.Read(buffer1, 0, buffer1.Length);
                        this.m_ReceptionStream.SetLength((long) 0);
                        this.m_ReceptionStream.Write(buffer1, 0, buffer1.Length);
                    }
                }
                catch
                {
                }
                this.m_ReceptionStream.Seek((long) 0, SeekOrigin.End);
            }
            else
            {
                byte[] buffer2 = new byte[this.m_ReceptionStream.Length];
                this.m_ReceptionStream.Seek((long) 0, SeekOrigin.Begin);
                this.m_ReceptionStream.Read(buffer2, 0, (int) this.m_ReceptionStream.Length);
                if ((this.m_blockBytesReceived + ((uint) this.m_ReceptionStream.Length)) == ((this.blockEnd - this.blockStart) + 1))
                {
                    this.m_HeaderReceived = false;
                }
                this.m_Client.ReceiveBlockWebCache(this.blockStart + this.m_blockBytesReceived, (this.blockStart + this.m_blockBytesReceived) + ((uint) buffer2.Length), buffer2);
                if (!this.m_HeaderReceived)
                {
                    this.m_blockBytesReceived = 0;
                    CLog.Log(Constants.Log.Verbose, "Downloaded block via web cache");
                }
                else
                {
                    this.m_blockBytesReceived += ((uint) this.m_ReceptionStream.Length);
                }
                this.m_ReceptionStream.SetLength((long) 0);
            }
            return false;
        }

        protected override void OnConnectionFail(byte reason)
        {
            this.OnConnectionFail(reason);
        }

        private void OnConnectionFail(int reason)
        {
            if (!this.IsProxyConnection)
            {
                CKernel.WebCache.DecConcurrentTransfers();
            }
            if ((this.m_Client != null) && !this.m_ErrorNotified)
            {
                this.m_ErrorNotified = true;
                this.m_Client.OnDisconnectWebCacheDown(reason);
            }
            base.CloseConnection();
        }


        // Properties
        public bool IsProxyConnection
        {
            get
            {
                if (this.m_Client != null)
                {
                    return (this.m_Client.SoftwareID == 0x36);
                }
                return false;
            }
        }


        // Fields
        private uint blockEnd;
        private uint blockStart;
        private uint m_blockBytesReceived;
        private bool m_ErrorNotified;
        private bool m_HeaderReceived;
        private StringCollection m_Headers;
    }
}

