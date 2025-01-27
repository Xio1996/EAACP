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
            txtAuthentication.Text = EncryptionHelper.Decrypt(Properties.Settings.Default.Auth);
            txtAPIP.Text = Properties.Settings.Default.APIP;
            txtAPPort.Text = Properties.Settings.Default.APPort;
            txtStelIP.Text = Properties.Settings.Default.StelIP;
            txtStelPort.Text = Properties.Settings.Default.StelPort;
            txtStelPassword.Text = EncryptionHelper.Decrypt (Properties.Settings.Default.StelPassword);
            txtStellariumScriptDirectory.Text = Properties.Settings.Default.StScriptFolder;
            txtLat.Text = Properties.Settings.Default.SiteLat.ToString();
            txtLng.Text = Properties.Settings.Default.SiteLng.ToString();
            txtElev.Text = Properties.Settings.Default.SiteElev.ToString();

            btnCancel.Select();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Auth = EncryptionHelper.Encrypt(txtAuthentication.Text.Trim());
            Properties.Settings.Default.APIP = txtAPIP.Text.Trim();
            Properties.Settings.Default.APPort = txtAPPort.Text;

            Properties.Settings.Default.StelIP = txtStelIP.Text.Trim();
            Properties.Settings.Default.StelPort = txtStelPort.Text.Trim();
            Properties.Settings.Default.StScriptFolder = txtStellariumScriptDirectory.Text.Trim();
            Properties.Settings.Default.StelPassword = EncryptionHelper.Encrypt(txtStelPassword.Text.Trim());

            Properties.Settings.Default.SiteLat = Double.Parse(txtLat.Text.Trim());
            Properties.Settings.Default.SiteLng = Double.Parse(txtLat.Text.Trim());
            Properties.Settings.Default.SiteElev = Double.Parse(txtElev.Text.Trim());

            txtLng.Text = Properties.Settings.Default.SiteLng.ToString();
            txtElev.Text = Properties.Settings.Default.SiteElev.ToString();

            Properties.Settings.Default.Save();
            this.Close();
        }

        private void btnScriptFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtStellariumScriptDirectory.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
