namespace DupFinUI.forms
{
    partial class ProgressForm
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
            Scanning = new Label();
            progressBar1 = new ProgressBar();
            lableStatus = new Label();
            SuspendLayout();
            // 
            // Scanning
            // 
            Scanning.AutoSize = true;
            Scanning.BackColor = Color.Transparent;
            Scanning.FlatStyle = FlatStyle.Popup;
            Scanning.Font = new Font("Cooper Black", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Scanning.ForeColor = Color.LightSeaGreen;
            Scanning.Location = new Point(437, 10);
            Scanning.Margin = new Padding(4, 0, 4, 0);
            Scanning.Name = "Scanning";
            Scanning.Size = new Size(219, 41);
            Scanning.TabIndex = 0;
            Scanning.Text = "Scanning...";
            Scanning.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // progressBar1
            // 
            progressBar1.Cursor = Cursors.WaitCursor;
            progressBar1.Location = new Point(-2, 346);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(1097, 27);
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.TabIndex = 1;
            // 
            // lableStatus
            // 
            lableStatus.AutoEllipsis = true;
            lableStatus.BackColor = Color.Transparent;
            lableStatus.BorderStyle = BorderStyle.Fixed3D;
            lableStatus.FlatStyle = FlatStyle.Popup;
            lableStatus.Font = new Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lableStatus.ForeColor = Color.LightSeaGreen;
            lableStatus.Location = new Point(13, 313);
            lableStatus.Margin = new Padding(4, 0, 4, 0);
            lableStatus.Name = "lableStatus";
            lableStatus.Size = new Size(1067, 30);
            lableStatus.TabIndex = 0;
            lableStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ProgressForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.DarkSlateGray;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(1093, 375);
            Controls.Add(lableStatus);
            Controls.Add(progressBar1);
            Controls.Add(Scanning);
            Font = new Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Margin = new Padding(4, 3, 4, 3);
            Name = "ProgressForm";
            Text = "DupFin";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Scanning;
        private Label lableStatus;
        private ProgressBar progressBar1;

    }
}