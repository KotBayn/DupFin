using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DupFinUI.forms
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();
        }
        private void btnWelcome_Click(object sender, EventArgs e)
        {
            // Making a new form instance
            var mainForm = new MainForm(this);
            mainForm.Show();

            // hiding the welcome form
            this.Hide();
        }
        private void WelcomeForm_Load(object sender, EventArgs e)
        {
            // ----
        }
    }
}
