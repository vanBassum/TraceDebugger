namespace TraceDebugger.Tracing
{
    public class CMD_ResponseHorizontalInfo : ICommand
    {
        public PackageTypes PackageType => PackageTypes.RES_HorizontalInfo;
        public HorizontalTypes Type  { get; set; }
        public double Scale          { get; set; }
        public double Offset         { get; set; }

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
