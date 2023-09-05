using MedicalRecord.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Security.Claims;

namespace MedicalRecord.Controllers
{
    public class CreateDoctorController : Controller
    {
        private string connectionString = "Data Source=DESKTOP-M5LLFFV;Initial Catalog=WPF;Integrated Security=True";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Doctor doctor)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string queryCheckEmail = "SELECT COUNT(*) FROM doctors WHERE email = @Email";

                using (SqlCommand checkEmailCommand = new SqlCommand(queryCheckEmail, connection))
                {
                    checkEmailCommand.Parameters.AddWithValue("@Email", doctor.Email);

                    connection.Open();
                    int emailCount = (int)checkEmailCommand.ExecuteScalar();

                    if (emailCount > 0)
                    {
                        // Email already exists, return an error message to the client
                        TempData["DoctorCreatedMessage"] = "To Email χρησιμοποιείται ήδη!";
                        return RedirectToAction("Index", "Login");
                    }
                }

                // Proceed with inserting the doctor into the database
                string queryInsertDoctor = "INSERT INTO doctors (firstName, lastName, username, password, gender, phone, email) " +
                                           "VALUES (@FirstName, @LastName, @Username, @Password, @Gender, @Phone, @Email)";

                using (SqlCommand insertDoctorCommand = new SqlCommand(queryInsertDoctor, connection))
                {
                    insertDoctorCommand.Parameters.AddWithValue("@FirstName", doctor.FirstName);
                    insertDoctorCommand.Parameters.AddWithValue("@LastName", doctor.LastName);
                    insertDoctorCommand.Parameters.AddWithValue("@Username", doctor.Username);
                    insertDoctorCommand.Parameters.AddWithValue("@Password", doctor.Password);
                    insertDoctorCommand.Parameters.AddWithValue("@Gender", doctor.Gender);
                    insertDoctorCommand.Parameters.AddWithValue("@Phone", doctor.Phone);
                    insertDoctorCommand.Parameters.AddWithValue("@Email", doctor.Email);

                    // Perform the insertion
                    try
                    {
                        insertDoctorCommand.ExecuteNonQuery();
                        TempData["DoctorCreatedMessage"] = "Το προφίλ ιατρού σας δημιουργήθηκε με επιτυχία!";

                        // Introduce a 3-second delay (for demonstration/debugging purposes)
                        Thread.Sleep(3000); // Sleep for 3 seconds

                        return RedirectToAction("Index", "Login");
                    }
                    catch (SqlException ex)
                    {
                        // Handle SQL exceptions, log them, or return an appropriate error message
                        TempData["DoctorCreatedMessage"] = "Υπήρξε πρόβλημα κατά τη δημιουργία του ιατρού!";

                        // Introduce a 3-second delay (for demonstration/debugging purposes)
                        Thread.Sleep(3000); // Sleep for 3 seconds

                        return RedirectToAction("Index", "Login");
                    }
                }
            }
        }

        [HttpGet]
        public JsonResult username(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Json(new { exists = false }); // Return false if value is empty
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM doctors WHERE username = @Value";
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
        public JsonResult phone(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Json(new { exists = false }); // Return false if value is empty
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM doctors WHERE phone = @Value";
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
        public JsonResult email(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Json(new { exists = false }); // Return false if value is empty
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM doctors WHERE email = @Value";
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
