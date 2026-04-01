using System;
using System.Windows.Forms;

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
            // Проверяем, выбрана ли папка
            if (string.IsNullOrWhiteSpace(txtPath.Text))
            {
                MessageBox.Show("Выберите папку!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Передаём путь в ProgressForm
            var progressForm = new ProgressForm(txtPath.Text);
            progressForm.Show();
            this.Hide();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    txtPath.Text = dialog.SelectedPath;
            }
        }
    }
}