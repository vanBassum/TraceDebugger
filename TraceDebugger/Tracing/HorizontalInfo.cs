using System;

namespace TraceDebugger.Tracing
{
    public class HorizontalInfo
    {
        public byte Command { get; set; }
        public byte Type { get; set; }
        public double Scale { get; set; } //seconds
        public double Offset { get; set; } // seconds

        public void Deserialize(byte[] data)
        {
            if (data.Length < 18) return;
            Command = data[0];
            Type = data[1];
            Scale = BitConverter.ToDouble(data, 2);
            Offset = BitConverter.ToDouble(data, 10);
        }
    }
}
