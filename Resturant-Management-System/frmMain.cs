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

        private frmLogin loginForm = null;
        public frmMain()
        {
            InitializeComponent();

            this.Opacity = 0.0; // Start completely transparent
            timerFadeIn.Interval = 20; // 20ms interval
            timerFadeIn.Tick += new EventHandler(timerFadeIn_Tick);

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

        private void timerFadeOut_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.05;

            // Check if the form is completely transparent (or less, due to floating point arithmetic)
            if (this.Opacity <= 0.0)
            {
                // 1. Stop the timer
                timerFadeOut.Stop();

                // 2. Ensure the opacity is exactly 0.0 before hiding
                this.Opacity = 0.0;

                // 3. Hide the current form (now invisible)
                this.Hide();

                // 4. Show the stored login form
                if (loginForm != null)
                {
                    loginForm.Show();
                }
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
                loginForm = new frmLogin();
                // *** CHANGE: Start the fade-out animation instead of hiding the form immediately ***
                timerFadeOut.Start();

                Form modalBackground = new Form();

                using (frmLogin loginForm = new frmLogin())
                {
                    // Configure the dark overlay
                    modalBackground.StartPosition = FormStartPosition.Manual;
                    modalBackground.FormBorderStyle = FormBorderStyle.None;
                    modalBackground.Opacity = 0.70d; // 70% transparency
                    modalBackground.BackColor = Color.Black;
                    modalBackground.Size = this.Size;
                    modalBackground.Location = this.Location;
                    modalBackground.ShowInTaskbar = false;

                    // Show the overlay over the main form
                    modalBackground.Show(this);

                    // Link the Login form to the Overlay
                    loginForm.Owner = modalBackground;

                    // Show the Login form as a Modal Dialog
                    loginForm.ShowDialog();

                    // Once the Login form is closed (either via successful login or Exit)
                    // 4. Dispose of the overlay
                    modalBackground.Dispose();
                }

                // 5. Hide the current Main form after the logout/login process is finished
                this.Hide();
            }
        }
    }
}
