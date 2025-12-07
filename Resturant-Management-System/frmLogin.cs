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
        public frmLogin()
        {
            InitializeComponent();

            timerFadeIn.Interval = 10; // 20ms interval for smooth animation
            timerFadeIn.Tick += new EventHandler(timerFadeIn_Tick);
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

                // 2. Hide the current (login) form
                this.Hide();

                // 3. Create and show the main form
                frmMain frm = new frmMain();
                frm.Show();
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            timerFadeIn.Start();

            lblTitle.Left = (this.ClientSize.Width - lblTitle.Width) / 2;
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
    }
}