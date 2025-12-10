using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resturant_Management_System
{
    class MainClass
    {
        public static readonly string con_string = "Data Source=DESKTOP-PMGA4VM;Initial Catalog=RM;Integrated Security=True";
        public static SqlConnection con = new SqlConnection(con_string);

        //Method to check user validation
        public static bool IsValidUser(string username, string password)
        {
            bool isValid = false;
            string qry = @"Select * from users where username = '" + username + "' COLLATE SQL_Latin1_General_CP1_CS_AS and upass = '" + password + "' COLLATE SQL_Latin1_General_CP1_CS_AS";
            SqlCommand cmd = new SqlCommand(qry, con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                isValid = true;
                USER = dt.Rows[0]["uName"].ToString();
            }
            return isValid;
        }

        //Create property for username

        public static string user;

        public static string USER
        {
            get { return user; }
            private set { user = value; }
        }

        public static int CRUD(string qry, params SqlParameter[] parameters)
        {
            int res = 0;
            try
            {
                SqlCommand cmd = new SqlCommand(qry, con);
                // Add parameters to the command if any are provided
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                con.Open();
                res = cmd.ExecuteNonQuery(); // Execute the command
                con.Close();
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it or show a message)
                System.Windows.Forms.MessageBox.Show("Database Error: " + ex.Message);
                res = -1; // Indicate failure
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return res;
        }
    }

}
