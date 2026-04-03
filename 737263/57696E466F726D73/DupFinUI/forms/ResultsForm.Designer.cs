using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace DupFinUI.forms
{
    partial class ResultsForm
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
            Results = new Label();
            btnBack = new Button();
            treeResults = new TreeView();
            labelCount = new Label();
            btnSave = new Button();
            btnDelete = new Button();
            SuspendLayout();
            // 
            // Results
            // 
            Results.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Results.AutoSize = true;
            Results.BackColor = Color.Transparent;
            Results.FlatStyle = FlatStyle.Popup;
            Results.Font = new System.Drawing.Font("Cooper Black", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Results.ForeColor = Color.LightSeaGreen;
            Results.Location = new Point(258, 9);
            Results.Name = "Results";
            Results.Size = new Size(265, 36);
            Results.TabIndex = 0;
            Results.Text = "Results of scan:\r\n";
            Results.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnBack
            // 
            btnBack.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnBack.AutoSize = true;
            btnBack.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnBack.BackColor = Color.LightSeaGreen;
            btnBack.FlatStyle = FlatStyle.Popup;
            btnBack.Font = new System.Drawing.Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBack.ForeColor = Color.Aqua;
            btnBack.Location = new Point(12, 285);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(162, 38);
            btnBack.TabIndex = 4;
            btnBack.Text = "Back to menu";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // treeResults
            // 
            treeResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            treeResults.BackColor = SystemColors.Control;
            treeResults.CheckBoxes = true;
            treeResults.Font = new System.Drawing.Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            treeResults.Location = new Point(29, 63);
            treeResults.Name = "treeResults";
            treeResults.Size = new Size(715, 191);
            treeResults.TabIndex = 5;
            treeResults.AfterSelect += treeResults_AfterSelect;
            // 
            // labelCount
            // 
            labelCount.AutoSize = true;
            labelCount.Font = new System.Drawing.Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelCount.ForeColor = Color.Aqua;
            labelCount.Location = new Point(672, 32);
            labelCount.Name = "labelCount";
            labelCount.Size = new Size(0, 28);
            labelCount.TabIndex = 6;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.AutoSize = true;
            btnSave.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnSave.BackColor = Color.LightSeaGreen;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new System.Drawing.Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.Aqua;
            btnSave.Location = new Point(629, 285);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(140, 38);
            btnSave.TabIndex = 7;
            btnSave.Text = "Save results";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnDelete
            // 
            btnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDelete.AutoSize = true;
            btnDelete.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnDelete.BackColor = Color.LightSeaGreen;
            btnDelete.FlatStyle = FlatStyle.Popup;
            btnDelete.Font = new System.Drawing.Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDelete.ForeColor = Color.Aqua;
            btnDelete.Location = new Point(347, 285);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(87, 38);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // ResultsForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.DarkSlateGray;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(781, 335);
            Controls.Add(btnDelete);
            Controls.Add(btnSave);
            Controls.Add(labelCount);
            Controls.Add(treeResults);
            Controls.Add(btnBack);
            Controls.Add(Results);
            Font = new System.Drawing.Font("Perpetua", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "ResultsForm";
            Text = "DupFin";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Results;
            private Button btnBack;
        private TreeView treeResults;
        private Label labelCount;
        private Button btnSave;
        private Button btnDelete;
    }
    }