using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ABC_Traders
{
    public class CarService
    {
        private readonly string _connection;

        public CarService(string connection)
        {
            _connection = connection;
        }

        //Add new car details
        public bool AddCar(Cars car)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = "INSERT INTO [cars] (model, brand, colour, year, body_type, price) VALUES (@model, @brand, @colour, @year, @body_type, @price)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@model", car.Model);
                    cmd.Parameters.AddWithValue("@brand", car.Brand);
                    cmd.Parameters.AddWithValue("@colour", car.Colour);
                    cmd.Parameters.AddWithValue("@year", car.Year);
                    cmd.Parameters.AddWithValue("@body_type", car.BodyType);
                    cmd.Parameters.AddWithValue("@price", car.Price);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        public bool CarExists(string carId)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = "SELECT COUNT(*) FROM [cars] WHERE car_id = @carId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@carId", carId);

                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }


        //Update car details
        public bool UpdateCar(Cars car)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = @"UPDATE [cars] 
                        SET model = @Model, 
                        brand = @Brand, 
                        colour = @Colour, 
                        year = @Year, 
                        body_type = @BodyType
                    WHERE car_id = @CarID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@CarID", car.CarID);
                    cmd.Parameters.AddWithValue("@Model", car.Model);
                    cmd.Parameters.AddWithValue("@Brand", car.Brand);
                    cmd.Parameters.AddWithValue("@Colour", car.Colour);
                    cmd.Parameters.AddWithValue("@Year", car.Year);
                    cmd.Parameters.AddWithValue("@BodyType", car.BodyType);
                    cmd.Parameters.AddWithValue("@Price", car.Price);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        //delete car details
        public bool DeleteCar(int carId)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = "DELETE FROM [cars] WHERE car_id = @carId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@carId", carId);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }


        //Car Search functionality
        public DataTable SearchCars(string searchQuery)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                string query = @"SELECT car_id, model, brand, colour, year, body_type, price FROM [cars] 
                         WHERE model LIKE @searchQuery 
                         OR brand LIKE @searchQuery 
                         OR colour LIKE @searchQuery 
                         OR year LIKE @searchQuery 
                         OR body_type LIKE @searchQuery 
                         OR price LIKE @searchQuery";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }




    }
}
