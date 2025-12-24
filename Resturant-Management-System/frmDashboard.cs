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
        private Timer timerFadeIn = new Timer();
        public frmDashboard()
        {
            InitializeComponent();

            this.Opacity = 0.0; // Start completely transparent
            timerFadeIn.Interval = 20; // 20ms interval (or 10 as you used in frmLogin)
            timerFadeIn.Tick += new EventHandler(timerFadeIn_Tick);
        }

        public void StartFadeIn()
        {
            // Reset opacity and restart the timer for the fade-in effect
            this.Opacity = 0.0;
            timerFadeIn.Stop();
            timerFadeIn.Start();
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
            DialogResult result = ShowLoginFormAndGetResult();

            while (result == DialogResult.Retry || result == DialogResult.No)
            {
                if (result == DialogResult.Retry) // User clicked "Signup" from Login
                {
                    DialogResult signupResult = ShowSignupFormAndGetResult();

                    if (signupResult == DialogResult.OK)
                    {
                        // Signup was successful or user clicked "Login" button in Signup form
                        result = ShowLoginFormAndGetResult();
                    }
                    else
                    {
                        // User clicked "Exit" or closed the Signup form
                        return; // Exit the method entirely
                    }
                }
                else if (result == DialogResult.No) // User clicked "Forgot Password"
                {
                    DialogResult forgotResult = ShowForgotPasswordFormAndGetResult();

                    if (forgotResult == DialogResult.OK)
                    {
                        result = ShowLoginFormAndGetResult();
                    }
                    else if (forgotResult == DialogResult.Retry)
                    {
                        result = DialogResult.Retry; // Redirect to signup logic in next iteration
                    }
                    else
                    {
                        // User closed the Forgot Password form
                        return;
                    }
                }
            }

            if (result == DialogResult.Retry)
            {
                // Reuse your existing signup logic or trigger the btnSignup_Click event
                btnSignup_Click(sender, e);
            }

            if (result == DialogResult.OK)
            {
                // 2. Successful Login - proceed to main form
                // This is where the application flow continues.

                // Hide the dashboard
                this.Hide();

                // Create and show the main form (frmMain should exist in your project)
                frmMain frm = new frmMain();
                frm.Show();

                // Close the dashboard if you don't need it hidden in the background
                // this.Close();
            }
        }


        private void frmDashboard_Load(object sender, EventArgs e)
        {
            // Define how many pixels you want to move them up
            int moveUpBy = 30;

            // Subtract from the 'Top' property to move up
            btnLogin.Top -= moveUpBy;
            btnSignup.Top -= moveUpBy;

            timerFadeIn.Start();
        }

        private void btnSignup_Click(object sender, EventArgs e)
        {

            DialogResult result = ShowSignupFormAndGetResult();

            // We use a loop to allow the user to navigate between Login, Signup, and Forgot Password
            while (result == DialogResult.OK || result == DialogResult.No || result == DialogResult.Retry)
            {
                if (result == DialogResult.OK)
                {
                    // User clicked "Login" from Signup Form OR finished Signup and we want to show Login
                    DialogResult loginResult = ShowLoginFormAndGetResult();

                    if (loginResult == DialogResult.OK)
                    {
                        // Successful login - Transition to Main Form
                        this.Hide();
                        frmMain frm = new frmMain();
                        frm.Show();
                        break;
                    }

                    // Update result based on what happened in Login (e.g., did they click Signup? Forgot Password?)
                    result = loginResult;
                }
                else if (result == DialogResult.No)
                {
                    // User clicked "Forgot Password" from the Login Form
                    DialogResult forgotResult = ShowForgotPasswordFormAndGetResult();

                    // After closing Forgot Password, usually we go back to Login
                    if (forgotResult == DialogResult.OK)
                    {
                        result = DialogResult.OK; // Set to OK to trigger the Login form block above
                    }
                    else
                    {
                        result = forgotResult; // Could be Retry (Signup) or Cancel
                    }
                }
                else if (result == DialogResult.Retry)
                {
                    // User clicked "Signup" from Login or Forgot Password Form
                    result = ShowSignupFormAndGetResult();
                }
                else
                {
                    // User closed the forms (Cancel)
                    break;
                }
            }
        }

        // Inside Resturant_Management_System.frmDashboard

        // *** NEW METHOD to handle the transition to the Login Form with Overlay ***
        private DialogResult ShowLoginFormAndGetResult()
        {
            // 1. Create a transparent background form (The "Blind" effect)
            Form modalBackground = new Form();
            DialogResult result = DialogResult.Cancel;

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
                result = loginForm.ShowDialog();

                // 4. Once Login is closed, dispose of the overlay
                modalBackground.Dispose();
            }
            return result;
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

        private DialogResult ShowForgotPasswordFormAndGetResult()
        {
            Form modalBackground = new Form();
            DialogResult result = DialogResult.Cancel;

            using (frmForgotPassword forgotForm = new frmForgotPassword())
            {
                // Setup the Overlay (The "Blind" effect)
                modalBackground.StartPosition = FormStartPosition.Manual;
                modalBackground.FormBorderStyle = FormBorderStyle.None;
                modalBackground.Opacity = 0.70d;
                modalBackground.BackColor = Color.Black;
                modalBackground.Size = this.Size;
                modalBackground.Location = this.Location;
                modalBackground.ShowInTaskbar = false;
                modalBackground.Show(this);

                forgotForm.Owner = modalBackground;

                // Start position center of the dashboard
                forgotForm.StartPosition = FormStartPosition.CenterParent;

                result = forgotForm.ShowDialog();
                modalBackground.Dispose();
            }
            return result;
        }

        private void timerFadeIn_Tick(object sender, EventArgs e)
        {
            // Increase the opacity by a small step
            this.Opacity += 0.05;

            // Check if the form is fully visible
            if (this.Opacity >= 1.0)
            {
                // Stop the timer when the animation is complete
                timerFadeIn.Stop();
            }
        }
    }
}
