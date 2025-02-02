using ASCOM.DriverAccess;
using ASCOM.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAACP
{
    internal class TelescopeChooser
    {
        public string TelescopeID = "";

        public bool Choose()
        {
            bool result = false;

            Util U = new Util();
            TelescopeID = Telescope.Choose("");
            if (TelescopeID != "")
            {
                result = true;
            }

            return result;
        }
    }
}
