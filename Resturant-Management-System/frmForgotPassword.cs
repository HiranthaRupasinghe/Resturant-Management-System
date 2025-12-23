using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resturant_Management_System
{
    public partial class frmForgotPassword : Form
    {

        private Timer timerFadeIn = new Timer();
        private Timer timerFadeOut = new Timer();
        private bool isClosing = false;
        public frmForgotPassword()
        {
            InitializeComponent();

            this.Opacity = 0;
            timerFadeIn.Interval = 10; // 20ms interval for smooth animation
            timerFadeIn.Tick += new EventHandler(timerFadeIn_Tick);

            timerFadeOut.Interval = 10;
            timerFadeOut.Tick += new EventHandler(timerFadeOut_Tick);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                guna2MessageDialog1.Show("Please enter a username to search.", "Input Required");
                return;
            }

            DataTable dt = MainClass.GetUserDetails(txtUsername.Text);

            if (dt.Rows.Count > 0 && dt.Rows[0]["username"].ToString().Equals(txtUsername.Text, StringComparison.Ordinal))
            {
                // User found - display the question
                txtSecurityQuestion.Text = dt.Rows[0]["SecurityQuestion"].ToString();
                // Store the correct answer in a hidden variable or tag for comparison later
                txtAnswer.Tag = dt.Rows[0]["Answer"].ToString();
            }
            else
            {
                guna2MessageDialog1.Show("Username not found or not matching!", "Search Warning");
                txtSecurityQuestion.Clear();
                txtAnswer.Tag = null;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtAnswer.Text) ||
                string.IsNullOrWhiteSpace(txtNewPass.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPass.Text))
            {
                guna2MessageDialog2.Show("All fields are required!", "Error");
                return;
            }

            // 2. Check security answer (Case sensitive comparison)
            string correctAnswer = txtAnswer.Tag?.ToString();
            if (!txtAnswer.Text.Equals(correctAnswer, StringComparison.Ordinal))
            {
                guna2MessageDialog1.Show("The answer to the security question is incorrect or not matching!.", "Warning");
                return;
            }

            // 3. Check if passwords match
            if (txtNewPass.Text != txtConfirmPass.Text)
            {
                guna2MessageDialog2.Show("Passwords do not match!", "Error");
                return;
            }

            // 4. Update the password in database
            string qry = "UPDATE users SET upass = @pass WHERE username = @user";
            SqlParameter[] ps = {
                new SqlParameter("@pass", txtNewPass.Text),
                new SqlParameter("@user", txtUsername.Text)
            };

            if (MainClass.CRUD(qry, ps) > 0)
            {
                guna2MessageDialog4.Show("Password updated successfully!", "Success");
                this.Close(); // Return to login
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

        private void frmForgotPassword_Load(object sender, EventArgs e)
        {
            timerFadeIn.Start();
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

        private void timerFadeOut_Tick(object sender, EventArgs e)
        {
            // Decrease the opacity by a small step
            this.Opacity -= 0.05;

            // Check if the form is fully invisible
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            timerFadeOut.Start();
        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            //this.Close();
            timerFadeOut.Start();
        }
    }
}
