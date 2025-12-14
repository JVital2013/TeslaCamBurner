namespace TeslaCamBurner
{
    public class ContainerInfo
    {
        public readonly UInt16 width, height;
        public readonly byte[] sps, pps;
        public readonly UInt32 timescale;
        public readonly List<double> durations = [];

        public ContainerInfo(string filePath)
        {
            byte[] byteBuffer = new byte[4];
            UInt16 spsLen, ppsLen;
            UInt32 entryCount, count, delta;
            double ms;

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Box moov = Box.FindBox(ref fs, 0, fs.Length, "moov");
            Box trak = Box.FindBox(ref fs, moov.start, moov.end, "trak");
            Box mdia = Box.FindBox(ref fs, trak.start, trak.end, "mdia");
            Box minf = Box.FindBox(ref fs, mdia.start, mdia.end, "minf");
            Box mdhd = Box.FindBox(ref fs, mdia.start, mdia.end, "mdhd");
            Box stbl = Box.FindBox(ref fs, minf.start, minf.end, "stbl");
            Box stts = Box.FindBox(ref fs, stbl.start, stbl.end, "stts");
            Box stsd = Box.FindBox(ref fs, stbl.start, stbl.end, "stsd");
            Box avc1 = Box.FindBox(ref fs, stsd.start + 8, stsd.end, "avc1");
            Box avcC = Box.FindBox(ref fs, avc1.start + 78, avc1.end, "avcC");

            // Extract SPS/PPS
            fs.Seek(avcC.start + 6, SeekOrigin.Begin);
            fs.ReadExactly(byteBuffer, 0, 2);
            spsLen = (UInt16)((byteBuffer[0] << 8) | byteBuffer[1]);
            sps = new byte[spsLen];
            fs.ReadExactly(sps, 0, spsLen);
            fs.Seek(1, SeekOrigin.Current);
            fs.ReadExactly(byteBuffer, 0, 2);
            ppsLen = (UInt16)((byteBuffer[0] << 8) | byteBuffer[1]);
            pps = new byte[ppsLen];
            fs.ReadExactly(pps, 0, ppsLen);

            // Get timescale from mdhd (ticks per second, used to convert stts deltas to ms)
            fs.Seek(mdhd.start, SeekOrigin.Begin);
            fs.ReadExactly(byteBuffer, 0, 1);
            fs.Seek((byteBuffer[0] == 1 ? mdhd.start + 20 : mdhd.start + 12), SeekOrigin.Begin);
            fs.ReadExactly(byteBuffer, 0, 4);
            timescale = (UInt32)((byteBuffer[0] << 24) | (byteBuffer[1] << 16) | (byteBuffer[2] << 8) | byteBuffer[3]);

            // Get frame durations from stts (delta ticks per frame -> converted to ms)
            fs.Seek(stts.start + 4, SeekOrigin.Begin);
            fs.ReadExactly(byteBuffer, 0, 4);
            entryCount = (UInt32)((byteBuffer[0] << 24) | (byteBuffer[1] << 16) | (byteBuffer[2] << 8) | byteBuffer[3]);
            for (int i = 0; i < entryCount; i++)
            {
                fs.ReadExactly(byteBuffer, 0, 4);
                count = (UInt32)((byteBuffer[0] << 24) | (byteBuffer[1] << 16) | (byteBuffer[2] << 8) | byteBuffer[3]);
                fs.ReadExactly(byteBuffer, 0, 4);
                delta = (UInt32)((byteBuffer[0] << 24) | (byteBuffer[1] << 16) | (byteBuffer[2] << 8) | byteBuffer[3]);
                ms = ((double)delta / (double)timescale) * 1000.0;
                for (int j = 0; j < count; j++) durations.Add(ms);
            }

            // Get width and height
            fs.Seek(avc1.start + 24, SeekOrigin.Begin);
            fs.ReadExactly(byteBuffer, 0, 4);
            width = (UInt16)((byteBuffer[0] << 8) | byteBuffer[1]);
            height = (UInt16)((byteBuffer[2] << 8) | byteBuffer[3]);

            // Clean up
            fs.Dispose();
        }
    }
}
