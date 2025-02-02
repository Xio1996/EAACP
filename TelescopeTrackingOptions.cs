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
    public partial class TelescopeTrackingOptions : Form
    {
        private TelescopeChooser telescopeChooser = new TelescopeChooser();

        public TelescopeTrackingOptions()
        {
            InitializeComponent();
        }

        private void btnTelescope_Click(object sender, EventArgs e)
        {
            if (telescopeChooser.Choose())
            {
                txtTTASCOMTelescope.Text = telescopeChooser.TelescopeID;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ASCOMTelescope = txtTTASCOMTelescope.Text;
            Properties.Settings.Default.TTASCOMUpdateRate = cbTTUpdate.SelectedItem.ToString();
            Properties.Settings.Default.TTGraphicSize = txtGraphicSize.Text;
            Properties.Settings.Default.TTGraphicSymbol = cbStGraphic.SelectedItem.ToString();
            Properties.Settings.Default.TTGraphicColour = Color.FromName(cbColour.SelectedItem.ToString());
            
            Properties.Settings.Default.Save();

            this.DialogResult = DialogResult.OK;
        }

        private void TelescopeTrackingOptions_Load(object sender, EventArgs e)
        {
            txtTTASCOMTelescope.Text = Properties.Settings.Default.ASCOMTelescope;
            cbTTUpdate.SelectedItem = Properties.Settings.Default.TTASCOMUpdateRate;
            txtGraphicSize.Text = Properties.Settings.Default.TTGraphicSize.ToString();
            cbStGraphic.SelectedItem = Properties.Settings.Default.TTGraphicSymbol;
            cbColour.SelectedItem = Properties.Settings.Default.TTGraphicColour.Name;
        }
    }
}
