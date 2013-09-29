namespace eAnt.eDonkey
{
    using eAnt.Classes;
    using eAnt.Types;
    using System;
    using System.Collections;

    internal class CWebCacheClient : CClient
    {
        // Methods
        public CWebCacheClient(CCachedBlock block) : base(CKernel.WebCache.Port, CKernel.WebCache.IP, (uint) 0, (ushort) 0, block.Element.File.FileHash)
        {
            if (block.Element.File == null)
            {
                this.m_Completado = 1;
                this.m_DownloadState = Protocol.DownloadState.None;
                this.m_WebCacheClientConnection = null;
            }
            else
            {
                this.m_CachedBlock = block;
                this.m_Software = 0x36;
                this.m_DownloadElement = block.Element;
                CKernel.FilesList[this.DownFileHash].SourcesList.AddSource(this, true);
                this.m_WebCacheClientConnection = new CWebCacheClientConnection(CKernel.WebCache.IP, CKernel.WebCache.Port, this);
                this.m_ARC4Encoder = new CMARC4Decoder(block.Key);
                byte[] buffer1 = this.m_ARC4Encoder.Decript(this.m_DownloadElement.File.FileHash);
                string text1 = Convert.ToBase64String(buffer1);
                char[] chArray1 = new char[1] { '=' } ;
                text1 = text1.TrimEnd(chArray1);
                text1.Replace('/', '_');
                this.m_DownloadBlocks = new ArrayList();
                CFileBlock block1 = new CFileBlock();
                block1.start = block.StartOffset;
                block1.end = block.EndOffset;
                block1.FileHash = this.DownFileHash;
                block1.position = block.StartOffset;
                this.m_DownloadBlocks.Add(block1);
                this.m_Completado = 0;
                this.m_UserName = CKernel.WebCache.Name;
                this.m_DownloadState = Protocol.DownloadState.Downloading;
                this.m_WebCacheClientConnection.GetURL(block.HostPort, "", block.StartOffset, block.EndOffset, text1, 0);
            }
        }

        protected void m_CleanWebCacheBlocks()
        {
            if (this.m_DownloadBlocks.Count > 0)
            {
                this.m_DownloadElement.File.RemoveRequestedBlock(this.m_CachedBlock.RealStartOffset, this.m_CachedBlock.RealEndOffset);
            }
            this.m_DownloadBlocks.Clear();
        }

        private void m_ProcessCacheReceivedBlock(CReceivedBlock receivedBlock)
        {
            int num1 = 0;
            CKernel.GlobalStatistics.IncSessionDownWebCache(receivedBlock.Data.Length);
            lock (this.m_DownloadBlocks)
            {
                while (num1 < this.m_DownloadBlocks.Count)
                {
                    CFileBlock block1 = (CFileBlock) this.m_DownloadBlocks[num1];
                    if ((block1.start <= receivedBlock.Start) && ((block1.end + 1) >= receivedBlock.End))
                    {
                        if (block1.buffer == null)
                        {
                            block1.buffer = new byte[(block1.end + 1) - block1.start];
                        }
                        block1.position = receivedBlock.End;
                        System.Buffer.BlockCopy(receivedBlock.Data, 0, block1.buffer, (int) (receivedBlock.Start - block1.start), (int) (receivedBlock.End - receivedBlock.Start));
                        if ((block1.end + 1) == receivedBlock.End)
                        {
                            bool flag1;
                            uint num2;
                            receivedBlock.Data = null;
                            receivedBlock = null;
                            if ((this.m_CachedBlock.RealStartOffset == block1.start) && (this.m_CachedBlock.RealEndOffset == block1.end))
                            {
                                num2 = (this.m_CachedBlock.RealEndOffset - this.m_CachedBlock.RealStartOffset) + 1;
                                CLog.Log(Constants.Log.Verbose, "Writted complete webcache block " + this.m_DownloadElement.File.FileName + " size:" + num2.ToString());
                                flag1 = this.m_DownloadElement.File.WriteBlock(this.m_CachedBlock.RealStartOffset, this.m_CachedBlock.RealEndOffset, block1.buffer);
                                this.m_Completado = 2;
                                this.m_DownloadState = Protocol.DownloadState.None;
                            }
                            else
                            {
                                num2 = (this.m_CachedBlock.RealEndOffset - this.m_CachedBlock.RealStartOffset) + 1;
                                CLog.Log(Constants.Log.Verbose, "Writted webcache truncated block " + this.m_DownloadElement.File.FileName + " size:" + num2.ToString());
                                byte[] buffer1 = new byte[(this.m_CachedBlock.RealEndOffset - this.m_CachedBlock.RealStartOffset) + 1];
                                System.Buffer.BlockCopy(block1.buffer, (int) (this.m_CachedBlock.RealStartOffset - block1.start), buffer1, 0, buffer1.Length);
                                flag1 = this.m_DownloadElement.File.WriteBlock(this.m_CachedBlock.RealStartOffset, this.m_CachedBlock.RealEndOffset, buffer1);
                                this.m_Completado = 2;
                                this.m_DownloadState = Protocol.DownloadState.None;
                            }
                            if (!flag1)
                            {
                                this.m_DownloadElement.File.RemoveRequestedBlock(block1.start, block1.end);
                                CKernel.FilesList.StopFile(this.m_DownloadElement.File.FileHash);
                            }
                            if (this.m_DownloadElement != null)
                            {
                                this.m_DownloadElement.Statistics.IncSessionDownloadWebCache((block1.end - block1.start) + 1);
                            }
                            block1.buffer = null;
                            block1 = null;
                            if (this.m_DownloadBlocks.Count > 0)
                            {
                                this.m_DownloadBlocks.RemoveAt(num1);
                            }
                        }
                        return;
                    }
                    num1++;
                }
            }
        }

        public override void OnDisconnectWebCacheDown(int reason)
        {
            if (this.m_Completado == 0)
            {
                this.m_Completado = 1;
            }
            this.m_DownloadState = Protocol.DownloadState.None;
            this.m_WebCacheClientConnection = null;
            this.m_CleanWebCacheBlocks();
            this.DownFileHash = null;
            if (this.m_DownloadElement != null)
            {
                this.m_DownloadElement.SourcesList.RemoveSource(this);
            }
        }

        public override void ReceiveBlockWebCache(uint start, uint end, byte[] packet)
        {
            this.m_DownloadedBytes += ((uint) packet.Length);
            CReceivedBlock block1 = new CReceivedBlock();
            block1.Start = start;
            block1.End = end;
            block1.FileHash = this.DownFileHash;
            byte[] buffer1 = this.m_ARC4Encoder.Decript(packet);
            block1.Data = buffer1;
            this.m_ProcessCacheReceivedBlock(block1);
        }


        // Properties
        public bool ConnectionEnded
        {
            get
            {
                return (this.m_Completado > 0);
            }
        }

        public bool Success
        {
            get
            {
                return (this.m_Completado > 1);
            }
        }


        // Fields
        private CCachedBlock m_CachedBlock;
        private int m_Completado;
    }
}

