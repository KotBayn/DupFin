namespace DupFinUI.forms
{
    partial class MainForm
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
            Choosing = new Label();
            btnLaunchScan = new Button();
            textBoxPath = new TextBox();
            btnBrowse = new Button();
            comboHashTypeBox = new ComboBox();
            Selecting = new Label();
            SuspendLayout();
            // 
            // Choosing
            // 
            Choosing.AutoSize = true;
            Choosing.BackColor = Color.Transparent;
            Choosing.FlatStyle = FlatStyle.Popup;
            Choosing.Font = new Font("Cooper Black", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Choosing.ForeColor = Color.LightSeaGreen;
            Choosing.Location = new Point(196, 9);
            Choosing.Name = "Choosing";
            Choosing.Size = new Size(413, 41);
            Choosing.TabIndex = 0;
            Choosing.Text = "Select folder for scan:\r\n";
            Choosing.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnLaunchScan
            // 
            btnLaunchScan.AutoSize = true;
            btnLaunchScan.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnLaunchScan.BackColor = Color.LightSeaGreen;
            btnLaunchScan.Dock = DockStyle.Bottom;
            btnLaunchScan.FlatStyle = FlatStyle.Popup;
            btnLaunchScan.Font = new Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLaunchScan.ForeColor = Color.Aqua;
            btnLaunchScan.Location = new Point(0, 297);
            btnLaunchScan.Name = "btnLaunchScan";
            btnLaunchScan.Size = new Size(781, 38);
            btnLaunchScan.TabIndex = 2;
            btnLaunchScan.Text = "Start Scan!";
            btnLaunchScan.UseVisualStyleBackColor = false;
            btnLaunchScan.Click += btnLaunch_Click;
            // 
            // textBoxPath
            // 
            textBoxPath.BackColor = Color.LightSeaGreen;
            textBoxPath.Cursor = Cursors.IBeam;
            textBoxPath.Font = new Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBoxPath.ForeColor = Color.Aqua;
            textBoxPath.Location = new Point(50, 100);
            textBoxPath.Name = "textBoxPath";
            textBoxPath.Size = new Size(576, 35);
            textBoxPath.TabIndex = 3;
            textBoxPath.Text = "Enter the folder name...";
            textBoxPath.TextChanged += textBoxPath_TextChanged;
            // 
            // btnBrowse
            // 
            btnBrowse.BackColor = Color.LightSeaGreen;
            btnBrowse.FlatStyle = FlatStyle.Popup;
            btnBrowse.Font = new Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBrowse.ForeColor = Color.Aqua;
            btnBrowse.Location = new Point(630, 100);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(119, 34);
            btnBrowse.TabIndex = 4;
            btnBrowse.Text = "Browse...";
            btnBrowse.UseVisualStyleBackColor = false;
            btnBrowse.Click += btnBrowse_Click_1;
            // 
            // comboHashTypeBox
            // 
            comboHashTypeBox.BackColor = Color.LightSeaGreen;
            comboHashTypeBox.Cursor = Cursors.Cross;
            comboHashTypeBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboHashTypeBox.FlatStyle = FlatStyle.Popup;
            comboHashTypeBox.Font = new Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            comboHashTypeBox.ForeColor = Color.Aqua;
            comboHashTypeBox.FormattingEnabled = true;
            comboHashTypeBox.Items.AddRange(new object[] { "MD5", "SHA256", "SHA512" });
            comboHashTypeBox.Location = new Point(299, 212);
            comboHashTypeBox.Name = "comboHashTypeBox";
            comboHashTypeBox.Size = new Size(182, 36);
            comboHashTypeBox.TabIndex = 5;
            comboHashTypeBox.SelectedIndexChanged += comboHashTypeBox_SelectedIndexChanged;
            // 
            // Selecting
            // 
            Selecting.AutoSize = true;
            Selecting.Font = new Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Selecting.ForeColor = Color.Aqua;
            Selecting.Location = new Point(292, 158);
            Selecting.Name = "Selecting";
            Selecting.Size = new Size(196, 28);
            Selecting.TabIndex = 6;
            Selecting.Text = "Choose hash type:";
            Selecting.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkSlateGray;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(781, 335);
            Controls.Add(Selecting);
            Controls.Add(comboHashTypeBox);
            Controls.Add(btnBrowse);
            Controls.Add(textBoxPath);
            Controls.Add(btnLaunchScan);
            Controls.Add(Choosing);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Name = "MainForm";
            Text = "DupFin";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Choosing;
        private Button btnLaunchScan;
        private TextBox textBoxPath;
        private Button btnBrowse;
        private ComboBox comboHashTypeBox;
        private Label Selecting;
    }
}