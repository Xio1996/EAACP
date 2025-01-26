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
    public partial class ObjectTextOptions : Form
    {
        public ObjectTextOptions()
        {
            InitializeComponent();
        }

        private void btnFilePath_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = "c:\\";
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the path of specified file
                    string filePath = saveFileDialog.FileName;
                    txtFilePath.Text = filePath;
                }
            }
        }

        private void btnClearManualText_Click(object sender, EventArgs e)
        {
            txtManualEntry.Text = "";
        }

        private void ObjectTextOptions_Load(object sender, EventArgs e)
        {
            chkID.Checked = Properties.Settings.Default.objTxtID;
            chkName.Checked = Properties.Settings.Default.objTxtName;
            chkConstellation.Checked = Properties.Settings.Default.objTxtConstellation;
            chkType.Checked = Properties.Settings.Default.objTxtType;
            chkMagnitude.Checked = Properties.Settings.Default.objTxtMagnitude;
            txtManualEntry.Text = Properties.Settings.Default.objTxtManualText;
            txtFilePath.Text = Properties.Settings.Default.objTxtFilePath;
            txtMaxChars.Text = Properties.Settings.Default.objTxtMaxChars;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.objTxtID = chkID.Checked;
            Properties.Settings.Default.objTxtName = chkName.Checked;
            Properties.Settings.Default.objTxtType = chkType.Checked;
            Properties.Settings.Default.objTxtMagnitude = chkMagnitude.Checked;
            Properties.Settings.Default.objTxtManualText = txtManualEntry.Text;
            Properties.Settings.Default.objTxtFilePath = txtFilePath.Text;
            Properties.Settings.Default.objTxtMaxChars = txtMaxChars.Text;
            Properties.Settings.Default.objTxtConstellation = chkConstellation.Checked;
        }

        private void txtMaxChars_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only digits and control characters (e.g., backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
