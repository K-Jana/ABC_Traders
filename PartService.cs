using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC_Traders
{
    public class PartService
    {
        private readonly string _connection;
        public PartService(string connection)
        {
            _connection = connection;
        }

        //Add parts to the database
        public bool RegisterParts(Parts part)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = "INSERT INTO [parts] (part_id,brand,category,colour,size,warranty,price) VALUES (@part_id,@brand, @category, @colour, @size, @warranty,@price)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@part_id", part.PartID);
                cmd.Parameters.AddWithValue("@brand", part.Brand);
                cmd.Parameters.AddWithValue("@category", part.Category);
                cmd.Parameters.AddWithValue("@colour", part.Colour);
                cmd.Parameters.AddWithValue("@size", part.Size);
                cmd.Parameters.AddWithValue("@warranty", part.Warranty);
                cmd.Parameters.AddWithValue("@price", part.Price);
                

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool PartExists(string partID)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = "SELECT COUNT(*) FROM [parts] WHERE part_id = @partID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@partID", partID);

                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool UpdatePart(Parts part)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = @"UPDATE [parts] 
                        SET brand = @Brand, 
                            category = @Category, 
                            colour = @Colour, 
                            size = @Size, 
                            warranty = @Warranty,
                            price = @Price
                        WHERE part_id = @PartID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PartID", part.PartID);
                    cmd.Parameters.AddWithValue("@Brand", part.Brand);
                    cmd.Parameters.AddWithValue("@Category", part.Category);
                    cmd.Parameters.AddWithValue("@Colour", part.Colour);
                    cmd.Parameters.AddWithValue("@Size", part.Size);
                    cmd.Parameters.AddWithValue("@Warranty", part.Warranty);
                    cmd.Parameters.AddWithValue("@Price", part.Price);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public bool DeletePart(int partId)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = "DELETE FROM [parts] WHERE part_id = @PartID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PartID", partId);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public DataTable SearchParts(string searchQuery)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = @"SELECT part_id, brand, category, colour, size, warranty, price 
                         FROM [parts] 
                         WHERE brand LIKE @SearchQuery 
                            OR category LIKE @SearchQuery 
                            OR colour LIKE @SearchQuery 
                            OR size LIKE @SearchQuery 
                            OR warranty LIKE @SearchQuery";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }



    }
}
