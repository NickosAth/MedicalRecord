using MedicalRecord.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Security.Claims;

namespace MedicalRecord.Controllers
{
	public class ShowDoctorsProfileController : Controller
	{
        string connectionString = "Data Source=DESKTOP-M5LLFFV;Initial Catalog=WPF;Integrated Security=True";

        public IActionResult Index()
		{
			return View();
		}

        public IActionResult Profile()
        {
            // Retrieve the currently logged in doctor's username
            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Index", "Home");
            }

            Doctor doctor;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM doctors WHERE username=@Username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            doctor = new Doctor
                            {
                                FirstName = reader.GetString(0),
                                LastName = reader.GetString(1),
                                Username = reader.GetString(2),
                                Password = reader.GetString(3),
                                Gender = reader.GetString(4),
                                Phone = reader.GetString(5),
                                Email = reader.GetString(6)
                            };

                            return View(doctor);
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
