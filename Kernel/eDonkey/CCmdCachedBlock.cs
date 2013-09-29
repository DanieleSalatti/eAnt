namespace eAnt.eDonkey
{
    using System;
    using System.IO;

    internal class CCmdCachedBlock
    {
        // Methods
        public CCmdCachedBlock()
        {
        }

        public CCmdCachedBlock(MemoryStream buffer)
        {
            BinaryReader reader1 = new BinaryReader(buffer);
            this.IPProxy = reader1.ReadUInt32();
            this.IPClient = reader1.ReadUInt32();
            this.PortClient = reader1.ReadUInt16();
            this.FileHash = reader1.ReadBytes(0x10);
            this.StartOffset = reader1.ReadUInt32();
            this.EndOffset = reader1.ReadUInt32();
            this.RemoteKey = reader1.ReadBytes(0x10);
            if (reader1.BaseStream.Position <= (reader1.BaseStream.Length - 4))
            {
                this.Id = reader1.ReadUInt32();
            }
        }

        public CCmdCachedBlock(uint Id, uint IPProxy, uint IPClient, ushort PortClient, byte[] FileHash, uint StartOffset, uint EndOffset, byte[] RemoteKey, MemoryStream buffer)
        {
            BinaryWriter writer1 = new BinaryWriter(buffer);
            writer1.Write((byte) 0x57);
            writer1.Write((byte) 0xff);
            writer1.Write(IPProxy);
            writer1.Write(IPClient);
            writer1.Write(PortClient);
            writer1.Write(FileHash);
            writer1.Write(StartOffset);
            writer1.Write(EndOffset);
            writer1.Write(RemoteKey);
            writer1.Write(Id);
        }


        // Fields
        public uint EndOffset;
        public byte[] FileHash;
        public uint Id;
        public uint IPClient;
        public uint IPProxy;
        public ushort PortClient;
        public byte[] RemoteKey;
        public uint StartOffset;
    }
}

