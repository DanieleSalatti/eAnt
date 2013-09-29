namespace eAnt.eDonkey
{
    using eAnt.Types;
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net.Sockets;
    using System.Text.RegularExpressions;

    internal class CWebCacheServerConnection : CConnection
    {
        // Methods
        public CWebCacheServerConnection(Socket in_Socket, MemoryStream in_ReceptionStream) : base(in_Socket, in_ReceptionStream)
        {
            this.m_ProcessRequest();
        }

        private void m_ProcessRequest()
        {
            base.m_ResetTimeOut();
            this.m_ReceptionStream.Seek((long) 0, SeekOrigin.Begin);
            StreamReader reader1 = new StreamReader(this.m_ReceptionStream);
            string text1 = "";
            bool flag1 = true;
            this.m_Headers = new StringCollection();
            try
            {
                text1 = reader1.ReadLine();
                string text2 = "";
                while ((text1 != null) && (text1.Length > 0))
                {
                    this.m_Headers.Add(text1);
                    this.m_HeaderLength += (text1.Length + 2);
                    if (text1.IndexOf("Pragma: IDs=") == 0)
                    {
                        string text3 = text1.Substring(text1.IndexOf('=') + 1);
                        char[] chArray1 = new char[1] { '|' } ;
                        string[] textArray1 = text3.Split(chArray1);
                        uint num1 = Convert.ToUInt32(textArray1[0]);
                        if (textArray1[1].IndexOf(',') >= 0)
                        {
                            text2 = textArray1[1].Substring(0, textArray1[1].IndexOf(','));
                        }
                        else
                        {
                            text2 = textArray1[1];
                        }
                        this.m_Client = CKernel.ClientsList.GetClientByWebCacheUpId(num1);
                    }
                    try
                    {
                        text1 = reader1.ReadLine();
                        if (text1 == null)
                        {
                            flag1 = false;
                        }
                        continue;
                    }
                    catch
                    {
                        flag1 = false;
                        break;
                    }
                }
                if (!flag1)
                {
                    this.m_ReceptionStream.Seek((long) 0, SeekOrigin.End);
                    this.ReceivePacket();
                }
                else if (this.m_Client != null)
                {
                    Regex regex1 = new Regex("GET /encryptedData/(?<start>[0-9]+)-(?<end>[0-9]+)/(?<encHash>.*).htm HTTP/(?<major>[0-9]+).(?<minor>[0-9]+)");
                    Match match1 = regex1.Match(this.m_Headers[0]);
                    string text4 = match1.Result("${start}");
                    string text5 = match1.Result("${end}");
                    string text6 = match1.Result("${encHash}");
                    uint num2 = Convert.ToUInt32(text4);
                    uint num3 = Convert.ToUInt32(text5);
                    if (num2 > num3)
                    {
                        this.OnConnectionFail(0x1a0);
                    }
                    if (!this.m_Client.ProcessRequestPartsWebCache(this, num2, num3, text6, text2))
                    {
                        this.OnConnectionFail(0x194);
                    }
                    else
                    {
                        this.m_ReceptionStream.Close();
                        this.m_ReceptionStream = new MemoryStream();
                        this.ReceivePacket();
                    }
                }
                else
                {
                    bool flag2 = false;
                    if (CKernel.Preferences.GetBool("WebInterfaceEnabled"))
                    {
                        try
                        {
                            string text7 = "";
                            string text8 = "";
                            string text9 = "";
                            if (this.m_Headers[0] == "GET / HTTP/1.1")
                            {
                                text7 = "logon";
                                text8 = "asp";
                                text9 = "";
                            }
                            else if (this.m_Headers[0] == "GET /favicon.ico HTTP/1.1")
                            {
                                text7 = "favicon";
                                text8 = "ico";
                                text9 = "";
                            }
                            else
                            {
                                Regex regex2 = new Regex(@"GET /WebInterface/(?<requestPath>\w+)\.(?<requestExtension>\w+)(?<queryString>.*) HTTP/(?<major>[0-9]+).(?<minor>[0-9]+)");
                                Match match2 = regex2.Match(this.m_Headers[0]);
                                text7 = match2.Result("${requestPath}");
                                text8 = match2.Result("${requestExtension}");
                                text9 = match2.Result("${queryString}");
                            }
                            string text10 = "";
                            foreach (string text11 in this.m_Headers)
                            {
                                if (text11.Substring(0, 6) == "Cookie")
                                {
                                    text10 = text11.Substring(8);
                                }
                            }
                            flag2 = true;
                            CLog.Log(Constants.Log.Verbose, "Requested:" + this.m_Headers[0]);
                            CKernel.WebServer.ProcessRequest(text7, text8, text10, text9, this);
                        }
                        catch (Exception exception1)
                        {
                            Console.WriteLine(exception1.ToString());
                            CLog.Log(Constants.Log.Verbose, "Requested object not found: " + this.m_Headers[0]);
                            this.OnConnectionFail(0x194);
                        }
                    }
                    if (!flag2 && flag1)
                    {
                        this.OnConnectionFail(0x194);
                    }
                    this.m_ReceptionStream.Close();
                    this.m_ReceptionStream = new MemoryStream();
                    this.ReceivePacket();
                }
            }
            catch (Exception exception2)
            {
                CLog.Log(Constants.Log.Info, exception2.ToString());
                this.OnConnectionFail(400);
            }
        }

        protected override void OnConnectionFail(byte reason)
        {
            this.OnConnectionFail(reason);
        }

        private void OnConnectionFail(int reason)
        {
            try
            {
                if (this.m_Client != null)
                {
                    this.m_Client.OnDisconnectWebCache(reason);
                }
                bool flag1 = ((reason == 400) || (reason == 0x1a0)) || (reason == 0x194);
                if (flag1 && this.m_socket.Connected)
                {
                    MemoryStream stream1 = new MemoryStream();
                    StreamWriter writer1 = new StreamWriter(stream1);
                    writer1.Write("HTTP/1.1 " + reason.ToString() + "\r\n");
                    writer1.Write("Content-Length: 0\r\n");
                    writer1.Write("\r\n");
                    this.m_socket.Send(stream1.ToArray());
                }
            }
            catch
            {
            }
            base.CloseConnection();
        }

        protected override void OnPacketReceived(IAsyncResult ar)
        {
            int num1 = 0;
            try
            {
                num1 = this.m_socket.EndReceive(ar);
                if ((num1 != 0) && (this.m_ReceptionPacket != null))
                {
                    this.m_ReceptionStream.Write(this.m_ReceptionPacket, 0, num1);
                    this.m_ProcessRequest();
                }
                else
                {
                    this.OnConnectionFail(3);
                }
            }
            catch
            {
                this.OnConnectionFail(2);
            }
        }


        // Properties
        public int HeaderLength
        {
            get
            {
                return this.m_HeaderLength;
            }
        }


        // Fields
        private int m_HeaderLength;
        private StringCollection m_Headers;
    }
}

