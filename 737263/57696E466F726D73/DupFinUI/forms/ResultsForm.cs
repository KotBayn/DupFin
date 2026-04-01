using System;
using System.Windows.Forms;
using DupFin.Services;

namespace DupFinUI.forms
{
    public partial class ResultsForm : Form
    {
        public ResultsForm()
        {
            InitializeComponent();
            LoadResults();
        }

        private void LoadResults()
        {
            int duplicates = 0;
            foreach (var group in FileScanner.FoundFiles.Values)
            {
                if (group.Count > 1)
                {
                    duplicates += group.Count - 1;
                    gridResults.Rows.Add(group[0], group[i + 1], "Hash");
                }
            }
            lblCount.Text = $"Всего дубликатов: {duplicates}";
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            // Логика сохранения
            MessageBox.Show("Сохранение...");
        }

        private void btnToMain_Click(object sender, EventArgs e)
        {
            // Возврат на главную (если нужно)
            var mainForm = new MainForm();
            mainForm.Show();
            this.Close();
        }
    }
}