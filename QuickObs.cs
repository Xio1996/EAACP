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

            if (Properties.Settings.Default.QOYPos == -1)
            {
                this.CenterToParent();
            }
            else
            {
                if (Screen.PrimaryScreen.Bounds.Width > Properties.Settings.Default.QOXPos + this.Width)
                {
                    this.Left = Properties.Settings.Default.QOXPos;
                    if (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height > Properties.Settings.Default.QOYPos + this.Height)
                    {
                        this.Top = Properties.Settings.Default.QOYPos;
                    }
                    else
                    {
                        this.CenterToParent();
                    }
                }
                else
                {
                    this.CenterToParent();
                }
            }
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
