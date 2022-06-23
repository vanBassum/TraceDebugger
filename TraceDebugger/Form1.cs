using FRMLib;
using FRMLib.Scope;
using STDLib.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TraceDebugger
{
    public partial class Form1 : Form
    {
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
            scope.Settings.HorizontalToHumanReadable = (a) => a.ToString("F0") + "s";
            scope.Settings.HorSnapSize = 0.1f;

            Trace trace = new Trace { Name = "Temp", Offset = 0, Scale = 5, Layer = 9, Unit = "°C", Pen = Palettes.DistinctivePallet[2], DrawStyle = Trace.DrawStyles.Lines, DrawOption = Trace.DrawOptions.ShowScale };

            //string dateString = dt.ToString("dd-MM-yyyy") + "\r\n" + dt.ToString("HH:mm:ss");


            scope.Traces.Add(trace);

            trace.Points.Add(new PointD(0, 1));
            trace.Points.Add(new PointD(1, 2));
            trace.Points.Add(new PointD(2, 3));




        }
    }
}
