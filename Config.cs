using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EAACP
{
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Config_Load(object sender, EventArgs e)
        {
            txtAuthentication.Text = Properties.Settings.Default.Auth;
            txtAPIP.Text = Properties.Settings.Default.APIP;
            txtAPPort.Text = Properties.Settings.Default.APPort;
            txtStelIP.Text = Properties.Settings.Default.StelIP;
            txtStelPort.Text = Properties.Settings.Default.StelPort;
            btnCancel.Select();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Auth = txtAuthentication.Text.Trim();
            Properties.Settings.Default.APIP = txtAPIP.Text.Trim();
            Properties.Settings.Default.APPort = txtAPPort.Text;

            Properties.Settings.Default.StelIP = txtStelIP.Text.Trim();
            Properties.Settings.Default.StelPort = txtStelPort.Text.Trim();

            Properties.Settings.Default.Save();
            this.Close();
        }

    }
}
