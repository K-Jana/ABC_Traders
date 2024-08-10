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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ABC_Traders
{
    public partial class AdminHomeForm : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=MALIFICENT;Initial Catalog=ABCCarTraders;Integrated Security=True;TrustServerCertificate=True");

        private readonly UserService _userService;
        private readonly CarService _carService;
        private readonly PartService _partService;
        private readonly CarOrderService _carOrderService;
        private readonly PartOrderService _partOrderService;
        public AdminHomeForm()
        {
            InitializeComponent();
            string connection = @"Data Source=MALIFICENT;Initial Catalog=ABCCarTraders;Integrated Security=True;TrustServerCertificate=True";
            _userService = new UserService(connection);
            _carService = new CarService(connection);
            _partService = new PartService(connection);
            _carOrderService = new CarOrderService(connection);
            _partOrderService = new PartOrderService(connection);

        }

        private void AdminHomeForm_Load(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            string search_query = txtBoxSearch.Text;
            string query = @"SELECT user_id, First_name, Last_name,NIC,email,address,username,password FROM [user] 
                      WHERE First_name LIKE @search_query 
                       OR Last_name LIKE @search_query 
                       OR NIC LIKE @search_query 
                       OR email LIKE @search_query 
                       OR address LIKE @search_query 
                       OR username LIKE @search_query";
            try
            {
                conn.Open();
                SqlCommand cmd =new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@search_query", "%" + search_query + "%");
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGridViewCustomer.DataSource = dataSet.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                conn.Close(); 
            }
            
            
        }

        private void tabCustomer_Click(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {


                if (string.IsNullOrWhiteSpace(txtBoxFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtBoxLastName.Text) ||
                string.IsNullOrWhiteSpace(txtBoxNIC.Text) ||
                string.IsNullOrWhiteSpace(txtBoxEmail.Text) ||
                string.IsNullOrWhiteSpace(txtBoxAddress.Text) ||
                string.IsNullOrWhiteSpace(txtBoxUsername.Text) ||
                string.IsNullOrWhiteSpace(txtBoxPassword.Text))
                {
                    MessageBox.Show("Please fill all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string email = txtBoxEmail.Text;
                if (check(email) != 0)
                {
                    MessageBox.Show("Email is already registered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                User newUser = new User
                {
                    FirstName = txtBoxFirstName.Text,
                    LastName = txtBoxLastName.Text,
                    NIC = txtBoxNIC.Text,
                    Email = txtBoxEmail.Text,
                    Address = txtBoxAddress.Text,
                    Username = txtBoxUsername.Text,
                    Password = txtBoxPassword.Text,
                    UserType = 0 // Registration for cutomers
                };

                bool isRegistered = _userService.RegisterUser(newUser);
                if (isRegistered)
                {
                    MessageBox.Show("User registered successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   
                    
                }
                else
                {
                    MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    
        //Email check function
        int check(string email)
        {
            conn.Open();
            string query = "SELECT COUNT(*) FROM [user] WHERE email ='" + txtBoxEmail + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            int v = (int)cmd.ExecuteScalar();
            conn.Close();
            return v;
        }
        //userId check function
        int CheckUserId(int userId)
        {
            conn.Open();
            string query = "SELECT COUNT(*) FROM [user] WHERE user_id = @UserId";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            int result = (int)cmd.ExecuteScalar();
            conn.Close();
            return result;
        }
        //CarID check function
        int CheckCarId(int carId)
        {
            conn.Open();
            string query = "SELECT COUNT(*) FROM [car] WHERE car_id = @CarId";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CarId", carId);
            int result = (int)cmd.ExecuteScalar();
            conn.Close();
            return result;
        }
        //PartId check function
        int CheckPartId(int partId)
        {
            conn.Open();
            string query = "SELECT COUNT(*) FROM [part] WHERE part_id = @PartId";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PartId", partId);
            int result = (int)cmd.ExecuteScalar();
            conn.Close();
            return result;
        }
        //OrderID check function
        int CheckOrderExists(int orderId, int userId, int carId, int partId)
        {
            conn.Open();
            string query = "SELECT COUNT(*) FROM [order] WHERE order_id = @OrderId AND user_id = @UserId AND car_id = @CarId AND part_id = @PartId";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@OrderId", orderId);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@CarId", carId);
            cmd.Parameters.AddWithValue("@PartId", partId);
            int result = (int)cmd.ExecuteScalar();
            conn.Close();
            return result;
        }

        private void txtBoxFirstName_TextChanged(object sender, EventArgs e)
        {

        }


        //Update user details
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtBoxUserID.Text) ||
                 string.IsNullOrWhiteSpace(txtBoxFirstName.Text) ||
                 string.IsNullOrWhiteSpace(txtBoxLastName.Text) ||
                 string.IsNullOrWhiteSpace(txtBoxNIC.Text) ||
                 string.IsNullOrWhiteSpace(txtBoxEmail.Text) ||
                 string.IsNullOrWhiteSpace(txtBoxAddress.Text) ||
                 string.IsNullOrWhiteSpace(txtBoxUsername.Text) ||
                 string.IsNullOrWhiteSpace(txtBoxPassword.Text))
                {
                    MessageBox.Show("Please fill all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int userId;
                if (!int.TryParse(txtBoxUserID.Text, out userId))
                {
                    MessageBox.Show("Please enter a valid user ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                User user = new User
                {
                    UserId = userId,
                    FirstName = txtBoxFirstName.Text,
                    LastName = txtBoxLastName.Text,
                    NIC = txtBoxNIC.Text,
                    Email = txtBoxEmail.Text,
                    Address = txtBoxAddress.Text,
                    Username = txtBoxUsername.Text,
                    Password = txtBoxPassword.Text
                };

                bool isUpdated = _userService.UpdateUser(user);
                if (isUpdated)
                {
                    MessageBox.Show("User details updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No user found with the specified ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }

        
    }

        //Delete users
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int userId;
            if (!int.TryParse(txtBoxUserID.Text, out userId))
            {
                MessageBox.Show("Please enter a valid user ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool isDeleted = _userService.DeleteCustomer(userId);
            if (isDeleted)
            {
                MessageBox.Show("User deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No user found with the specified ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtBoxColour_TextChanged(object sender, EventArgs e)
        {

        }

        //Add car to the database
        private void btnAddcars_Click(object sender, EventArgs e)
        {
            try {
                if (!string.IsNullOrWhiteSpace(txtBoxModel.Text) &&
                    !string.IsNullOrWhiteSpace(txtBoxBrand.Text) &&
                    !string.IsNullOrWhiteSpace(txtBoxColour.Text) &&
                    !string.IsNullOrWhiteSpace(txtBoxYear.Text) &&
                    !string.IsNullOrWhiteSpace(txtBoxBodyType.Text) &&
                    !string.IsNullOrWhiteSpace(txtBoxprice.Text))
                {
                    if (!_carService.CarExists(txtBoxCarID.Text))
                    {
                        Cars car = new Cars
                        {
                            Model = txtBoxModel.Text,
                            Brand = txtBoxBrand.Text,
                            Colour = txtBoxColour.Text,
                            Year = txtBoxYear.Text,
                            BodyType = txtBoxBodyType.Text,
                            Price = decimal.Parse(txtBoxprice.Text)
                        };

                        bool isAdded = _carService.AddCar(car);

                        if (isAdded)
                        {
                            MessageBox.Show("Car registered successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to register car", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("The car has already been registered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("Fill all the fields!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Update car details
        private void btnUpdateCars_Click(object sender, EventArgs e)
        {
            try
            {
                int carId;
                if (!int.TryParse(txtBoxCarID.Text, out carId))
                {
                    MessageBox.Show("Please enter a valid car ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                    var car = new Cars
                    {
                        Model = txtBoxModel.Text,
                        Brand = txtBoxBrand.Text,
                        Colour = txtBoxColour.Text,
                        Year = txtBoxYear.Text,
                        BodyType = txtBoxBodyType.Text,
                        Price = decimal.Parse(txtBoxprice.Text)
                    };
                    bool isUpdated = _carService.UpdateCar(car);

                    if (isUpdated)
                    {
                        MessageBox.Show("Car details updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No car found with the specified ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        //delete car from the database
        private void btnDeleteCars_Click(object sender, EventArgs e)
        {
            int carId;
            if (!int.TryParse(txtBoxUserID.Text, out carId))
            {
                MessageBox.Show("Please enter a valid Car ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool isDeleted = _carService.DeleteCar(carId);
            if (isDeleted)
            {
                MessageBox.Show("Car entry deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No car found with the specified ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Search cars from the database
        private void btnSearchCars_Click(object sender, EventArgs e)
        {
            string search_query = txtBoxSearch.Text;
            string query = @"SELECT car_id,model,brand,colour,year,body_type,price FROM [cars] 
                      WHERE model LIKE @search_query 
                       OR brand LIKE @search_query 
                       OR colour LIKE @search_query 
                       OR year LIKE @search_query 
                       OR body_type LIKE @search_query 
                       OR price LIKE @search_query";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@search_query", "%" + search_query + "%");
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGridViewCars.DataSource = dataSet.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void tabParts_Click(object sender, EventArgs e)
        {

        }
        //Add parts function implementation
        private void btnPartsAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //I have some doubts
                if (//!string.IsNullOrWhiteSpace(txtBoxpartID.Text) &&
                    !string.IsNullOrWhiteSpace(txtBoxPartBrand.Text) &&
                    !string.IsNullOrWhiteSpace(txtBoxPartCategory.Text) &&
                    !string.IsNullOrWhiteSpace(txtBoxPartColour.Text) &&
                    !string.IsNullOrWhiteSpace(txtBoxPartSize.Text) &&
                    !string.IsNullOrWhiteSpace(txtBoxPartWarranty.Text) &&
                    !string.IsNullOrWhiteSpace(txtBoxPartPrice.Text))
                {
                    if (!_partService.PartExists(txtBoxPartID.Text))
                    {
                        Parts part = new Parts
                        {
                            Brand = txtBoxPartBrand.Text,
                            Category = txtBoxPartCategory.Text,
                            Colour = txtBoxPartColour.Text,
                            Size = txtBoxPartSize.Text,
                            Warranty = int.Parse(txtBoxBodyType.Text),
                            Price = decimal.Parse(txtBoxPartPrice.Text)
                        };

                        bool isAdded = _partService.RegisterParts(part);

                        if (isAdded)
                        {
                            MessageBox.Show("Part registered successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to register part", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("The part has already been registered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("Fill all the fields!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //Update parts function implementation
        private void btnPartsUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int partId;
                if (!int.TryParse(txtBoxPartID.Text, out partId))
                {
                    MessageBox.Show("Please enter a valid part ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var part = new Parts
                {
                    Brand = txtBoxPartBrand.Text,
                    Category = txtBoxPartCategory.Text,
                    Colour = txtBoxPartColour.Text,
                    Size = txtBoxPartSize.Text,
                    Warranty = int.Parse(txtBoxBodyType.Text),
                    Price = decimal.Parse(txtBoxPartPrice.Text)
                };
                bool isUpdated = _partService.UpdatePart(part);

                if (isUpdated)
                {
                    MessageBox.Show("Part details updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No part found with the specified ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Delete function parts iimplementation
        private void btnPartsDelete_Click(object sender, EventArgs e)
        {
            int partId;
            if (!int.TryParse(txtBoxPartID.Text, out partId))
            {
                MessageBox.Show("Please enter a valid Part ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool isDeleted = _partService.DeletePart(partId);
            if (isDeleted)
            {
                MessageBox.Show("Part entry deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No part found with the specified ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Search parts function implementation
        private void btnPartsSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchQuery = txtBoxPartsSearch.Text.Trim();

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    // Call the SearchParts method from the PartService
                    DataTable partsData = _partService.SearchParts(searchQuery);

                    if (partsData != null && partsData.Rows.Count > 0)
                    {
                        // Display the results in the DataGridView
                        dataGridViewParts.DataSource = partsData;
                    }
                    else
                    {
                        MessageBox.Show("No parts found matching the search criteria.", "No Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridViewParts.DataSource = null; // Clear previous results
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a search query.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tab_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAddCarOrder_Click(object sender, EventArgs e)
        {
            try
            {
                CarOrder carOrder = new CarOrder
                {
                    OrderID = int.Parse(txtBoxCarOrderOrderID.Text),
                    CarID = int.Parse(txtBoxCarID.Text),
                    Status = cmbBoxCarOrderStatus.SelectedText,
                    UserID = int.Parse(txtBoxCarOrderCustomerID.Text),
                    OrderDate = DateTime.Parse(dateTimePickerCarOrder.Text)
                };

                bool success = _carOrderService.RegisterCarOrder(carOrder);
                if (success)
                {
                    MessageBox.Show("Car order registered successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to register car order.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdateCarOrder_Click(object sender, EventArgs e)
        {
            try
            {
                CarOrder carOrder = new CarOrder
                {
                    OrderID = int.Parse(txtBoxCarOrderOrderID.Text),
                    CarID = int.Parse(txtBoxCarID.Text),
                    Status = cmbBoxCarOrderStatus.SelectedText,
                    UserID = int.Parse(txtBoxCarOrderCustomerID.Text),
                    OrderDate = DateTime.Parse(dateTimePickerCarOrder.Text)
                };

                bool success = _carOrderService.UpdateCarOrder(carOrder);
                if (success)
                {
                    MessageBox.Show("Car order updated successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to update car order.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteCarOrder_Click(object sender, EventArgs e)
        {
            try
            {
                int orderId = int.Parse(txtBoxCarOrderOrderID.Text);

                bool success = _carOrderService.DeleteCarOrder(orderId);
                if (success)
                {
                    MessageBox.Show("Car order deleted successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to delete car order.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtBoxSearchCarOrder_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSearchCarOrder_Click(object sender, EventArgs e)
        {
            try
            {
                string searchQuery = txtBoxSearchCarOrder.Text;

                DataTable results = _carOrderService.SearchCarOrders(searchQuery);
                dataGridViewCarOrder.DataSource = results;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnPartOrderUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                PartOrder partOrder = new PartOrder
                {
                    OrderID = int.Parse(txtBoxPartOrderID.Text),
                    PartID = int.Parse(txtBoxPartOrderPartID.Text),
                    Status = cmbBoxPartOrderStatus.SelectedText,
                    UserID = int.Parse(txtBoxPartOrderCustomerID.Text),
                    OrderDate = DateTime.Parse(dateTimePickerPartOrder.Text)
                };

                bool success = _partOrderService.UpdatePartOrder(partOrder);
                if (success)
                {
                    MessageBox.Show("Part order updated successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to update part order.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPartOrderDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int orderId = int.Parse(txtBoxPartOrderID.Text);

                bool success = _partOrderService.DeletePartOrder(orderId);
                if (success)
                {
                    MessageBox.Show("Part order deleted successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to delete part order.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPartOrderSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchQuery = txtBoxPartOrderSearch.Text;

                DataTable results = _partOrderService.SearchPartOrders(searchQuery);
                dataGridViewPartOrder.DataSource = results;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPartOrderAdd_Click(object sender, EventArgs e)
        {
            try
            {
                PartOrder partOrder = new PartOrder
                {
                    OrderID = int.Parse(txtBoxPartOrderID.Text),
                    PartID = int.Parse(txtBoxPartOrderPartID.Text),
                    Status = cmbBoxPartOrderStatus.SelectedText,
                    UserID = int.Parse(txtBoxPartOrderCustomerID.Text),
                    OrderDate = DateTime.Parse(dateTimePickerPartOrder.Text)
                };

                
                bool success = _partOrderService.RegisterPartOrder(partOrder);
                if (success)
                {
                    MessageBox.Show("Part order registered successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to register part order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabPartOrder_Click(object sender, EventArgs e)
        {

        }
    }
}
