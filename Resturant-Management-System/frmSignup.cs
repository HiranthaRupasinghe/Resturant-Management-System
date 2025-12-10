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
using System.Xml.Linq;

namespace Resturant_Management_System
{
    public partial class frmSignup : Form
    {
        private Timer timerFadeIn = new Timer();
        public frmSignup()
        {
            InitializeComponent();

            this.Opacity = 0;
            timerFadeIn.Interval = 10; // 20ms interval for smooth animation
            timerFadeIn.Tick += new EventHandler(timerFadeIn_Tick);
        }

        private void frmSignup_Load(object sender, EventArgs e)
        {
            timerFadeIn.Start();
            lblTitle.Left = (this.ClientSize.Width - lblTitle.Width) / 2;
            btnExit.Left = (this.ClientSize.Width - btnExit.Width) / 2;
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

        private void btnExit_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            DialogResult result = guna2MessageDialog1.Show("Are you sure you want to exit the application?", "Confirm Exit");

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // 1. Create an instance of the Login form
            frmLogin loginForm = new frmLogin();

            // 2. Show the Login form
            // Since frmLogin also has a fade-in timer, simply showing it will trigger its animation.
            loginForm.Show();

            // 3. Close the current Signup form
            this.Close();
        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
            // Assuming your controls are:
            // txtUsername, txtPassword, txtConfirmPassword, txtName, txtPhone, 
            // cmbSecurityQuestion (for the ComboBox), txtAnswer (for the answer)

            // 1. Basic validation: Check for required fields
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtAnswer.Text) ||
                cmdSecurityQuestion.SelectedIndex < 0) // Check if an item is selected in the ComboBox
            {
                guna2MessageDialog2.Show("Please fill in all required fields.", "Missing Information");
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                // Trim any leading/trailing spaces for accurate length check
                string phoneNumber = txtPhone.Text.Trim();

                // Check if the trimmed length is exactly 10 digits
                if (phoneNumber.Length != 10 || !phoneNumber.All(char.IsDigit))
                {
                    guna2MessageDialog4.Show("The phone number must be exactly 10 digits.", "Invalid Phone Number");
                    txtPhone.Focus(); // Set focus back to the phone field
                    return;
                }
            }

            // 2. Case-Sensitive Password Match Check
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                guna2MessageDialog2.Show("Password Not matching", "Password Mismatch");
                txtConfirmPassword.Clear();
                txtConfirmPassword.Focus();
                return;
            }

            // 3. Get the selected security question text from the ComboBox
            string selectedQuestion = cmdSecurityQuestion.SelectedItem.ToString();

            // 4. Prepare the SQL Query for Insertion
            string qry = @"INSERT INTO users (username, upass, uName, uphone, SecurityQuestion, Answer) 
                   VALUES (@username, @upass, @uName, @uphone, @SecurityQuestion, @Answer)";

            // 5. Create SQL Parameters
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@username", txtUsername.Text),
                new SqlParameter("@upass", txtPassword.Text),
                new SqlParameter("@uName", txtName.Text),
                // Handle optional phone number if txtPhone can be empty
                new SqlParameter("@uphone", string.IsNullOrWhiteSpace(txtPhone.Text) ? (object)DBNull.Value : txtPhone.Text), 
        
                // Pass the selected text from the ComboBox
                new SqlParameter("@SecurityQuestion", selectedQuestion),
                new SqlParameter("@Answer", txtAnswer.Text)
            };

            // 6. Execute the CRUD method
            int result = MainClass.CRUD(qry, parameters);

            // 7. Check the result and show the success message
            if (result > 0)
            {
                guna2MessageDialog3.Show("Signed up successfully! You can now log in.", "Success Information");

                // Clear the form fields upon successful signup
                txtUsername.Clear();
                txtPassword.Clear();
                txtConfirmPassword.Clear();
                txtName.Clear();
                txtPhone.Clear();
                cmdSecurityQuestion.SelectedIndex = -1; // Reset ComboBox selection
                txtAnswer.Clear();
            }
            else if (result == -1)
            {
                guna2MessageDialog4.Show("Signup failed. A database error occurred.", "Error");
            }
        }
    }
}
