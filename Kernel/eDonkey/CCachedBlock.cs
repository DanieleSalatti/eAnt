namespace eAnt.eDonkey
{
    using eAnt.Types;
    using System;
    using System.Net;

    internal class CCachedBlock
    {
        // Methods
        public CCachedBlock(CCmdCachedBlock block, CClient client2)
        {
            this.Element = CKernel.FilesList[block.FileHash];
            this.StartOffset = block.StartOffset;
            this.EndOffset = block.EndOffset;
            this.RealStartOffset = this.StartOffset;
            this.RealEndOffset = this.EndOffset;
            this.ID = block.Id;
            if (((this.Element == null) || this.Element.File.Completed) || ((this.Element.File.FileStatus != Protocol.FileState.Ready) || !this.Element.File.GetNewBlock(ref this.RealStartOffset, ref this.RealEndOffset)))
            {
                this.Element = null;
                CLog.Log(Constants.Log.Verbose, "Cached block not needed");
            }
            else
            {
                this.Key = block.RemoteKey;
                string text1 = new IPAddress((long) block.IPClient).ToString();
                this.HostPort = text1 + ":" + block.PortClient.ToString();
                this.IPClient = block.IPClient;
                this.PortClient = block.PortClient;
            }
        }


        // Fields
        public CElement Element;
        public uint EndOffset;
        public string HostPort;
        public uint ID;
        public uint IPClient;
        public byte[] Key;
        public ushort PortClient;
        public uint RealEndOffset;
        public uint RealStartOffset;
        public uint StartOffset;
    }
}

