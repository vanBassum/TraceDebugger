using System;

namespace TraceDebugger.Tracing
{
    public class CMD_ResponseTraceInfo : ICommand
    {
        public PackageTypes PackageType => PackageTypes.RES_TraceInfo;

        public byte TraceNo { get; set; }
        public TraceTypes Type  { get; set; }
        public double Scale     { get; set; }
        public double Offset    { get; set; }


        public byte[] Serialize()
        {
            throw new NotImplementedException();
        }

        bool ICommand.Deserialize(byte[] data)
        {
            if (data.Length < 18) return false;
            //Command = data[0];
            TraceNo = data[1];
            Type = (TraceTypes)data[2];
            Scale = BitConverter.ToDouble(data, 3);
            Offset = BitConverter.ToDouble(data, 11);
            return true;
        }
    }
}
