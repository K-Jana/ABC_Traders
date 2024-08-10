using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Net;

namespace ABC_Traders
{
    public class UserService
    {
        private readonly string _connection;
        public UserService(string connection)
        {
            _connection = connection;
        }

        //User Register function
        public bool RegisterUser(User user)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = "INSERT INTO [user] (First_name, Last_name, NIC, email, address, username, password, user_type) VALUES (@First_name, @last_name, @NIC, @email, @address, @username, @password, @user_type)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@First_name", user.FirstName);
                cmd.Parameters.AddWithValue("@last_name", user.LastName);
                cmd.Parameters.AddWithValue("@NIC", user.NIC);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@address", user.Address);
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@user_type", user.UserType);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        //Customer Search function
        public DataTable SearchUsers(string searchQuery)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = @"SELECT user_id, First_name, Last_name, NIC, email, address, username, password FROM [user] 
                            WHERE First_name LIKE @searchQuery 
                            OR Last_name LIKE @searchQuery 
                            OR NIC LIKE @searchQuery 
                            OR email LIKE @searchQuery 
                            OR address LIKE @searchQuery 
                            OR username LIKE @searchQuery";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        //Customer update function
        public bool UpdateUser(User user) 
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = @"UPDATE [user] 
                        SET First_name = @FirstName, 
                        Last_name = @LastName, 
                        NIC = @NIC, 
                        email = @Email, 
                        address = @Address, 
                        username = @Username, 
                        password = @Password
                    WHERE user_id = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@UserID", user.UserId);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@NIC", user.NIC);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Address", user.Address);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }

        }

        //Customer delete function
        public bool DeleteCustomer(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = "DELETE FROM [user] WHERE user_id = @userId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }


    }
}
