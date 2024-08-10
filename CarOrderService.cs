using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC_Traders
{
    internal class CarOrderService
    {
        private readonly string _connection;
        public CarOrderService(string connection)
        {
            _connection = connection;
        }

        public bool RegisterCarOrder(CarOrder carOrder)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = @"INSERT INTO car_order (order_id, car_id, status, user_id, date) 
                         VALUES (@OrderID, @CarID, @Status, @UserID, @Date)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", carOrder.OrderID);
                    cmd.Parameters.AddWithValue("@CarID", carOrder.CarID);
                    cmd.Parameters.AddWithValue("@Status", carOrder.Status);
                    cmd.Parameters.AddWithValue("@UserID", carOrder.UserID);
                    cmd.Parameters.AddWithValue("@Date", carOrder.OrderDate);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        public bool UpdateCarOrder(CarOrder carOrder)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = @"UPDATE CarOrders 
                        SET car_id = @CarID, 
                            status = @Status, 
                            user_id = @UserID, 
                            date = @Date
                        WHERE order_id = @OrderID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", carOrder.OrderID);
                    cmd.Parameters.AddWithValue("@CarID", carOrder.CarID);
                    cmd.Parameters.AddWithValue("@Status", carOrder.Status);
                    cmd.Parameters.AddWithValue("@UserID", carOrder.UserID);
                    cmd.Parameters.AddWithValue("@Date", carOrder.OrderDate);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public bool DeleteCarOrder(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = "DELETE FROM CarOrders WHERE order_id = @OrderID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public DataTable SearchCarOrders(string searchQuery)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = @"SELECT c.brand, c.colour, c.body_type AS body_model, o.order_id, o.date, o.user_id
                        FROM CarOrders o
                        JOIN Cars c ON o.car_id = c.car_id
                        WHERE o.order_id LIKE @searchQuery
                           OR o.date LIKE @searchQuery
                           OR o.user_id LIKE @searchQuery";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }



    }
}
