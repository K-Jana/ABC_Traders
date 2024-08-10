using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ABC_Traders
{
    public partial class RegistrationForm : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=MALIFICENT;Initial Catalog=ABCCarTraders;Integrated Security=True;TrustServerCertificate=True");


        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void RegistrationForm_Load(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBoxFirstName.Text != "" && txtBoxlastName.Text != "" && txtBoxNIC.Text != "" && txtBoxEmail.Text != "" && txtBoxAddress.Text != "" && txtBoxUsername.Text != "" && txtBoxPassword.Text != "")
                {
                    int v = check(txtBoxEmail.Text);
                    if (v == 0)
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("INSERT INTO [user] VALUES(@First_name, @Last_name, @NIC, @email, @address, @username, @password, @user_type)", conn);

                        cmd.Parameters.AddWithValue("@First_name", txtBoxFirstName.Text);
                        cmd.Parameters.AddWithValue("@Last_name", txtBoxlastName.Text);
                        cmd.Parameters.AddWithValue("@NIC", txtBoxNIC.Text);
                        cmd.Parameters.AddWithValue("@email", txtBoxEmail.Text);
                        cmd.Parameters.AddWithValue("@address", txtBoxAddress.Text);
                        cmd.Parameters.AddWithValue("@username", txtBoxUsername.Text);
                        cmd.Parameters.AddWithValue("@password", txtBoxPassword.Text);
                        cmd.Parameters.AddWithValue("@user_type", 1);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Registered Successfully");

                        txtBoxFirstName.Clear();
                        txtBoxlastName.Clear();
                        txtBoxNIC.Clear();
                        txtBoxEmail.Clear();
                        txtBoxAddress.Clear();
                        txtBoxUsername.Clear();
                        txtBoxPassword.Clear();

                        conn.Close();

                    }
                    else
                    {
                        MessageBox.Show("You are already registered as a user.","Error",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else 
                {
                    MessageBox.Show("Fill all the fields!!","Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        int check(string email)
        {
            conn.Open();
            string query = "SELECT COUNT(*) FROM [user] WHERE email ='" + txtBoxEmail + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            int v= (int)cmd.ExecuteScalar();
            conn.Close();
            return v;
        }

        private void linkLblLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }
    }
}
