using System;

namespace TraceDebugger.Tracing
{
    public class TraceInfo
    {
        public byte Command { get; set; }
        public byte TraceNo { get; set; }
        public byte Type { get; set; }
        public double Scale { get; set; } //seconds
        public double Offset { get; set; } // seconds

        public void Deserialize(byte[] data)
        {
            if (data.Length < 18) return;
            Command = data[0];
            TraceNo = data[1];
            Type = data[2];
            Scale = BitConverter.ToDouble(data, 3);
            Offset = BitConverter.ToDouble(data, 11);
        }
    }
}
