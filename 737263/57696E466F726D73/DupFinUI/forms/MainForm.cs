using DupFin.Enums;

namespace DupFinUI.forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            comboHashTypeBox.DataSource = Enum.GetValues(typeof(HashAlgorithmType));
            comboHashTypeBox.SelectedItem = HashAlgorithmType.SHA256;
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            // Checking the path
            if (string.IsNullOrWhiteSpace(textBoxPath.Text))
            {
                MessageBox.Show("Choose folder!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Binding Enum to DataSource
            HashAlgorithmType selectedAlgo = HashAlgorithmType.XxHash64;

            if (comboHashTypeBox.SelectedItem != null)
            {
                selectedAlgo = (HashAlgorithmType)comboHashTypeBox.SelectedItem;
            }

            bool matchName = chkMatchName.Checked;
            bool checkPaths = chkCheckPaths.Checked;

            // Sending both parameters to next form
            var progressForm = new ProgressForm(textBoxPath.Text, selectedAlgo, matchName, checkPaths);
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

            // Nuke
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Environment.Exit(0);
            }
        }
    }
}