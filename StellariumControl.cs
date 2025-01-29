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
    public partial class StellariumControl : Form
    {
        private Stellarium Stellarium = new Stellarium();

        public StellariumControl()
        {
            InitializeComponent();
        }

        private void StellariumControl_Load(object sender, EventArgs e)
        {
            bool bMP;
            bool.TryParse(Stellarium.GetStelProperty("actionShow_Planets_ShowMinorBodyMarkers"), out bMP);
        }
    }
}
