using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resturant_Management_System
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool isLoggedIn = Properties.Settings.Default.IsLoggedIn;
            DateTime lastLogin = Properties.Settings.Default.LastLoginTime;

            // Calculate the difference in minutes
            double minutesSinceLogin = (DateTime.Now - lastLogin).TotalMinutes;

            // Check if logged in AND if it has been less than 5 minutes
            if (isLoggedIn && minutesSinceLogin < 5)
            {
                Application.Run(new frmMain());
            }
            else
            {
                // Force logout if time expired or never logged in
                Properties.Settings.Default.IsLoggedIn = false;
                Properties.Settings.Default.Save();
                Application.Run(new frmDashboard());
            }
        }
    }
}
