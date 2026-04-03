using System.Windows.Forms;
using System.Xml.Linq;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace DupFinUI.forms
{
    partial class WelcomeForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
            Welcoming = new Label();
            btnWelcome = new Button();
            welcomeText = new Label();
            SuspendLayout();
            // 
            // Welcoming
            // 
            Welcoming.AutoSize = true;
            Welcoming.BackColor = Color.Transparent;
            Welcoming.FlatStyle = FlatStyle.Popup;
            Welcoming.Font = new System.Drawing.Font("Cooper Black", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Welcoming.ForeColor = Color.LightSeaGreen;
            Welcoming.Location = new Point(196, 9);
            Welcoming.Name = "Welcoming";
            Welcoming.Size = new Size(388, 41);
            Welcoming.TabIndex = 0;
            Welcoming.Text = "Welcome to DupFin!\r\n";
            Welcoming.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnWelcome
            // 
            btnWelcome.AutoSize = true;
            btnWelcome.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnWelcome.BackColor = Color.LightSeaGreen;
            btnWelcome.Dock = DockStyle.Bottom;
            btnWelcome.FlatStyle = FlatStyle.Popup;
            btnWelcome.Font = new System.Drawing.Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnWelcome.ForeColor = Color.Aqua;
            btnWelcome.Location = new Point(0, 297);
            btnWelcome.Name = "btnWelcome";
            btnWelcome.Size = new Size(781, 38);
            btnWelcome.TabIndex = 1;
            btnWelcome.Text = "Launch!";
            btnWelcome.UseVisualStyleBackColor = false;
            btnWelcome.Click += btnWelcome_Click;
            // 
            // welcomeText
            // 
            welcomeText.Font = new System.Drawing.Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            welcomeText.ForeColor = Color.LightSeaGreen;
            welcomeText.Location = new Point(178, 96);
            welcomeText.Name = "welcomeText";
            welcomeText.Size = new Size(424, 99);
            welcomeText.TabIndex = 2;
            welcomeText.Text = "This tool will allow you to effectively search for duplicate files in your file system. (For now only Windows)";
            welcomeText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // WelcomeForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkSlateGray;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(781, 335);
            Controls.Add(welcomeText);
            Controls.Add(btnWelcome);
            Controls.Add(Welcoming);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Name = "WelcomeForm";
            Text = "DupFin";
            Load += WelcomeForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Welcoming;
            private Button btnWelcome;
            private Label welcomeText;
    }
}