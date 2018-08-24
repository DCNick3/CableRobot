using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CableRobot
{
    public partial class RobotControllerForm : Form
    {
        private readonly RobotContoller _contoller;
        private readonly Block<Vector3>[] _points;

        public RobotControllerForm(Block<Vector3>[] points)
        {
            InitializeComponent();
            _points = points;
            _contoller = new RobotContoller(InverseKinematicsComputer.ComputeLengths(points).ToArray(), new IPEndPoint(IPAddress.Parse("192.168.250.1"), 9600));
            _contoller.Log += ContollerOnLog;
            _contoller.ValuesUpdated += ContollerOnValuesUpdated;
            _contoller.StateUpdated += Contoller_StateUpdated;
            UpdateRuntimeParametersLabel();
            //RenderPoints();
        }

        private void Contoller_StateUpdated(object sender, RobotControllerState e)
        {
            Invoke(new Action(() => ShowState(_contoller.State)));
        }

        private void ContollerOnValuesUpdated(object sender, EventArgs e)
        {
            Invoke(new Action(UpdateRuntimeParametersLabel));
        }

        private void ContollerOnLog(object sender, LogEventArgs e)
        {
            var str = $"[{e.Time}]: {e.Message}\r\n";
            Invoke(new Action(() => logTextBox.AppendText(str)));
        }

        private Vector3 CurrentPosition()
        {
            var current = _contoller.GetCommandId();
            if (current.Item1 < _points.Length && current.Item2 < _points[current.Item1].Elements.Length)
            {
                return _points[current.Item1].Elements[current.Item2];
            }
            return new Vector3();
        }

        private void UpdateRuntimeParametersLabel()
        {
            string text;
            if (_contoller.State == RobotControllerState.Idle)
            {
                text =
                    $"{_contoller.State}\r\n" +
                    $"Code Blocks: {_contoller.BlockCount}\r\n" +
                    $"Commands: {_contoller.CommandCount}\r\n" +
                    $"Approximate Drawing Time: {_contoller.TotalTime.TotalMinutes:F1}m";
            }
            else
            {
                var v = CurrentPosition();
                text =
                    $"{_contoller.State}\r\n" +
                    $"Progress:\r\n" +
                    $"      {_contoller.BlockCount - _contoller.BlocksLeft:0000}/{_contoller.BlockCount:0000}\r\n" +
                    $"        ({_contoller.CommandCount - _contoller.CommandsLeft:0000000}/{_contoller.CommandCount:0000000})\r\n" +
                    $"Time left: {Math.Floor(_contoller.TimeLeft.TotalMinutes)}m {_contoller.TimeLeft.Seconds}s\r\n" +
                    $"Command Rate: {_contoller.Rate:F1}\r\n" +
                    $"Head position: \r\n" +
                    $"  {v.X:00000},{v.Y:00000},{v.Z:00000}";
            }
            RenderPoints();

            runtimeParametersLabel.Text = text;
        }

        private void ShowState(RobotControllerState state)
        {
            switch (state)
            {
                case RobotControllerState.Idle:
                    startButton.Enabled = true;
                    stopButton.Enabled = false;
                    pauseButton.Enabled = false;
                    break;
                case RobotControllerState.Suspended:
                    startButton.Enabled = true;
                    stopButton.Enabled = false;
                    pauseButton.Enabled = false;
                    break;
                case RobotControllerState.Working:
                    startButton.Enabled = false;
                    stopButton.Enabled = true;
                    pauseButton.Enabled = true;
                    break;
            }

            UpdateRuntimeParametersLabel();
        }

        private void SetState(RobotControllerState state)
        {
            if (_contoller.State != state)
                _contoller.SetStateAsync(state);
        }

        private void RenderPoints()
        {
            Vector2 sz = new Vector2(8000, 3800);
            var size = visualizationPictureBox.Size;
            var scale = Math.Min(size.Width / sz.X, size.Height / sz.Y);
            Point GetPoint(Vector3 p)
            {
                var pp = (new Vector2(p.X, -p.Y) + sz / 2.0) * scale;
                return new Point(Math.Min((int)pp.X, size.Width - 1), Math.Min((int)pp.Y, size.Height - 1));
            }

            Bitmap bm = new Bitmap((int)(sz * scale).X, (int)(sz * scale).Y);

            using (var g = Graphics.FromImage(bm))
            {
                g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, bm.Width, bm.Height));
            }

            int count = 0;
            foreach (var block in _points)
            {
                foreach (var point in block.Elements)
                {
                    if (point.Z < -1)
                    {
                        var ppp = GetPoint(point);
                        if (_contoller.CommandsExecuted > count)
                            bm.SetPixel(ppp.X, ppp.Y, Color.Lime);
                        else
                            bm.SetPixel(ppp.X, ppp.Y, Color.Black);
                    }
                    count++;
                }
            }

            using (var g = Graphics.FromImage(bm))
            {
                var pos = CurrentPosition();
                var dot = GetPoint(pos);
                var color = pos.Z < -1 ? Color.Blue : Color.Red;
                g.FillEllipse(new SolidBrush(color), dot.X - 2.5f, dot.Y - 2.5f, 5, 5);
            }

            var im = visualizationPictureBox.Image;
            visualizationPictureBox.Image = bm;
            im?.Dispose();
        }

        private void RobotControllerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_contoller.State != RobotControllerState.Idle)
            {
                e.Cancel = true;
                if (MessageBox.Show("Execution of trajectory will be stopped.\r\nDo you want to continue?",
                    "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _contoller.SetStateAsync(RobotControllerState.Idle)
                        .ContinueWith(task => Invoke(new Action(() => Close())));
                }
            }
        }

        private void emergencyStopButton_Click(object sender, EventArgs e)
        {
            _contoller.EmergencyStop();
        }

        private void startButton_Click(object sender, EventArgs e) => SetState(RobotControllerState.Working);
        private void stopButton_Click(object sender, EventArgs e) => SetState(RobotControllerState.Idle);
        private void pauseButton_Click(object sender, EventArgs e) => SetState(RobotControllerState.Suspended);

        private void RobotControllerForm_Closed(object sender, FormClosedEventArgs e)
        {
            _contoller.Dispose();
        }

        private void visualizationPictureBox_SizeChanged(object sender, EventArgs e)
        {
            if (_points != null)
                RenderPoints();
        }
    }

    public class LogEventArgs : EventArgs
    {
        public DateTime Time { get; set; }
        public string Message { get; set; }
    }
}
