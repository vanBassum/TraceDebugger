namespace TraceDebugger.Tracing
{
    public interface ICommand
    {
        PackageTypes PackageType { get; }

        bool Deserialize(byte[] data);
        byte[] Serialize();

    }
}
