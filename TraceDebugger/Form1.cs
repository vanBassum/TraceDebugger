using FRMLib;
using FRMLib.Scope;
using STDLib.Math;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TraceDebugger.Tracing;

namespace TraceDebugger
{
    public partial class Form1 : Form
    {
        TracerConnection tracerManager;
        ScopeController scope;
        

        public Form1()
        {
            InitializeComponent();
            menuStrip1.AddMenuItem("File/New", () => scope.ClearData());
            //menuStrip1.AddMenuItem("File/Open...", OpenFromFileDialog);
            //menuStrip1.AddMenuItem("File/Save As...", SaveToFileDialog);
            menuStrip1.AddMenuItem("File/Close", () => this.Close());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            scope = new ScopeController();
            traceView1.DataSource = scope;
            scopeView1.DataSource = scope;
            markerView1.DataSource = scope;

            scope.Settings.DrawScalePosVertical = DrawPosVertical.Right;
            scope.Settings.DrawScalePosHorizontal = DrawPosHorizontal.Bottom;
            scope.Settings.HorizontalDivisions = 10;
            //scope.Settings.HorScale = TimeSpan.FromMinutes(10).Ticks;
            //scope.Settings.HorOffset = DateTime.Now.Ticks;
            //scope.Settings.HorizontalToHumanReadable = (a) => (a / 1000000).ToString("F0") + "s";
            //scope.Settings.HorSnapSize = 1000000f;


            scope.Settings.HorizontalToHumanReadable = TicksToString;

            tracerManager = new TracerConnection();
            tracerManager.ConnectAsync("192.168.11.120:31600");
            tracerManager.CommandReceived += Tracer_MeasurementReceived;

        }

        static string TicksToString(double ticks)
        {
            DateTime dt = new DateTime((long)ticks);
            return dt.ToString("dd-MM-yyyy") + " \r\n" + dt.ToString("HH:mm:ss");
        }

        private void Tracer_MeasurementReceived(object sender, ICommand e)
        {
            switch (e)
            {
                case CMD_EvtMeasurement cmd:
                    HandleCommand(cmd);
                    break;
            }

        }


        Dictionary<int, Trace> traces = new Dictionary<int, Trace>();
        void HandleCommand(CMD_EvtMeasurement cmd)
        {
            Trace trace = null;
            if (!traces.TryGetValue(cmd.TraceNo, out trace))
            {
                traces[cmd.TraceNo] = trace = new Trace();
                scope.Traces.Add(trace);
                //Task.Run(async () =>
                //{
                //    TraceInfo traceInfo = await tracer.RequestTraceInfo(measurement.TraceNo);
                //    ApplyTraceInfo(traceInfo);
                //});
            }

            var dt = DateTimeOffset.FromUnixTimeSeconds((long)cmd.XValue).DateTime;

            trace.Points.Add(new PointD(dt.Ticks, cmd.YValue));


            this.InvokeIfRequired(()=> scope.RedrawAll());

        }


        



        /*
        private void Tracer_MeasurementReceived(object sender, Measurement measurement)
        {
            Trace trace = null;
            if (!traces.TryGetValue(measurement.TraceNo, out trace))
            {
                traces[measurement.TraceNo] = trace = new Trace();
                scope.Traces.Add(trace);
                //Task.Run(async () =>
                //{
                //    TraceInfo traceInfo = await tracer.RequestTraceInfo(measurement.TraceNo);
                //    ApplyTraceInfo(traceInfo);
                //});
            }

            trace.Points.Add(new PointD(measurement.XValue, measurement.YValue));

            this.InvokeIfRequired(() =>
            {
                scopeView1.DrawAll();
            });
        }

        void ApplyTraceInfo(TraceInfo info)
        {
            if (traces.TryGetValue(info.TraceNo, out Trace trace))
            {
                trace.Offset = info.Offset;
                trace.Scale = info.Scale;

                switch ((TraceTypes)info.Type)
                {
                    case TraceTypes.Analog:
                        trace.DrawStyle = Trace.DrawStyles.Lines;
                        break;
                    case TraceTypes.State:
                        trace.DrawStyle = Trace.DrawStyles.State;
                        break;
                }
            }
        }

        void ApplyHorizontalInfo(HorizontalInfo info)
        {

            switch ((HorTypes)info.Type)
            {
                case HorTypes.Timestamp:
                    scope.Settings.HorizontalToHumanReadable = (ticks) =>
                    {
                        DateTime dt = new DateTime((long)ticks);
                        return dt.ToString("dd-MM-yyyy") + " \r\n" + dt.ToString("HH:mm:ss");
                    };
                    scope.Settings.HorOffset = TimeSpan.FromSeconds(info.Offset).Ticks;
                    scope.Settings.HorScale = TimeSpan.FromSeconds(info.Scale).Ticks;
                    scope.Settings.HorSnapSize = TimeSpan.FromSeconds(1).Ticks;
                    break;
                case HorTypes.SecondsSinceStart:
                    scope.Settings.HorizontalToHumanReadable = (x) => x.ToString("F1") + "s";
                    scope.Settings.HorOffset = info.Offset;
                    scope.Settings.HorScale = info.Scale;
                    scope.Settings.HorSnapSize = 1;
                    break;
            }


        }
        */

        private async void button1_Click(object sender, EventArgs e)
        {
            //if (sender is Button button)
            //{
            //    button.Enabled = false;
            //    var horInfo = await tracer.RequestHorizontalInfo();
            //    ApplyHorizontalInfo(horInfo);
            //    button.Enabled = true;
            //}
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            //if (sender is Button button)
            //{
            //    button.Enabled = false;
            //    var info = await tracer.RequestTraceInfo(1);
            //    ApplyTraceInfo(info);
            //    button.Enabled = true;
            //}
        }
    }

}
