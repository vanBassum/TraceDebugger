namespace TraceDebugger.Tracing
{
    public class CMD_RequestTraceInfo : ICommand
    {
        public PackageTypes PackageType => PackageTypes.REQ_TraceInfo;
        public byte traceNo { get; set; }

        public bool Deserialize(byte[] data)
        {
            throw new System.NotImplementedException();
        }

        public byte[] Serialize()
        {
            throw new System.NotImplementedException();
        }
    }
}
