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
    public partial class frmDashboard : Form
    {
        public frmDashboard()
        {
            InitializeComponent();
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ShowLoginFormWithOverlay();
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            // Define how many pixels you want to move them up
            int moveUpBy = 30;

            // Subtract from the 'Top' property to move up
            btnLogin.Top -= moveUpBy;
            btnSignup.Top -= moveUpBy;
        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
         
            DialogResult result = ShowSignupFormAndGetResult();

            if (result == DialogResult.OK)
            {
                // If DialogResult.OK is returned (which happens when btnLogin is clicked on frmSignup)
                ShowLoginFormWithOverlay();
            }

        }

        // Inside Resturant_Management_System.frmDashboard

        // *** NEW METHOD to handle the transition to the Login Form with Overlay ***
        private void ShowLoginFormWithOverlay()
        {
            // 1. Create a transparent background form (The "Blind" effect)
            Form modalBackground = new Form();

            using (frmLogin loginForm = new frmLogin())
            {
                loginForm.Opacity = 0.0;
                // Configure the dark overlay
                modalBackground.StartPosition = FormStartPosition.Manual;
                modalBackground.FormBorderStyle = FormBorderStyle.None;
                modalBackground.Opacity = 0.70d;
                modalBackground.BackColor = Color.Black;
                modalBackground.Size = this.Size;
                modalBackground.Location = this.Location;
                modalBackground.ShowInTaskbar = false;

                // Show the overlay
                modalBackground.Show(this);

                // 2. Link the Login form to the Overlay
                loginForm.Owner = modalBackground;

                // 3. Show the Login form as a Modal Dialog
                loginForm.ShowDialog();

                // 4. Once Login is closed, dispose of the overlay
                modalBackground.Dispose();
            }
        }

        private DialogResult ShowSignupFormAndGetResult()
        {
            Form modalBackground = new Form();
            DialogResult result = DialogResult.Cancel; // Default result

            using (frmSignup signupForm = new frmSignup())
            {
                signupForm.Opacity = 0.0;
                // Configure the dark overlay
                modalBackground.StartPosition = FormStartPosition.Manual;
                modalBackground.FormBorderStyle = FormBorderStyle.None;
                modalBackground.Opacity = 0.70d;
                modalBackground.BackColor = Color.Black;
                modalBackground.Size = this.Size;
                modalBackground.Location = this.Location;
                modalBackground.ShowInTaskbar = false;

                // Show the overlay
                modalBackground.Show(this);

                // Link the Signup form to the Overlay
                signupForm.Owner = modalBackground;

                // Show the Signup form as a Modal Dialog and capture the result
                result = signupForm.ShowDialog();

                // Once Signup is closed, dispose of the overlay
                modalBackground.Dispose();
            }
            return result;
        }
    }
}
