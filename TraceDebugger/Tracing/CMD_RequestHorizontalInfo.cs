namespace TraceDebugger.Tracing
{
    public class CMD_RequestHorizontalInfo : ICommand
    {
        public PackageTypes PackageType => PackageTypes.REQ_HorizontalInfo;

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
