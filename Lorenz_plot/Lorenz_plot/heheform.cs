using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.WindowsForms;

namespace lorenz
{
    struct LorenzTrack
    {
        public LorenzSystem LorenzSystem { get; set; }
        public List<Tuple<double, double, double>> Trajectories { get; set; }
        public LineSeries LineSeries { get; set; }
        public System.Windows.Forms.Timer Timer { get; set; }
        public int CurrentIndex { get; set; }
        public Panel ControlPanel { get; set; }
    }

    public partial class Form1 : Form
    {
        private List<LorenzTrack> lorenzTracks = new List<LorenzTrack>();
        private PlotView plotView;
        private Panel settingsPanel;
        private TextBox textBoxO;
        private TextBox textBoxP;
        private TextBox textBoxB;
        private TextBox textBoxX;
        private TextBox textBoxY;
        private TextBox textBoxZ;
        private Button buttonAdd;

        private Label description;
        private ScatterSeries scatterSeries;

        public Form1()
        {
            InitializeComponent();
            InitializePlotView();
            InitializeSettingsPanel();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(1300, 800);
            this.Text = "Lorenz System Animation";
            this.ResumeLayout(false);
        }

        private void InitializePlotView()
        {
            plotView = new PlotView
            {
                Dock = DockStyle.None,
                Top = 0,
                Left = 0,
                Height = 800,
                Width = 800
            };
            var plotModel = new PlotModel { Title = "Lorenz System Trajectory" };
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X" });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y" });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Right, Title = "Z" });

            scatterSeries = new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerFill = OxyColors.Red
            };
            plotModel.Series.Add(scatterSeries);

            plotView.Model = plotModel;
            plotView.MouseDown += PlotView_MouseDown;
            plotView.MouseMove += PlotView_MouseMove;
            plotView.MouseUp += PlotView_MouseUp;

            this.Controls.Add(plotView);
        }

        private void InitializeSettingsPanel()
        {
            settingsPanel = new Panel
            {
                Top = 0,
                Left = 800,
                Width = 420,
                Height = this.ClientSize.Height, // Adjusted height for visible area
                AutoScroll = true
            };
            description = new Label { Top = 53, Height = 100, Left = 5, Width = 200, Text = "o:                                                  x: \n\np:                                                  y: \n\nb:                                                  z:" };

            textBoxO = new TextBox { Top = 50, Left = 20, Width = 140, Text = "10.0" };
            textBoxP = new TextBox { Top = 80, Left = 20, Width = 140, Text = "28.0" };
            textBoxB = new TextBox { Top = 110, Left = 20, Width = 140, Text = "2.67" };
            textBoxX = new TextBox { Top = 50, Left = 180, Width = 120, Text = "1.0" };
            textBoxY = new TextBox { Top = 80, Left = 180, Width = 120, Text = "1.0" };
            textBoxZ = new TextBox { Top = 110, Left = 180, Width = 120, Text = "1.0" };
            buttonAdd = new Button { Top = 150, Left = 20, Width = 280, Text = "Add Track" };

            buttonAdd.Click += ButtonAdd_Click;

            settingsPanel.Controls.Add(textBoxO);
            settingsPanel.Controls.Add(textBoxP);
            settingsPanel.Controls.Add(textBoxB);
            settingsPanel.Controls.Add(textBoxX);
            settingsPanel.Controls.Add(textBoxY);
            settingsPanel.Controls.Add(textBoxZ);
            settingsPanel.Controls.Add(buttonAdd);
            settingsPanel.Controls.Add(description);

            this.Controls.Add(settingsPanel);
        }
        private void ChangeTrackColor(int trackIndex, LineSeries lineSeries)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    var newColor = OxyColor.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                    lineSeries.Color = newColor;

                    // Update the button colors in the control panel
                    var panel = lorenzTracks[trackIndex].ControlPanel;
                    foreach (Button button in panel.Controls.OfType<Button>())
                    {
                        button.BackColor = colorDialog.Color;
                    }

                    plotView.InvalidatePlot(true); // Redraw plot with the new color
                }
            }
        }

        private void DeleteTrack(int trackIndex)
        {
            // Confirm deletion
            var result = MessageBox.Show("Are you sure you want to delete this track?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                // Stop and remove the timer
                var track = lorenzTracks[trackIndex];
                track.Timer.Stop();
                track.Timer.Dispose();

                // Remove the LineSeries from the plot
                plotView.Model.Series.Remove(track.LineSeries);

                // Remove the ControlPanel from the settingsPanel
                settingsPanel.Controls.Remove(track.ControlPanel);

                // Remove the LorenzTrack from the list
                lorenzTracks.RemoveAt(trackIndex);

                // Adjust positions of remaining control panels
                for (int i = trackIndex; i < lorenzTracks.Count; i++)
                {
                    lorenzTracks[i].ControlPanel.Top = 200 + i * 60;
                }

                // Ensure scroll bar appears by adjusting the scrollable area
                settingsPanel.AutoScrollMinSize = new Size(0, settingsPanel.Controls.Cast<Control>().Max(c => c.Bottom) + 20);

                // Redraw the plot
                plotView.InvalidatePlot(true);
            }
        }

        private Panel CreateControlPanel(int trackIndex, LineSeries lineSeries)
        {
            var panel = new Panel
            {
                Width = 410,
                Height = 50,
                BorderStyle = BorderStyle.FixedSingle,
                Top = 200 + trackIndex * 60 // Position based on the number of tracks
            };

            var buttonStart = new Button
            {
                Text = "Start",
                Width = 90,
                Height = 30,
                Top = 10,
                Left = 10,
                BackColor = Color.FromArgb(lineSeries.Color.R, lineSeries.Color.G, lineSeries.Color.B)
            };
            buttonStart.Click += (s, e) => lorenzTracks[trackIndex].Timer.Start();

            var buttonStop = new Button
            {
                Text = "Stop",
                Width = 90,
                Height = 30,
                Top = 10,
                Left = 110,
                BackColor = Color.FromArgb(lineSeries.Color.R, lineSeries.Color.G, lineSeries.Color.B)
            };
            buttonStop.Click += (s, e) => lorenzTracks[trackIndex].Timer.Stop();

            var buttonChangeColor = new Button
            {
                Text = "Change Color",
                Width = 90,
                Height = 30,
                Top = 10,
                Left = 210,
                BackColor = Color.FromArgb(lineSeries.Color.R, lineSeries.Color.G, lineSeries.Color.B)
            };
            buttonChangeColor.Click += (s, e) => ChangeTrackColor(trackIndex, lineSeries);

            var buttonDelete = new Button
            {
                Text = "Delete",
                Width = 90,
                Height = 30,
                Top = 10,
                Left = 310,
                BackColor = Color.Red,
                ForeColor = Color.White
            };
            buttonDelete.Click += (s, e) => DeleteTrack(trackIndex);

            panel.Controls.Add(buttonStart);
            panel.Controls.Add(buttonStop);
            panel.Controls.Add(buttonChangeColor);
            panel.Controls.Add(buttonDelete);

            return panel;
        }



        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (double.TryParse(textBoxO.Text, out double o) &&
                double.TryParse(textBoxP.Text, out double p) &&
                double.TryParse(textBoxB.Text, out double B) &&
                double.TryParse(textBoxX.Text, out double x) &&
                double.TryParse(textBoxY.Text, out double y) &&
                double.TryParse(textBoxZ.Text, out double z))
            {
                var lorenzSystem = new LorenzSystem(o, p, B);
                var trajectory = lorenzSystem.Solve(x, y, z, 2000.0, 500000);

                var lineSeries = new LineSeries { Color = OxyColors.Blue, LineStyle = LineStyle.Solid };
                plotView.Model.Series.Add(lineSeries);

                var timer = new System.Windows.Forms.Timer
                {
                    Interval = 1 // milliseconds
                };
                int currentIndex = 0;
                timer.Tick += (s, ev) => Timer_Tick(ref currentIndex, lineSeries, trajectory, lorenzTracks.Count);
                timer.Start();

                var controlPanel = CreateControlPanel(lorenzTracks.Count, lineSeries);
                controlPanel.Top = 200 + lorenzTracks.Count * 60; // Adjust position based on the number of tracks

                var lorenzTrack = new LorenzTrack
                {
                    LorenzSystem = lorenzSystem,
                    Trajectories = trajectory,
                    LineSeries = lineSeries,
                    Timer = timer,
                    CurrentIndex = 0,
                    ControlPanel = controlPanel
                };


                // Update scatterSeries with the initial point
                scatterSeries.Points.Clear();
                scatterSeries.Points.Add(new ScatterPoint(x, y));

                // Ensure scroll bar appears by adjusting the scrollable area
                settingsPanel.AutoScrollMinSize = new Size(0, controlPanel.Bottom + 20);
                lorenzTracks.Add(lorenzTrack);
                settingsPanel.Controls.Add(controlPanel);
            }
            else
            {
                MessageBox.Show("Invalid input values.", "Lorenz Plotter", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void PlotView_MouseDown(object sender, MouseEventArgs e)
        {
            var position = ScreenPointToPlotView(e.Location);
            if (scatterSeries.Points.Count > 0)
            {
                var scatterPoint = scatterSeries.Points[0];
                if (IsNear(scatterPoint, position))
                {
                    scatterSeries.Tag = true;
                }
            }
        }

        private void PlotView_MouseMove(object sender, MouseEventArgs e)
        {
            if (scatterSeries.Tag != null && (bool)scatterSeries.Tag)
            {
                var position = ScreenPointToPlotView(e.Location);
                scatterSeries.Points[0] = new ScatterPoint(position.X, position.Y);
                plotView.InvalidatePlot(true);
            }
        }

        private void PlotView_MouseUp(object sender, MouseEventArgs e)
        {
            if (scatterSeries.Tag != null && (bool)scatterSeries.Tag)
            {
                scatterSeries.Tag = null;

                var position = ScreenPointToPlotView(e.Location);
                textBoxX.Text = position.X.ToString();
                textBoxY.Text = position.Y.ToString();
            }
        }

        private DataPoint ScreenPointToPlotView(Point screenPoint)
        {
            var inverseTransform = plotView.Model.DefaultXAxis.InverseTransform(screenPoint.X, screenPoint.Y, plotView.Model.DefaultYAxis);
            return new DataPoint(inverseTransform.X, inverseTransform.Y);
        }

        private bool IsNear(ScatterPoint point, DataPoint position)
        {
            const double threshold = 1.0;
            return Math.Abs(point.X - position.X) < threshold && Math.Abs(point.Y - position.Y) < threshold;
        }

        private void Timer_Tick(ref int currentIndex, LineSeries series, List<Tuple<double, double, double>> trajectory, int index)
        {
            if (currentIndex < trajectory.Count)
            {
                var point = trajectory[currentIndex];
                series.Points.Add(new DataPoint(point.Item1, point.Item2));
                currentIndex++;
                plotView.InvalidatePlot(true); // Redraw plot
            }
            else
            {
                lorenzTracks[index - 1].Timer.Stop();
            }
        }
    }

}
