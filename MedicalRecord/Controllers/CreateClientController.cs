using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using MedicalRecord.Models;

namespace MedicalRecord.Controllers
{
    public class CreateClientController : Controller
    {
        string connectionString = "Data Source=DESKTOP-M5LLFFV;Initial Catalog=WPF;Integrated Security=True";

        public IActionResult Index()
        {
            return View(new Client());
        }

        [HttpPost]
        public ActionResult Create(Client client)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open(); // Open the connection here

                string checkQuery = "SELECT COUNT(*) FROM clients WHERE amka = @Amka OR phone = @Phone OR email = @Email";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Amka", client.Amka);
                    checkCommand.Parameters.AddWithValue("@Phone", client.Phone);
                    checkCommand.Parameters.AddWithValue("@Email", client.Email);

                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        TempData["ClientExistsMessage"] = "Client with the provided AMKA, phone, or email already exists.";
                        return View("Index", client);
                    }
                }

                string insertQuery = "INSERT INTO clients (firstName, lastName, fathersName, gender, dob, amka, job, insurance, familyStatus, phone, email, areaOfResidence, cityOfResidence, addressOfResidence, zipCodeOfResidence) " +
                                    "VALUES (@FirstName, @LastName, @FatherName, @Gender, @Dob, @Amka, @Job, @Insurance, @FamilyStatus, @Phone, @Email, @AreaOfResidence, @CityOfResidence, @AddressOfResidence, @ZipCodeOfResidence)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", client.FirstName);
                    command.Parameters.AddWithValue("@LastName", client.LastName);
                    command.Parameters.AddWithValue("@FatherName", client.FathersName);
                    command.Parameters.AddWithValue("@Gender", client.Gender);
                    command.Parameters.AddWithValue("@Dob", client.Dob);
                    command.Parameters.AddWithValue("@Amka", client.Amka);
                    command.Parameters.AddWithValue("@Job", client.Job);
                    command.Parameters.AddWithValue("@Insurance", client.Insurance);
                    command.Parameters.AddWithValue("@FamilyStatus", client.FamilyStatus);
                    command.Parameters.AddWithValue("@Phone", client.Phone);
                    command.Parameters.AddWithValue("@Email", client.Email);
                    command.Parameters.AddWithValue("@AreaOfResidence", client.AreaOfResidence);
                    command.Parameters.AddWithValue("@CityOfResidence", client.CityOfResidence);
                    command.Parameters.AddWithValue("@AddressOfResidence", client.AddressOfResidence);
                    command.Parameters.AddWithValue("@ZipCodeOfResidence", client.ZipCodeOfResidence);

                    command.ExecuteNonQuery();
                }

                // Close the connection when you're done
                connection.Close();
            }

            return RedirectToAction("Index", "ShowClients");
        }

        [HttpGet]
        public JsonResult Amka(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Json(new { exists = false }); // Return false if value is empty
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM clients WHERE amka = @Value";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Value", value);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return Json(new { exists = count > 0 });
                }
            }
        }

        [HttpGet]
        public JsonResult Phone(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Json(new { exists = false }); // Return false if value is empty
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM clients WHERE phone = @Value";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Value", value);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return Json(new { exists = count > 0 });
                }
            }
        }

        [HttpGet]
        public JsonResult Email(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Json(new { exists = false }); // Return false if value is empty
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM clients WHERE email = @Value";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Value", value);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return Json(new { exists = count > 0 });
                }
            }
        }
    }
}
