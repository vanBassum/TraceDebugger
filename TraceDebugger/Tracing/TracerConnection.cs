using STDLib.Ethernet;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace TraceDebugger.Tracing
{


    public class TracerConnection
    {
        TcpSocketClient socket;
        public event EventHandler<ICommand> CommandReceived;


        public TracerConnection()
        {
            socket = new TcpSocketClient();
        }

        public async Task ConnectAsync(string host)
        {
            await socket.ConnectAsync(host);
            socket.OnDataRecieved += Socket_OnDataRecieved;
        }

        private void Socket_OnDataRecieved(object sender, byte[] e)
        {
            if (e.Length < 1)
                return;

            if (GetCommand((PackageTypes)e[0]) is ICommand cmd)
            {
                if(cmd.Deserialize(e))
                    CommandReceived?.Invoke(this, cmd);
            }
        }

        ICommand GetCommand(PackageTypes type)
        {
            switch(type)
            {
                case PackageTypes.EVT_Measurement: return new CMD_EvtMeasurement();
                case PackageTypes.REQ_TraceInfo: return new CMD_RequestTraceInfo();
                case PackageTypes.RES_TraceInfo: return new CMD_ResponseTraceInfo();
                case PackageTypes.REQ_HorizontalInfo: return new CMD_RequestHorizontalInfo();
                case PackageTypes.RES_HorizontalInfo: return new CMD_ResponseHorizontalInfo();
                default: return null;
            }
        }




        /*
        public async Task<HorizontalInfo> RequestHorizontalInfo(CancellationTokenSource cts = null)
        {
            if (cts == null)
                (cts = new CancellationTokenSource()).CancelAfter(2500);
            cts.Token.Register(() => { tcs_HorizontalInfo.TrySetCanceled(); });
            socket.SendData(new byte[] { (byte)PackageTypes.REQ_HorizontalInfo});
            return await tcs_HorizontalInfo.Task;
        }

        public async Task<TraceInfo> RequestTraceInfo(int traceNo, CancellationTokenSource cts = null)
        {
            if (cts == null)
                (cts = new CancellationTokenSource()).CancelAfter(2500);
            tcs_TraceInfo = new TaskCompletionSource<TraceInfo>();
            cts.Token.Register(() => { tcs_TraceInfo.TrySetCanceled(); });
            socket.SendData(new byte[] { (byte)PackageTypes.REQ_TraceInfo, (byte)traceNo });
            return await tcs_TraceInfo.Task;
        }

        private void Socket_OnDataRecieved(object sender, byte[] e)
        {
            if (e.Length < 1)
                return;

            switch ((PackageTypes)e[0])
            {
                case PackageTypes.EVT_Measurement:
                    Measurement measurement = new Measurement();
                    measurement.Deserialize(e);
                    MeasurementReceived?.Invoke(this, measurement);
                    break;
                case PackageTypes.RES_HorizontalInfo:
                    HorizontalInfo horizontalInfo = new HorizontalInfo();
                    horizontalInfo.Deserialize(e);
                    tcs_HorizontalInfo.SetResult(horizontalInfo);
                    break;
                case PackageTypes.RES_TraceInfo:
                    TraceInfo traceInfo = new TraceInfo();
                    traceInfo.Deserialize(e);
                    tcs_TraceInfo.SetResult(traceInfo);
                    break;
            }
        }
        */
    }
}
