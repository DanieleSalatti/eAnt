namespace eAnt.eDonkey
{
    using eAnt.Types;
    using System;
    using System.Collections;
    using System.Net;
    using System.Threading;

    internal class CWebCache
    {
        // Methods
        public CWebCache()
        {
            this.m_LastNameLookup = DateTime.MinValue;
        }

        public void AddCachedBlock(CCmdCachedBlock cachedBlock, CClient client)
        {
            if (this.m_RuningOK)
            {
                if (client == null)
                {
                    CLog.Log(Constants.Log.Verbose, "Cached block de un cliente desconocido");
                }
                if (!this.BlockFailedBefore(cachedBlock))
                {
                    CCachedBlock block1 = new CCachedBlock(cachedBlock, client);
                    bool flag1 = false;
                    if (block1.Element != null)
                    {
                        for (int num1 = 0; num1 < this.m_PendingBlocks.Count; num1++)
                        {
                            if (((((CCachedBlock) this.m_PendingBlocks[num1]).StartOffset == block1.StartOffset) && (((CCachedBlock) this.m_PendingBlocks[num1]).EndOffset == block1.EndOffset)) && (((CCachedBlock) this.m_PendingBlocks[num1]).Key[0] == block1.Key[0]))
                            {
                                flag1 = true;
                                CLog.Log(Constants.Log.Verbose, "Cached block already added");
                                break;
                            }
                        }
                        if (!flag1)
                        {
                            this.m_PendingBlocks.Add(block1);
                        }
                    }
                }
            }
        }

        private bool BlockFailedBefore(CCmdCachedBlock cachedBlock)
        {
            if ((this.m_FailedWebcacheBlocks != null) && (this.m_FailedWebcacheBlocks.Count != 0))
            {
                for (int num1 = 0; num1 < this.m_FailedWebcacheBlocks.Count; num1++)
                {
                    CCachedBlock block1 = (CCachedBlock) this.m_FailedWebcacheBlocks[num1];
                    if (((cachedBlock.StartOffset == block1.StartOffset) && (cachedBlock.EndOffset == block1.EndOffset)) && ((cachedBlock.RemoteKey[0] == block1.Key[0]) && (cachedBlock.RemoteKey[1] == block1.Key[1])))
                    {
                        CLog.Log(Constants.Log.Verbose, "Ignored previous failed block");
                        return true;
                    }
                }
            }
            return false;
        }

        public int DecConcurrentTransfers()
        {
            if (this.m_ConcurrentTransfers > 0)
            {
                this.m_ConcurrentTransfers--;
            }
            return this.m_ConcurrentTransfers;
        }

        public void EndWebCache()
        {
            try
            {
                if ((this.m_WebCacheThread != null) && this.m_WebCacheThread.IsAlive)
                {
                    this.m_WebCacheThread.Abort();
                }
            }
            catch
            {
            }
        }

        public int IncConcurrentTransfers()
        {
            this.m_ConcurrentTransfers++;
            return this.m_ConcurrentTransfers;
        }

        public int IncWebCacheErrors()
        {
            int num1;
            this.m_WebCacheErrors = num1 = this.m_WebCacheErrors + 1;
            return num1;
        }

        private void m_Process()
        {
            CCachedBlock block1 = null;
            CWebCacheClient client1 = null;
            while (this.m_WebCacheThread.IsAlive)
            {
                if ((this.m_PendingBlocks.Count > 0) && (block1 == null))
                {
                    block1 = (CCachedBlock) this.m_PendingBlocks[0];
                    this.m_PendingBlocks.RemoveAt(0);
                    if (block1.Element.File != null)
                    {
                        if (block1.Element.File.FileStatus == Protocol.FileState.Ready)
                        {
                            client1 = new CWebCacheClient(block1);
                        }
                        else
                        {
                            block1.Element.File.RemoveRequestedBlock(block1.RealStartOffset, block1.RealEndOffset);
                            block1 = null;
                        }
                    }
                    else
                    {
                        block1 = null;
                    }
                }
                if ((client1 != null) && client1.ConnectionEnded)
                {
                    if (client1.Success)
                    {
                        if (this.m_SuccessWebcacheBlocks == null)
                        {
                            this.m_SuccessWebcacheBlocks = new ArrayList();
                        }
                        if (this.m_SuccessWebcacheBlocks.Count > 400)
                        {
                            this.m_SuccessWebcacheBlocks.RemoveAt(0);
                        }
                        this.m_SuccessWebcacheBlocks.Add(block1);
                    }
                    else
                    {
                        if (this.m_FailedWebcacheBlocks == null)
                        {
                            this.m_FailedWebcacheBlocks = new ArrayList();
                        }
                        if (this.m_FailedWebcacheBlocks.Count > 400)
                        {
                            this.m_FailedWebcacheBlocks.RemoveAt(0);
                        }
                        block1.Element = null;
                        this.m_FailedWebcacheBlocks.Add(block1);
                    }
                    block1 = null;
                    client1 = null;
                    CLog.Log(Constants.Log.Verbose, "Cache blocks on queue: " + this.m_PendingBlocks.Count.ToString());
                }
                Thread.Sleep(100);
            }
        }

        public void ResetWebCacheErrors()
        {
            this.m_WebCacheErrors = 0;
        }

        public void ReStartWebCache()
        {
            this.EndWebCache();
            this.StartWebCache();
        }

        public void StartWebCache()
        {
            this.m_PendingBlocks = new ArrayList();
            this.rnd = new Random();
            this.m_WebCacheErrors = 0;
            this.m_ConcurrentTransfers = 0;
            this.m_Name = "";
            this.m_RuningOK = CKernel.Preferences.GetBool("WebCacheEnabled");
            if (this.m_RuningOK)
            {
                try
                {
                    this.m_Name = CKernel.Preferences.GetString("WebCacheProxy");
                    if (this.m_Name == null)
                    {
                        this.m_Name = "";
                    }
                    if (this.m_Name.Length > 0)
                    {
                        IPHostEntry entry1 = Dns.GetHostByName(this.m_Name);
                        if (entry1.AddressList.Length > 0)
                        {
                            this.m_WebCacheIP = BitConverter.ToUInt32(entry1.AddressList[0].GetAddressBytes(), 0);
                        }
                        else
                        {
                            CLog.Log(Constants.Log.Info, "Invalid webcache configured");
                            this.m_RuningOK = false;
                            return;
                        }
                        this.m_LastNameLookup = DateTime.Now;
                        this.m_WebCachePort = CKernel.Preferences.GetUShort("WebCacheProxyPort");
                        this.m_WebCacheThread = new Thread(new ThreadStart(this.m_Process));
                        this.m_WebCacheThread.Name = "WebCacheThread";
                        this.m_WebCacheThread.Start();
                    }
                    else
                    {
                        this.m_RuningOK = false;
                    }
                }
                catch
                {
                    CLog.Log(Constants.Log.Info, "The Webcache name is not valid");
                    this.m_RuningOK = false;
                }
            }
        }


        // Properties
        public bool AllowNewTransfer
        {
            get
            {
                int num1 = this.rnd.Next(100);
                if (this.m_ConcurrentTransfers == 0)
                {
                    return (num1 <= 10);
                }
                return false;
            }
        }

        public uint IP
        {
            get
            {
                if ((DateTime.Now - this.m_LastNameLookup) > new TimeSpan(0, 15, 0))
                {
                    try
                    {
                        this.m_LastNameLookup = DateTime.Now;
                        IPHostEntry entry1 = Dns.GetHostByName(this.m_Name);
                        if (entry1.AddressList.Length > 0)
                        {
                            this.m_WebCacheIP = BitConverter.ToUInt32(entry1.AddressList[0].GetAddressBytes(), 0);
                        }
                    }
                    catch
                    {
                    }
                }
                return this.m_WebCacheIP;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        public ushort Port
        {
            get
            {
                return this.m_WebCachePort;
            }
        }

        public bool RuningOK
        {
            get
            {
                return this.m_RuningOK;
            }
        }

        public ArrayList SucceddBlocks
        {
            get
            {
                return this.m_SuccessWebcacheBlocks;
            }
        }

        public bool WebCacheMaxErrors
        {
            get
            {
                return (this.m_WebCacheErrors > 10);
            }
        }


        // Fields
        private int m_ConcurrentTransfers;
        private ArrayList m_FailedWebcacheBlocks;
        private DateTime m_LastNameLookup;
        private string m_Name;
        private ArrayList m_PendingBlocks;
        private bool m_RuningOK;
        private ArrayList m_SuccessWebcacheBlocks;
        private int m_WebCacheErrors;
        private uint m_WebCacheIP;
        private ushort m_WebCachePort;
        private Thread m_WebCacheThread;
        private Random rnd;
    }
}

