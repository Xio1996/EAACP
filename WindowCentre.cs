using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EAACP
{
    internal class WindowHelper
    {
        public static void EnsureWindowVisible(Form form, Form parent = null)
        {
            bool isVisible = false;

            // Check if the window is at least partially visible on any screen
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.IntersectsWith(form.Bounds))
                {
                    isVisible = true;
                    return;
                }
            }

            // If the window is not visible on any screen, center it on the primary screen
            if (!isVisible)
            {
                if (parent != null)
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    return;
                }

                var primary = Screen.PrimaryScreen;
                var primaryWorkingArea = primary.WorkingArea;
                int primaryCenterX = (primaryWorkingArea.Right - form.Width) / 2;
                int primaryCenterY = (primaryWorkingArea.Bottom - form.Height) / 2;
                form.Location = new Point(primaryCenterX, primaryCenterY);
            }
        }
    }
}
