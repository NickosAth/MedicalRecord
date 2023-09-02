using Microsoft.AspNetCore.Mvc;
using MedicalRecord.Models;
using System.Data.SqlClient;

namespace MedicalRecord.Controllers
{
    public class ShowDoctorProfileController : Controller
    {
        private readonly string connectionString = "Data Source=DESKTOP-M5LLFFV;Initial Catalog=WPF;Integrated Security=True";

        public IActionResult Index()
        {
            // Authenticate the doctor by checking if they are logged in
            int? loggedInDoctorId = GetLoggedInDoctorId();

            if (loggedInDoctorId != null)
            {
                // Retrieve the doctor's information using the logged-in doctor's ID
                Doctor doctor = GetDoctorById(loggedInDoctorId.Value);

                if (doctor != null)
                {
                    return View(doctor);
                }
                else
                {
                    // Redirect to login page or show an appropriate message
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                // Redirect to login page if the doctor is not logged in
                return RedirectToAction("Index", "Login");
            }
        }

        private int GetLoggedInDoctorId()
        {
            if (Request.Cookies.TryGetValue("LoggedInDoctorId", out string loggedInDoctorId))
            {
                if (int.TryParse(loggedInDoctorId, out int doctorId))
                {
                    return doctorId;
                }
            }
            return -1;
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("LoggedInDoctorId");
            // Add any other logout logic you need.
            return RedirectToAction("Index", "Login");
        }

        private Doctor GetDoctorById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM doctors WHERE id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Doctor doctor = new Doctor
                            {
                                Id = (int)reader["id"],
                                FirstName = reader["firstName"].ToString(),
                                LastName = reader["lastName"].ToString(),
                                Username = reader["username"].ToString(),
                                Password = reader["password"].ToString(),
                                Gender = reader["gender"].ToString(),
                                Phone = reader["phone"].ToString(),
                                Email = reader["email"].ToString()
                                // ... Add other properties here
                            };

                            return doctor;
                        }
                    }
                }
            }

            return null; // Doctor with the given ID not found
        }
    }
}
