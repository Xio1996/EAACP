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
    public partial class QuickObs : Form
    {
        public QuickObs()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string ObsNote { get; set; }
        public bool Associated { get; set; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ObsNote = txtNote.Text;
            Associated = chkAssociated.Checked;
            this.DialogResult = DialogResult.OK;
        }

        private void QuickObs_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.QOXPos = this.Left;
            Properties.Settings.Default.QOYPos = this.Top;
            Properties.Settings.Default.Save();
        }
    }
}
