using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resturant_Management_System
{
    public partial class frmMain : Form
    {
        private Timer timerFadeIn = new Timer();
        private Timer timerFadeOut = new Timer();

        public frmMain()
        {
            InitializeComponent();

            this.Opacity = 0.0; // Start completely transparent
            timerFadeIn.Interval = 20; // 20ms interval
            timerFadeIn.Tick += new EventHandler(timerFadeIn_Tick);

            // *** NEW: Initialize the Fade-Out Timer in the constructor ***
            timerFadeOut.Interval = 20; // Same interval for symmetry
            timerFadeOut.Tick += new EventHandler(timerFadeOut_Tick);
        }

        //Methord to add Controls in Main Form
        public void AddContorls(Form f)
        {
            CenterPanel.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            CenterPanel.Controls.Add(f);
            f.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMaximized_Click(object sender, EventArgs e)
        {
            // Toggles the WindowState between Maximized and Normal
            if (this.WindowState == FormWindowState.Normal)
            {
                // If it's Normal, maximize it
                this.WindowState = FormWindowState.Maximized;
            }
            else // It must be Maximized (or Minimized, but usually you only toggle Normal/Maximized)
            {
                // Restore to normal size
                this.WindowState = FormWindowState.Normal;

                // Center the form on the screen
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(
                    (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                    (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2
                );
            }
        }

        private void btnMinimized_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblUser.Text = MainClass.USER;

            timerFadeIn.Start();
        }

        private void timerFadeIn_Tick(object sender, EventArgs e)
        {
            // Increase the opacity by a small step (0.05 will take 20 steps, or 400ms total)
            this.Opacity += 0.05;

            // Check if the form is fully visible
            if (this.Opacity >= 1.0)
            {
                // Stop the timer when the animation is complete
                timerFadeIn.Stop();
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            AddContorls(new frmHome());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = guna2MessageDialog1.Show("Are you sure you want to log out?", "Confirm Logout");

            if (result == DialogResult.Yes)
            {
                if (result == DialogResult.Yes)
                {
                    // *** CHANGE: Start the fade-out animation instead of hiding the form immediately ***
                    timerFadeOut.Start();
                }
            }
        }

        private void timerFadeOut_Tick(object sender, EventArgs e)
        {
            // Decrease the opacity by a small step
            this.Opacity -= 0.05;

            // Check if the form is completely transparent
            if (this.Opacity <= 0.0)
            {
                // 1. Stop the timer
                timerFadeOut.Stop();

                // *** MODIFIED: Use LINQ to find the existing instance of frmDashboard ***
                // 2. Find the existing Dashboard form
                frmDashboard DashboardForm = Application.OpenForms.OfType<frmDashboard>().FirstOrDefault();

                // 3. If the Dashboard form does not exist (e.g., if it was closed instead of hidden initially), create it.
                if (DashboardForm == null)
                {
                    DashboardForm = new frmDashboard();
                }

                // 4. Hide the current form (now that it's invisible)
                this.Hide();

                // 5. Show the Dashboard form
                DashboardForm.Show();
            }
        }

    }
}