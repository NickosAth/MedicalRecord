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
                string query = "INSERT INTO doctors (firstName, lastName, username, password, gender, phone, email) " +
                               "VALUES (@FirstName, @LastName, @Username, @Password, @Gender, @Phone, @Email)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", doctor.FirstName);
                    command.Parameters.AddWithValue("@LastName", doctor.LastName);
                    command.Parameters.AddWithValue("@Username", doctor.Username);
                    command.Parameters.AddWithValue("@Password", doctor.Password);
                    command.Parameters.AddWithValue("@Gender", doctor.Gender);
                    command.Parameters.AddWithValue("@Phone", doctor.Phone);
                    command.Parameters.AddWithValue("@Email", doctor.Email);

                    connection.Open();
                    command.ExecuteNonQuery();

                    // Add success message to ViewBag after doctor creation
                    ViewBag.SuccessMessage = "Doctor successfully created!";
                    return RedirectToAction("Index", "Login");
                }
            }

            return RedirectToAction("Index", "Login");
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