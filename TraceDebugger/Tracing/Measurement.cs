using System;

namespace TraceDebugger.Tracing
{
    public class Measurement
    {
        public byte Command { get; set; }
        public byte TraceNo { get; set; }
        public double XValue { get; set; }
        public double YValue { get; set; }

        public void Deserialize(byte[] data)
        {
            if (data.Length < 18) return;
            Command = data[0];
            TraceNo = data[1];
            XValue = BitConverter.ToDouble(data, 2);
            YValue = BitConverter.ToDouble(data, 10);
        }
    };
}
