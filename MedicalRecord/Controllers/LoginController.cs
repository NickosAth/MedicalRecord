using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using MedicalRecord.Models;

namespace MedicalRecord.Controllers
{
    public class LoginController : Controller
    {
        private readonly string _connectionString = "Data Source=DESKTOP-M5LLFFV;Initial Catalog=WPF;Integrated Security=True";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginPost(DoctorLogin doctorLogin)
        {
            int loggedInDoctorId = AuthenticateDoctor(doctorLogin);

            if (loggedInDoctorId != -1)
            {
                TempData["LoggedInDoctorId"] = loggedInDoctorId;
                return RedirectToAction("Index", "MainMenu");
            }
            else
            {
                ViewBag.ErrorMessage = "Λανθασμένα Στοιχεία Χρήστη!";
                return View("Index", "Login");
            }
        }

        private int AuthenticateDoctor(DoctorLogin doctorLogin)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT Id FROM doctors WHERE username=@Username AND password=@Password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", doctorLogin.Username);
                    command.Parameters.AddWithValue("@Password", doctorLogin.Password);

                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return (int)result;
                    }
                }
            }

            return -1; // Authentication failed
        }
    }
}