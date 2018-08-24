using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Svg;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace CableRobot
{
    public partial class TrajectoryForm : Form
    {
        private const int ExportStrokeWidth = 50;
        private const int ExportGridStrokeWidth = 2;
        private const int ExportGridSize = 50;

        private const int RenderStrokeWidth = 50;
        private const int RenderGridStrokeWidth = 10;
        private const int RenderGridSize = 100;

        private SvgGenerator _generator;
        private SvgDocument _svg;
        private readonly SvgRasterizer _svgRasterizer;
        private bool _modified = false;

        public TrajectoryForm()
        {
            InitializeComponent();

            _generator = new SvgGenerator();
            _svg = SvgDocument.FromSvg<SvgDocument>(_generator.GenerateCode(RenderStrokeWidth));
            _svgRasterizer = new SvgRasterizer(RenderGridStrokeWidth, RenderGridSize);
            RenderSvg();
        }

        private static ImageFormat SelectImageFormat(string path)
        {
            switch(Path.GetExtension(path).ToLower())
            {
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".bmp":
                    return ImageFormat.Bmp;

                default:
                case ".png":
                    return ImageFormat.Png;
            }
        }

        private bool ExecuteCommand(string text)
        {
            _modified = true;

            Command[] commands = null;
            try
            {
                commands = CrcParser.ParseCommands(text).ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Parser Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (commands != null)
            {
                Debug.WriteLine($"Parsed {commands.Length} commands");

                try
                {
                    foreach (var cmd in commands)
                        _generator.ExecuteCommand(cmd);
                    _svg = SvgDocument.FromSvg<SvgDocument>(_generator.GenerateCode(RenderStrokeWidth));
                    RenderSvg();

                    if (outTextBox.Text != "")
                        outTextBox.AppendText("\r\n");

                    outTextBox.AppendText(LineEndUtil.ToWindows(text));
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Render Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return false;
        }

        private void ExportImage(string path)
        {
            var code = _generator.GenerateCode(ExportStrokeWidth);
            var svg = SvgDocument.FromSvg<SvgDocument>(code);
            
            using (var bm = SvgRasterizer.Rasterize(svg, ExportGridStrokeWidth, ExportGridSize))
            using (var f = File.OpenWrite(path))
            {
                bm.Save(f, SelectImageFormat(path));
            }
        }

        #region Dialogs

        private void RenderSvg()
        {
            float wFactor = pictureBox.Width / _svg.Width;
            float hFactor = pictureBox.Height / _svg.Height;

            if (pictureBox.Width == 0 || pictureBox.Height == 0)
                return;

            float factor = Math.Min(wFactor, hFactor);

            int w = (int)(_svg.Width * factor), h = (int)(_svg.Height * factor);

            var pp = pictureBox.Image;
            
            pictureBox.Image = _svgRasterizer.Rasterize(_svg, new Point(w, h));
            pp?.Dispose();
        }

        private bool SaveCodeDialog()
        {
            if (saveCodeFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveCodeFileDialog.FileName, LineEndUtil.ToUnix(outTextBox.Text));
                    _modified = false;
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error encountered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return false;
        }

        private bool ExportImageDialog()
        {
            if (imageExportFileDialog.ShowDialog() == DialogResult.OK)
            {
                var dialog = new ProgressDialog("Exporting...");
                Task.Run(() => ExportImage(imageExportFileDialog.FileName))
                    .ContinueWith(task =>
                    {
                        Invoke(new Action(() => dialog.Close()));
                        if (task.IsFaulted)
                            Invoke(new Action(() => MessageBox.Show(task.Exception.InnerException.Message, "Error encountered", MessageBoxButtons.OK, MessageBoxIcon.Error)));
                    });
                dialog.ShowDialog();
            }
            return false;
        }

        private bool ExportSvgDialog()
        {
            if (svgExportFileDialog.ShowDialog() == DialogResult.OK)
                try
                {
                    File.WriteAllText(svgExportFileDialog.FileName, LineEndUtil.ToUnix(_generator.GenerateCode(ExportStrokeWidth)));
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error encountered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            return false;
        }

        private bool ImportCodeDialog()
        {
            if (!_modified || _modified && MessageBox.Show("You are going to lose all your work. Do you want to proceed?",
                "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (importCodeFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Preserve program state to recover if failed to load
                    var mod = _modified;
                    var buf = outTextBox.Text;
                    outTextBox.Text = "";
                    _generator = new SvgGenerator();
                    try
                    {
                        var code = File.ReadAllText(importCodeFileDialog.FileName);

                        if (ExecuteCommand(code))
                        {
                            _modified = false;
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error encountered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    // Something is wrong
                    outTextBox.Text = "";
                    _generator = new SvgGenerator();
                    ExecuteCommand(buf);
                    _modified = mod;
                }
            }
            return false;
        }

        private void SendToRobotControllerDialog(string code)
        {
            var lengths = CodeProcessor.CodeToPoints(code);
            new RobotControllerForm(lengths.ToArray()).ShowDialog();
        }

        private DialogResult UnsavedChangesDialog() 
            => MessageBox.Show("You have some unsaved work. Do you want to save it?", 
                "Save work?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        
        private void ClearCode()
        {
            outTextBox.Text = "";
            _generator = new SvgGenerator();
            ExecuteCommand("");
            _modified = false;
        }

        private bool ImportHpglDialog()
        {
            if (!_modified || _modified && MessageBox.Show("You are going to lose all your work. Do you want to proceed?",
                   "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (importHpglFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Preserve program state to recover if failed to load
                    var mod = _modified;
                    var buf = outTextBox.Text;
                    outTextBox.Text = "";
                    _generator = new SvgGenerator();
                    try
                    {
                        var code = File.ReadAllText(importHpglFileDialog.FileName);

                        if (ExecuteCommand(CodeProcessor.HpglToCrc(code)))
                        {
                            _modified = true;
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error encountered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    // Something is wrong
                    outTextBox.Text = "";
                    _generator = new SvgGenerator();
                    ExecuteCommand(buf);
                    _modified = mod;
                }
            }
            return false;
        }

        private bool ImportSvgDialog()
        {
            if (!_modified || _modified && MessageBox.Show("You are going to lose all your work. Do you want to proceed?",
                   "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (importSvgFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Preserve program state to recover if failed to load
                    var mod = _modified;
                    var buf = outTextBox.Text;
                    outTextBox.Text = "";
                    _generator = new SvgGenerator();
                    try
                    {
                        var code = File.ReadAllText(importSvgFileDialog.FileName);

                        if (ExecuteCommand(CodeProcessor.SvgToCrc(code)))
                        {
                            _modified = true;
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error encountered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    // Something is wrong
                    outTextBox.Text = "";
                    _generator = new SvgGenerator();
                    ExecuteCommand(buf);
                    _modified = mod;
                }
            }
            return false;
        }

        #endregion

        #region Events

        private void TextIn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ExecuteCommand(inTextBox.Text))
                    inTextBox.Text = "";
                e.Handled = true;
            }
        }

        private void exportImageButton_Click(object sender, EventArgs e) => ExportImageDialog();

        private void pictureBox_SizeChanged(object sender, EventArgs e)
        {
            if (_svg != null)
                RenderSvg();
        }

        private void exportSvgButton_Click(object sender, EventArgs e) => ExportSvgDialog();

        private void importCodeButton_Click(object sender, EventArgs e) => ImportCodeDialog();

        private void saveCodeButton_Click(object sender, EventArgs e) => SaveCodeDialog();

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_modified)
            {
                var dr = UnsavedChangesDialog();
                if (dr == DialogResult.No)
                    return;
                if (dr == DialogResult.Yes && SaveCodeDialog())
                    return;
                e.Cancel = true;
            }
        }

        private void clearCodeButton_Click(object sender, EventArgs e)
        {
            if (_modified)
            {
                var dr = UnsavedChangesDialog();
                if (dr == DialogResult.No || dr == DialogResult.Yes && SaveCodeDialog())
                    ClearCode();
            }
            else
                ClearCode();

        }

        private void sendToRobotButton_Click(object sender, EventArgs e) => SendToRobotControllerDialog(outTextBox.Text);

        private void importHpglButton_Click(object sender, EventArgs e) => ImportHpglDialog();

        private void importSvgButton_Click(object sender, EventArgs e) => ImportSvgDialog();

        #endregion
    }

}


