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
        private bool _matchName;
        private bool _checkPaths;
        private bool _isSwitching = false;

        public ProgressForm(string path, HashAlgorithmType algo, bool matchName, bool checkPaths)
        {
            InitializeComponent();
            _path = path;
            _algo = algo;
            _matchName = matchName;
            _checkPaths = checkPaths;
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            progressBar1.Style = ProgressBarStyle.Marquee;
            await ScanAsync();
        }

        private async Task ScanAsync()
        {
            try
            {
                var progress = new Progress<string>(status =>
                {
                    lableStatus.Text = status;
                });

                await Task.Run(async () =>
                {
                    await FileScanner.ScanDirectory(_path, _algo, _matchName, progress);
                });

                _isSwitching = true;

                var resultsForm = new ResultsForm(_checkPaths);
                resultsForm.Show();
                this.Close();
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
            if (!_isSwitching && e.CloseReason == CloseReason.UserClosing)
            {
                Environment.Exit(0);
            }
        }
    }
}