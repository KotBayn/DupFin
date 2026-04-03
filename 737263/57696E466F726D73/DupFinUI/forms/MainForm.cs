using System;
using System.Windows.Forms;
using DupFin.Enums;

namespace DupFinUI.forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            // Checking the path
            if (string.IsNullOrWhiteSpace(textBoxPath.Text))
            {
                MessageBox.Show("Choose folder!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Reading type of hash from ComboBox. If user don`t choose anysing
            // use SHA256  
            HashAlgorithmType selectedAlgo = HashAlgorithmType.SHA256;

            if (comboHashTypeBox.SelectedItem != null)
            {
                // Making text back to Enum
                selectedAlgo = (HashAlgorithmType)Enum.Parse(typeof(HashAlgorithmType), comboHashTypeBox.SelectedItem.ToString()!);
            }

            // Sending both parametrs to next form
            var progressForm = new ProgressForm(textBoxPath.Text, selectedAlgo);
            progressForm.Show();
            this.Hide();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    textBoxPath.Text = dialog.SelectedPath;
            }
        }

        private void textBoxPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnBrowse_Click_1(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    textBoxPath.Text = dialog.SelectedPath;
            }
        }

        private void comboHashTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Если форму закрывают крестиком или через Alt+F4 - убиваем процесс к чертовой матери
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Environment.Exit(0);
            }
        }
    }
}