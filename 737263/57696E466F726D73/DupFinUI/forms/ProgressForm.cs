using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using DupFin.Services;
using DupFin.Enums;

namespace DupFinUI.forms
{
    public partial class ProgressForm : Form
    {
        private string _path;

        public ProgressForm(string path)
        {
            InitializeComponent();
            _path = path;
        }

        private async void ProgressForm_Load(object sender, EventArgs e)
        {
            // 
            await ScanAsync();
        }

        private async Task ScanAsync()
        {
            try
            {
                // Тут будет твой FileScanner
                await FileScanner.ScanDirectory(_path, HashAlgorithmType);

                // После завершения — переходим к результатам
                var resultsForm = new ResultsForm();
                resultsForm.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
                this.Close();
            }
        }
    }
}