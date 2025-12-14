using System.Text;

namespace TeslaCamBurner
{
    internal class Box
    {
        public readonly Int64 start;
        public readonly Int64 end;
        public readonly Int64 size;

        Box(Int64 start, Int64 end, Int64 size)
        {
            this.start = start;
            this.end = end;
            this.size = size;
        }

        internal static Box FindBox(ref FileStream fs, Int64 start, Int64 end, string name)
        {
            byte[] byteBuffer = new byte[8];
            Int64 pos = start;

            while (pos + 8 < end)
            {
                UInt64 size = 0;
                UInt32 headerSize = 8;
                string type;

                fs.Seek(pos, SeekOrigin.Begin);
                fs.ReadExactly(byteBuffer, 0, 4);
                size = (UInt64)((byteBuffer[0] << 24) | (byteBuffer[1] << 16) | (byteBuffer[2] << 8) | byteBuffer[3]);
                fs.ReadExactly(byteBuffer, 0, 4);
                type = Encoding.ASCII.GetString(byteBuffer, 0, 4);

                if (size == 1)
                {
                    headerSize = 16;
                    fs.ReadExactly(byteBuffer, 0, 8);
                    size = (UInt64)((byteBuffer[0] << 56) | (byteBuffer[1] << 48) | (byteBuffer[2] << 40) | (byteBuffer[3] << 32) |
                        (byteBuffer[4] << 24) | (byteBuffer[5] << 16) | (byteBuffer[6] << 8) | byteBuffer[7]);
                }
                else if (size == 0)
                {
                    size = (UInt64)(end - pos);
                }

                if (type == name)
                {
                    return new Box(pos + headerSize, pos + (long)size, (long)size - headerSize);
                }

                pos += (Int64)size;
            }

            throw new Exception($"Box {name} not found");
        }
    }
}
