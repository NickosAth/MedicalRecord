using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using MedicalRecord.Models;

namespace MedicalRecord.Controllers
{
    public class LoginController : Controller
    { 
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginPost(DoctorLogin doctorLogin)
        {
            string connectionString = "Data Source=DESKTOP-M5LLFFV;Initial Catalog=WPF;Integrated Security=True";
            // Connect to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Prepare the query
                string query = "SELECT FirstName FROM doctors WHERE username=@Username AND password=@Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", doctorLogin.Username);
                command.Parameters.AddWithValue("@Password", doctorLogin.Password);

                // Execute the query and check the result
                string firstName = (string)command.ExecuteScalar();
                if (firstName != null)
                {
                    // Set the LoggedInDoctorFirstName value in the ViewData dictionary
                    ViewData["LoggedInDoctorFirstName"] = firstName;

                    return RedirectToAction("Index", "MainMenu");
                }
                else
                {
                    ViewBag.ErrorMessage = "Λανθασμένα Στοιχεία Χρήστη!";
                    return View("Index", "Login");
                }
            }

        }
    }
}
