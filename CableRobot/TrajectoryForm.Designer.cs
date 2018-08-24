namespace CableRobot
{
    partial class TrajectoryForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.outTextBox = new System.Windows.Forms.TextBox();
            this.inTextBox = new System.Windows.Forms.TextBox();
            this.saveCodeButton = new System.Windows.Forms.Button();
            this.exportSvgButton = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.imageExportFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.exportImageButton = new System.Windows.Forms.Button();
            this.importCodeButton = new System.Windows.Forms.Button();
            this.svgExportFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveCodeFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.importCodeFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.sendToRobotButton = new System.Windows.Forms.Button();
            this.clearCodeButton = new System.Windows.Forms.Button();
            this.importHpglButton = new System.Windows.Forms.Button();
            this.importHpglFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.importSvgButton = new System.Windows.Forms.Button();
            this.importSvgFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // outTextBox
            // 
            this.outTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.outTextBox.Location = new System.Drawing.Point(12, 303);
            this.outTextBox.MaxLength = 1048576;
            this.outTextBox.Multiline = true;
            this.outTextBox.Name = "outTextBox";
            this.outTextBox.ReadOnly = true;
            this.outTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outTextBox.Size = new System.Drawing.Size(1003, 222);
            this.outTextBox.TabIndex = 3;
            // 
            // inTextBox
            // 
            this.inTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.inTextBox.Location = new System.Drawing.Point(12, 531);
            this.inTextBox.Name = "inTextBox";
            this.inTextBox.Size = new System.Drawing.Size(1003, 38);
            this.inTextBox.TabIndex = 0;
            this.inTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextIn_KeyDown);
            // 
            // saveCodeButton
            // 
            this.saveCodeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveCodeButton.Location = new System.Drawing.Point(1021, 507);
            this.saveCodeButton.Name = "saveCodeButton";
            this.saveCodeButton.Size = new System.Drawing.Size(165, 28);
            this.saveCodeButton.TabIndex = 2;
            this.saveCodeButton.Text = "Save CRC";
            this.saveCodeButton.UseVisualStyleBackColor = true;
            this.saveCodeButton.Click += new System.EventHandler(this.saveCodeButton_Click);
            // 
            // exportSvgButton
            // 
            this.exportSvgButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exportSvgButton.Location = new System.Drawing.Point(1021, 473);
            this.exportSvgButton.Name = "exportSvgButton";
            this.exportSvgButton.Size = new System.Drawing.Size(165, 28);
            this.exportSvgButton.TabIndex = 1;
            this.exportSvgButton.Text = "Export SVG";
            this.exportSvgButton.UseVisualStyleBackColor = true;
            this.exportSvgButton.Click += new System.EventHandler(this.exportSvgButton_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(12, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(1174, 285);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 5;
            this.pictureBox.TabStop = false;
            this.pictureBox.SizeChanged += new System.EventHandler(this.pictureBox_SizeChanged);
            // 
            // imageExportFileDialog
            // 
            this.imageExportFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|BMP Image|*.bmp|All files|*.*";
            this.imageExportFileDialog.Title = "Export Image";
            // 
            // exportImageButton
            // 
            this.exportImageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exportImageButton.Location = new System.Drawing.Point(1021, 439);
            this.exportImageButton.Name = "exportImageButton";
            this.exportImageButton.Size = new System.Drawing.Size(165, 28);
            this.exportImageButton.TabIndex = 6;
            this.exportImageButton.Text = "Export Image";
            this.exportImageButton.UseVisualStyleBackColor = true;
            this.exportImageButton.Click += new System.EventHandler(this.exportImageButton_Click);
            // 
            // importCodeButton
            // 
            this.importCodeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.importCodeButton.Location = new System.Drawing.Point(1021, 371);
            this.importCodeButton.Name = "importCodeButton";
            this.importCodeButton.Size = new System.Drawing.Size(165, 28);
            this.importCodeButton.TabIndex = 7;
            this.importCodeButton.Text = "Import CRC";
            this.importCodeButton.UseVisualStyleBackColor = true;
            this.importCodeButton.Click += new System.EventHandler(this.importCodeButton_Click);
            // 
            // svgExportFileDialog
            // 
            this.svgExportFileDialog.Filter = "Scalable Vector Graphics|*.svg|All files|*.*";
            this.svgExportFileDialog.Title = "Export SVG";
            // 
            // saveCodeFileDialog
            // 
            this.saveCodeFileDialog.Filter = "Cable Robot Code|*.crc|All Files|*.*";
            this.saveCodeFileDialog.Title = "SaveCode";
            // 
            // importCodeFileDialog
            // 
            this.importCodeFileDialog.Filter = "Cable Robot Code|*.crc|All Files|*.*";
            this.importCodeFileDialog.Title = "Import Code";
            // 
            // sendToRobotButton
            // 
            this.sendToRobotButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sendToRobotButton.Location = new System.Drawing.Point(1021, 541);
            this.sendToRobotButton.Name = "sendToRobotButton";
            this.sendToRobotButton.Size = new System.Drawing.Size(165, 28);
            this.sendToRobotButton.TabIndex = 8;
            this.sendToRobotButton.Text = "Send To Robot...";
            this.sendToRobotButton.UseVisualStyleBackColor = true;
            this.sendToRobotButton.Click += new System.EventHandler(this.sendToRobotButton_Click);
            // 
            // clearCodeButton
            // 
            this.clearCodeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.clearCodeButton.Location = new System.Drawing.Point(1021, 405);
            this.clearCodeButton.Name = "clearCodeButton";
            this.clearCodeButton.Size = new System.Drawing.Size(165, 28);
            this.clearCodeButton.TabIndex = 9;
            this.clearCodeButton.Text = "Clear Code";
            this.clearCodeButton.UseVisualStyleBackColor = true;
            this.clearCodeButton.Click += new System.EventHandler(this.clearCodeButton_Click);
            // 
            // importHpglButton
            // 
            this.importHpglButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.importHpglButton.Location = new System.Drawing.Point(1021, 337);
            this.importHpglButton.Name = "importHpglButton";
            this.importHpglButton.Size = new System.Drawing.Size(165, 28);
            this.importHpglButton.TabIndex = 10;
            this.importHpglButton.Text = "Import HPGL";
            this.importHpglButton.UseVisualStyleBackColor = true;
            this.importHpglButton.Click += new System.EventHandler(this.importHpglButton_Click);
            // 
            // importHpglFileDialog
            // 
            this.importHpglFileDialog.Filter = "HP Graphics Language File|*.hpgl|All Files|*.*";
            // 
            // importSvgButton
            // 
            this.importSvgButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.importSvgButton.Location = new System.Drawing.Point(1021, 303);
            this.importSvgButton.Name = "importSvgButton";
            this.importSvgButton.Size = new System.Drawing.Size(165, 28);
            this.importSvgButton.TabIndex = 11;
            this.importSvgButton.Text = "Import SVG";
            this.importSvgButton.UseVisualStyleBackColor = true;
            this.importSvgButton.Click += new System.EventHandler(this.importSvgButton_Click);
            // 
            // importSvgFileDialog
            // 
            this.importSvgFileDialog.Filter = "Scalable Vector Graphics File|*.svg|All Files|*.*";
            // 
            // TrajectoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1198, 581);
            this.Controls.Add(this.importSvgButton);
            this.Controls.Add(this.importHpglButton);
            this.Controls.Add(this.clearCodeButton);
            this.Controls.Add(this.sendToRobotButton);
            this.Controls.Add(this.importCodeButton);
            this.Controls.Add(this.exportImageButton);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.exportSvgButton);
            this.Controls.Add(this.saveCodeButton);
            this.Controls.Add(this.inTextBox);
            this.Controls.Add(this.outTextBox);
            this.Name = "TrajectoryForm";
            this.Text = "Cable Robot Trajectory Planner";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox outTextBox;
        private System.Windows.Forms.TextBox inTextBox;
        private System.Windows.Forms.Button saveCodeButton;
        private System.Windows.Forms.Button exportSvgButton;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.SaveFileDialog imageExportFileDialog;
        private System.Windows.Forms.Button exportImageButton;
        private System.Windows.Forms.Button importCodeButton;
        private System.Windows.Forms.SaveFileDialog svgExportFileDialog;
        private System.Windows.Forms.SaveFileDialog saveCodeFileDialog;
        private System.Windows.Forms.OpenFileDialog importCodeFileDialog;
        private System.Windows.Forms.Button sendToRobotButton;
        private System.Windows.Forms.Button clearCodeButton;
        private System.Windows.Forms.Button importHpglButton;
        private System.Windows.Forms.OpenFileDialog importHpglFileDialog;
        private System.Windows.Forms.Button importSvgButton;
        private System.Windows.Forms.OpenFileDialog importSvgFileDialog;
    }
}

