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
    public partial class LoginForm : Form
    {

        SqlConnection conn = new SqlConnection(@"Data Source=MALIFICENT;Initial Catalog=ABCCarTraders;Integrated Security=True;TrustServerCertificate=True");

        public LoginForm()
        {
            InitializeComponent();
        }

        

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT username, password, user_type FROM [user] WHERE username = '" +txtBoxUsername.Text+ "' AND password = '"+txtBoxPassword.Text+"' AND user_type = '"+comboBoxUserType.SelectedIndex+"'", conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dtable = new DataTable();
                sda.Fill(dtable);

                if (dtable.Rows.Count > 0)
                {   
                    //open relevant user home pages
                    if (comboBoxUserType.SelectedIndex == 0) //Customer
                    {
                        CustomerHomeForm customerHomeForm = new CustomerHomeForm();
                        customerHomeForm.Show();
                        this.Hide();
                    }
                    else if(comboBoxUserType.SelectedIndex == 1)//Administrator
                    {
                        AdminHomeForm adminHomeForm = new AdminHomeForm();
                        adminHomeForm.Show();
                        this.Hide();
                    }
                    
                }
                else 
                {
                    MessageBox.Show("Invalid login credentials","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBoxUsername.Clear();
                    txtBoxPassword.Clear();
                }
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBoxUsername.Clear();
                txtBoxPassword.Clear();
            }
            finally 
            {
                conn.Close();
            }

        }

        private void linkLblRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegistrationForm registrationform = new RegistrationForm();
            registrationform.Show();
            this.Hide();
        }
    }
}
