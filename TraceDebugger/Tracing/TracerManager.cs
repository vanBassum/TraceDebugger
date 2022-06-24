using System.Collections.Generic;
using System.Threading.Tasks;
using FRMLib.Scope;
using STDLib.Math;

namespace TraceDebugger.Tracing
{
    public class TracerManager
    {
        TracerConnection connection;
        ScopeController scopeController;
        Dictionary<int, Trace> traces = new Dictionary<int, Trace>();

        public TracerManager(ScopeController scopeController)
        {
            connection = new TracerConnection();
            connection.CommandReceived += Connection_CommandReceived;
            this.scopeController = scopeController;
        }

        public async Task ConnectAsync(string host)
        {
            await connection.ConnectAsync(host);

        }

        void HandleCommand(CMD_EvtMeasurement cmd)
        {
            Trace trace = null;
            if (!traces.TryGetValue(cmd.TraceNo, out trace))
            {
                traces[cmd.TraceNo] = trace = new Trace();
                scopeController.Traces.Add(trace);
                //Task.Run(async () =>
                //{
                //    TraceInfo traceInfo = await tracer.RequestTraceInfo(measurement.TraceNo);
                //    ApplyTraceInfo(traceInfo);
                //});
            }

            trace.Points.Add(new PointD(cmd.XValue, cmd.YValue));

            scopeController.RedrawAll();
            
        }


        private void Connection_CommandReceived(object sender, ICommand e)
        {
            switch (e)
            {
                case CMD_EvtMeasurement cmd:
                    HandleCommand(cmd);
                    break;
            }

        }

    }
}
