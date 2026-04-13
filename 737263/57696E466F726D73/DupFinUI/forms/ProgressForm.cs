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
        private bool _isSwitching = false;

        public ProgressForm(string path, HashAlgorithmType algo)
        {
            InitializeComponent();
            _path = path;
            _algo = algo;
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e); // Important, otherwise the form might not render before the scan starts

            progressBar1.Style = ProgressBarStyle.Marquee;

            // Start the scanning
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
                
                _isSwitching = true;
                // Transition to ResultsForm when finished
                var resultsForm = new ResultsForm();
                resultsForm.Show();
                this.Close(); // 
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

            // Nuke
            if (!_isSwitching && e.CloseReason == CloseReason.UserClosing)
            {
                Environment.Exit(0);
            }
        }
    }
}