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
                    ViewBag.Message = "Doctor successfully created!";
                }
            }

            return RedirectToAction("Index", "Login");
        } 
    }
}
