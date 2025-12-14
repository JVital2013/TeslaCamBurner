namespace TeslaCamBurner
{
    public class Parser
    {
        private byte[] nalBuffer = [];
        private List<byte> strippedNalBuffer = [];

        public SeiMetadata?[] Parse(string FilePath)
        {
            List<SeiMetadata?> messages = [];
            SeiMetadata? pendingSei = null;
            FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            Box mdat = Box.FindBox(ref fs, 0, fs.Length, "mdat");
            byte[] byteBuffer = new byte[4];
            UInt32 nalSize;

            fs.Seek(mdat.start, SeekOrigin.Begin);
            while (fs.Position + 4 < mdat.end)
            {
                fs.ReadExactly(byteBuffer, 0, 4);
                nalSize = (UInt32)((byteBuffer[0] << 24) | (byteBuffer[1] << 16) | (byteBuffer[2] << 8) | byteBuffer[3]);

                if (nalSize < 2 || fs.Position + nalSize > fs.Length)
                {
                    fs.Seek(Math.Max(nalSize, 0), SeekOrigin.Current);
                    continue;
                }

                nalBuffer = new byte[nalSize];
                fs.ReadExactly(nalBuffer, 0, (int)nalSize);
                int type = nalBuffer[0] & 0x1f;
                if (type == 6 && nalBuffer[1] == 5) // NAL type 6 = SEI, payload type 5 = user data unregistered
                {
                    pendingSei = DecodeSEI();
                }
                else if (type == 1 || type == 5)
                {
                    messages.Add(pendingSei);
                    pendingSei = null;
                }
            }

            fs.Dispose();
            Cleanup();

            return [.. messages];
        }

        private SeiMetadata? DecodeSEI()
        {
            if (nalBuffer.Length < 4) return null;

            int i = 3;
            while (i < nalBuffer.Length && nalBuffer[i] == 0x42) i++;
            if (i <= 3 || i + 1 >= nalBuffer.Length || nalBuffer[i] != 0x69) return null;
            try
            {
                StripEmulationBytes(i + 1);
                return SeiMetadata.Parser.ParseFrom([.. strippedNalBuffer]);
            }
            catch
            {
                return null;
            }
        }

        private void StripEmulationBytes(int startIndex)
        {
            strippedNalBuffer = [];
            int zeros = 0;
            for (int i = startIndex; i < nalBuffer.Length - 1; i++)
            {
                if (zeros >= 2 && nalBuffer[i] == 0x03)
                {
                    zeros = 0;
                    continue;
                }

                strippedNalBuffer.Add(nalBuffer[i]);
                zeros = nalBuffer[i] == 0 ? zeros + 1 : 0;
            }
        }

        private void Cleanup()
        {
            nalBuffer = [];
            strippedNalBuffer = [];
        }
    }
}
