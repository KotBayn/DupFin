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
        private HashAlgorithmType _algo;

        public ProgressForm(string path, HashAlgorithmType algo)
        {
            InitializeComponent();
            _path = path;
            _algo = algo;
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e); // Обязательная системная штука

            // Жестко задаем правильный стиль прямо из кода, чтобы ты в свойствах ничего не перепутал
            progressBar1.Style = ProgressBarStyle.Marquee;

            // Запускаем сканер
            await ScanAsync();
        }

        private async Task ScanAsync()
        {
            try
            {
                // Create a progress reporter that will update the label safely
                var progress = new Progress<string>(status =>
                {
                    lableStatus.Text = status;
                });
                // Pass the progress reporter to our scanner
                await Task.Run(async () =>
                {
                    await FileScanner.ScanDirectory(_path, _algo, progress);
                });

                // Transition to ResultsForm when finished
                var resultsForm = new ResultsForm();
                resultsForm.Show();
                this.Close(); // Закрываем окно прогресса
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Scan Error: {ex.Message}", "Error");
                this.Close();
            }
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