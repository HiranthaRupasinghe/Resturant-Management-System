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
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            guna2MessageDialog3.Text = "Are you sure you want to exit the application?";

            DialogResult result = guna2MessageDialog3.Show();

            if (result == DialogResult.Yes)
            {
                Application.Exit(); // Close the application only if Yes is selected
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Let create database and user table

            if (MainClass.IsValidUser(txtUsername.Text, txtPassword.Text) == false)
            {
                guna2MessageDialog1.Show("Invalid username or password");
                return;
            }
            else
            {
                // 1. Display the successful login message using guna2MessageDialog3
                guna2MessageDialog2.Show("Login Successful!");

                // 2. Hide the current (login) form
                this.Hide();

                // 3. Create and show the main form
                frmMain frm = new frmMain();
                frm.Show();
            }
        }
    }
}
