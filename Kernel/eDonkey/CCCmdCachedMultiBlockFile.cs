namespace eAnt.eDonkey
{
    using System;
    using System.Collections;
    using System.IO;

    internal class CCCmdCachedMultiBlockFile
    {
        // Methods
        public CCCmdCachedMultiBlockFile(MemoryStream buffer)
        {
            this.Blocks = new ArrayList();
            BinaryReader reader1 = new BinaryReader(buffer);
            this.n = reader1.ReadUInt16();
            uint num1 = reader1.ReadUInt32();
            byte[] buffer1 = reader1.ReadBytes(0x10);
            for (int num2 = 0; num2 < this.n; num2++)
            {
                CCmdCachedBlock block1 = new CCmdCachedBlock();
                block1.IPProxy = num1;
                block1.IPClient = reader1.ReadUInt32();
                block1.PortClient = reader1.ReadUInt16();
                block1.FileHash = buffer1;
                block1.StartOffset = reader1.ReadUInt32();
                block1.EndOffset = reader1.ReadUInt32();
                block1.RemoteKey = reader1.ReadBytes(0x10);
                block1.Id = reader1.ReadUInt32();
                this.Blocks.Add(block1);
            }
        }

        public CCCmdCachedMultiBlockFile(ArrayList cachedBlocks, uint IPProxy, byte[] fileHash, byte[] fileStatus, MemoryStream buffer)
        {
            if ((cachedBlocks != null) && (cachedBlocks.Count != 0))
            {
                BinaryWriter writer1 = new BinaryWriter(buffer);
                DonkeyHeader header1 = new DonkeyHeader(0x21, writer1, Protocol.ProtocolType.Ant);
                this.n = 0;
                writer1.Write(this.n);
                writer1.Write(IPProxy);
                writer1.Write(fileHash);
                foreach (CCachedBlock block1 in cachedBlocks)
                {
                    if (((block1.Element == null) || (block1.Element.File == null)) || !CKernel.SameHash(ref fileHash, ref block1.Element.File.FileHash))
                    {
                        continue;
                    }
                    uint num1 = CHash.GetChunkNumberAt(block1.StartOffset);
                    if (fileStatus[(int) ((IntPtr) num1)] == 0)
                    {
                        writer1.Write(block1.IPClient);
                        writer1.Write(block1.PortClient);
                        writer1.Write(block1.StartOffset);
                        writer1.Write(block1.EndOffset);
                        writer1.Write(block1.Key);
                        writer1.Write(block1.ID);
                        this.n = (ushort) (this.n + 1);
                    }
                }
                header1.Packetlength = (((uint) writer1.BaseStream.Length) - header1.Packetlength) + 1;
                writer1.Seek(0, SeekOrigin.Begin);
                header1.Serialize(writer1);
                writer1.Write(this.n);
            }
        }


        // Fields
        public ArrayList Blocks;
        public ushort n;
    }
}

