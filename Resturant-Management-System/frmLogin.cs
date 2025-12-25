using Guna.UI2.WinForms;
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
    public partial class frmLogin : Form
    {
        private Timer timerFadeIn = new Timer();

        private Timer timerFadeOut = new Timer();
        private bool isClosing = false;
        public frmLogin()
        {
            InitializeComponent();

            this.Opacity = 0;
            timerFadeIn.Interval = 10; // 20ms interval for smooth animation
            timerFadeIn.Tick += new EventHandler(timerFadeIn_Tick);

            timerFadeOut.Interval = 10;
            timerFadeOut.Tick += new EventHandler(timerFadeOut_Tick);
        }

        private void timerFadeOut_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.05;

            if (this.Opacity <= 0.0)
            {
                timerFadeOut.Stop();
                // Finally close the form after the animation is complete
                isClosing = true;
                this.Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Check if the form is already in the process of closing after the animation
            if (!isClosing && e.CloseReason == CloseReason.UserClosing)
            {
                // Cancel the default close operation
                e.Cancel = true;

                // Start the fade-out animation
                timerFadeOut.Start();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = guna2MessageDialog3.Show("Are you sure you want to exit the application?", "Confirm Exit");

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Let create database and user table

            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                guna2MessageDialog1.Show("Please fill in all required fields.", "Login Fail!");
                return; // Exit the method so it doesn't try to validate the user
            }

            if (MainClass.IsValidUser(txtUsername.Text, txtPassword.Text) == false)
            {
                guna2MessageDialog1.Show("Invalid username or password", "Login Fail!");
                return;
            }
            else
            {
                //timerFadeIn.Stop();
                // 1. Display the successful login message using guna2MessageDialog3
                guna2MessageDialog2.Show("Login Successfully for Our System.", "Login Successful!");
                Properties.Settings.Default.IsLoggedIn = true;
                Properties.Settings.Default.Save();

                this.DialogResult = DialogResult.OK;

                // Start Fade-Out animation before closing
                timerFadeOut.Start();

                /*frmDashboard dashboard = Application.OpenForms.OfType<frmDashboard>().FirstOrDefault();
                if (dashboard != null)
                {
                    dashboard.Hide(); // Or dashboard.Close();
                }

                // 3. Create and show the main form
                frmMain frm = new frmMain();
                frm.Show();

                // 2. Hide the current (login) form
                this.Close();*/

            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            timerFadeIn.Start();

            lblTitle.Left = (this.ClientSize.Width - lblTitle.Width) / 2;
            guna2PictureBox1.Left = (this.ClientSize.Width - guna2PictureBox1.Width) / 2;
            btnForgotPassword.Left = (this.ClientSize.Width - btnForgotPassword.Width) / 2;

            txtPassword.UseSystemPasswordChar = true;
            txtPassword.IconRight = Properties.Resources.Hide;
        }

        private void timerFadeIn_Tick(object sender, EventArgs e)
        {
            // Increase the opacity by a small step
            // 0.05 is a good balance for speed and smoothness (20 steps total)
            this.Opacity += 0.05;

            // Check if the form is fully visible
            if (this.Opacity >= 1.0)
            {
                // Stop the timer and the animation is complete
                timerFadeIn.Stop();
            }
        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            //this.Close();
            timerFadeOut.Start();
        }

        private void btnForgotPassword_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            //this.Close();
            timerFadeOut.Start();
        }

        private void txtPassword_IconRightClick(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;

            // Change the icon based on the new state
            if (txtPassword.UseSystemPasswordChar)
            {
                // Password is HIDDEN: Show the eye with the crosshair/slash
                txtPassword.IconRight = Properties.Resources.Hide;
            }
            else
            {
                // Password is VISIBLE: Show the clean eye without the crosshair
                txtPassword.IconRight = Properties.Resources.Unhide;
            }
        }
    }
}