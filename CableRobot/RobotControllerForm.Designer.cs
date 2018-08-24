using System.Drawing;
using System.Windows.Forms;

namespace CableRobot
{
    partial class RobotControllerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.visualizationPictureBox = new System.Windows.Forms.PictureBox();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.controlButtonsPanel = new System.Windows.Forms.Panel();
            this.emergencyStopButton = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.runtimeParametersLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.visualizationPictureBox)).BeginInit();
            this.controlButtonsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.visualizationPictureBox);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.logTextBox);
            this.splitContainer.Panel2.Controls.Add(this.controlButtonsPanel);
            this.splitContainer.Panel2.Controls.Add(this.runtimeParametersLabel);
            this.splitContainer.Size = new System.Drawing.Size(978, 521);
            this.splitContainer.SplitterDistance = 297;
            this.splitContainer.TabIndex = 0;
            // 
            // visualizationPictureBox
            // 
            this.visualizationPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.visualizationPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.visualizationPictureBox.Location = new System.Drawing.Point(12, 12);
            this.visualizationPictureBox.Name = "visualizationPictureBox";
            this.visualizationPictureBox.Size = new System.Drawing.Size(953, 282);
            this.visualizationPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.visualizationPictureBox.TabIndex = 1;
            this.visualizationPictureBox.TabStop = false;
            this.visualizationPictureBox.SizeChanged += new System.EventHandler(this.visualizationPictureBox_SizeChanged);
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logTextBox.Location = new System.Drawing.Point(428, 3);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(336, 208);
            this.logTextBox.TabIndex = 3;
            // 
            // controlButtonsPanel
            // 
            this.controlButtonsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controlButtonsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.controlButtonsPanel.Controls.Add(this.emergencyStopButton);
            this.controlButtonsPanel.Controls.Add(this.button5);
            this.controlButtonsPanel.Controls.Add(this.button4);
            this.controlButtonsPanel.Controls.Add(this.pauseButton);
            this.controlButtonsPanel.Controls.Add(this.stopButton);
            this.controlButtonsPanel.Controls.Add(this.startButton);
            this.controlButtonsPanel.Location = new System.Drawing.Point(780, 3);
            this.controlButtonsPanel.Name = "controlButtonsPanel";
            this.controlButtonsPanel.Size = new System.Drawing.Size(185, 208);
            this.controlButtonsPanel.TabIndex = 2;
            // 
            // emergencyStopButton
            // 
            this.emergencyStopButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.emergencyStopButton.BackColor = System.Drawing.Color.Red;
            this.emergencyStopButton.Font = new System.Drawing.Font("Comic Sans MS", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.emergencyStopButton.Location = new System.Drawing.Point(3, 3);
            this.emergencyStopButton.Name = "emergencyStopButton";
            this.emergencyStopButton.Size = new System.Drawing.Size(177, 28);
            this.emergencyStopButton.TabIndex = 6;
            this.emergencyStopButton.Text = "Emergency Stop";
            this.emergencyStopButton.UseVisualStyleBackColor = false;
            this.emergencyStopButton.Click += new System.EventHandler(this.emergencyStopButton_Click);
            // 
            // button5
            // 
            this.button5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button5.Location = new System.Drawing.Point(3, 37);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(177, 28);
            this.button5.TabIndex = 5;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button4.Location = new System.Drawing.Point(3, 71);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(177, 28);
            this.button4.TabIndex = 4;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // pauseButton
            // 
            this.pauseButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pauseButton.Enabled = false;
            this.pauseButton.Location = new System.Drawing.Point(3, 105);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(177, 28);
            this.pauseButton.TabIndex = 3;
            this.pauseButton.Text = "Suspend";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(3, 139);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(177, 28);
            this.stopButton.TabIndex = 2;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.startButton.Location = new System.Drawing.Point(3, 173);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(177, 28);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // runtimeParametersLabel
            // 
            this.runtimeParametersLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.runtimeParametersLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.runtimeParametersLabel.Font = new System.Drawing.Font("Courier New", 12F);
            this.runtimeParametersLabel.Location = new System.Drawing.Point(12, 3);
            this.runtimeParametersLabel.Name = "runtimeParametersLabel";
            this.runtimeParametersLabel.Size = new System.Drawing.Size(400, 208);
            this.runtimeParametersLabel.TabIndex = 0;
            // 
            // RobotControllerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 521);
            this.Controls.Add(this.splitContainer);
            this.Name = "RobotControllerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Robot Controller";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RobotControllerForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RobotControllerForm_Closed);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.visualizationPictureBox)).EndInit();
            this.controlButtonsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.PictureBox visualizationPictureBox;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.Panel controlButtonsPanel;
        private System.Windows.Forms.Label runtimeParametersLabel;
        private System.Windows.Forms.Button emergencyStopButton;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
    }
}