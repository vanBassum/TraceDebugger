using System;

namespace TraceDebugger.Tracing
{
    public class CMD_EvtMeasurement : ICommand
    {

        public PackageTypes PackageType => PackageTypes.EVT_Measurement;
        public byte TraceNo { get; set; }
        public double XValue { get; set; }
        public double YValue { get; set; }

        public bool Deserialize(byte[] data)
        {
            if (data.Length < 18) return false;
            //Command = data[0];
            TraceNo = data[1];
            XValue = BitConverter.ToDouble(data, 2);
            YValue = BitConverter.ToDouble(data, 10);
            return true;
        }

        public byte[] Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
