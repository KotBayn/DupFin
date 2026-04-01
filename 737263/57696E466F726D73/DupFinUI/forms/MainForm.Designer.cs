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
            btnWelcome = new Button();
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
            // btnWelcome
            // 
            btnWelcome.BackColor = Color.LightSeaGreen;
            btnWelcome.Dock = DockStyle.Bottom;
            btnWelcome.FlatStyle = FlatStyle.Popup;
            btnWelcome.Font = new Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnWelcome.ForeColor = Color.Aqua;
            btnWelcome.Location = new Point(0, 284);
            btnWelcome.Name = "btnWelcome";
            btnWelcome.Size = new Size(781, 51);
            btnWelcome.TabIndex = 1;
            btnWelcome.Text = "Launch!";
            btnWelcome.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkSlateGray;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(781, 335);
            Controls.Add(btnWelcome);
            Controls.Add(Choosing);
            Name = "MainForm";
            Text = "DupFin";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Choosing;
        private Button btnWelcome;
    }
}