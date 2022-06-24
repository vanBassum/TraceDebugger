using FRMLib;
using FRMLib.Scope;
using STDLib.Math;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TraceDebugger
{
    public partial class Form1 : Form
    {
        TracerConnection tracer;
        ScopeController scope;
        Dictionary<int, Trace> traces = new Dictionary<int, Trace>();

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
            tracer = new TracerConnection();
            traceView1.DataSource = scope;
            scopeView1.DataSource = scope;
            markerView1.DataSource = scope;

            scope.Settings.DrawScalePosVertical = DrawPosVertical.Right;
            scope.Settings.DrawScalePosHorizontal = DrawPosHorizontal.Bottom;
            scope.Settings.HorizontalDivisions = 10;
            scope.Settings.HorizontalToHumanReadable = (a) => (a / 1000000).ToString("F0") + "s";
            scope.Settings.HorSnapSize = 1000000f;

            tracer.Connect("192.168.35.109:31600");
            tracer.MeasurementReceived += Tracer_MeasurementReceived;

        }

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

        private async void button1_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.Enabled = false;
                var horInfo = await tracer.RequestHorizontalInfo();
                ApplyHorizontalInfo(horInfo);
                button.Enabled = true;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.Enabled = false;
                var info = await tracer.RequestTraceInfo(1);
                ApplyTraceInfo(info);
                button.Enabled = true;
            }
        }
    }

}
