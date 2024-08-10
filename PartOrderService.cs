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
    internal class PartOrderService
    {
        private readonly string _connection;
        public PartOrderService(string connection)
        {
            _connection = connection;
        }

        //Register Part orders
        public bool RegisterPartOrder(PartOrder partOrder)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = @"INSERT INTO PartOrders (order_id, part_id, status, user_id, date) 
                         VALUES (@OrderID, @PartID, @Status, @UserID, @Date)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", partOrder.OrderID);
                    cmd.Parameters.AddWithValue("@PartID", partOrder.PartID);
                    cmd.Parameters.AddWithValue("@Status", partOrder.Status);
                    cmd.Parameters.AddWithValue("@UserID", partOrder.UserID);
                    cmd.Parameters.AddWithValue("@Date", partOrder.OrderDate);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }


        //Update Part orders
        public bool UpdatePartOrder(PartOrder partOrder)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = @"UPDATE PartOrders 
                        SET part_id = @PartID, 
                            status = @Status, 
                            user_id = @UserID, 
                            date = @Date
                        WHERE order_id = @OrderID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", partOrder.OrderID);
                    cmd.Parameters.AddWithValue("@PartID", partOrder.PartID);
                    cmd.Parameters.AddWithValue("@Status", partOrder.Status);
                    cmd.Parameters.AddWithValue("@UserID", partOrder.UserID);
                    cmd.Parameters.AddWithValue("@Date", partOrder.OrderDate);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        //Delete parts order
        public bool DeletePartOrder(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = "DELETE FROM PartOrders WHERE order_id = @OrderID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        //SearchParts order
        public DataTable SearchPartOrders(string searchQuery)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = @"SELECT p.brand, p.colour, p.category, p.warranty, p.size, o.order_id, o.date, o.user_id
                        FROM part_orders o
                        JOIN Parts p ON o.part_id = p.part_id
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
