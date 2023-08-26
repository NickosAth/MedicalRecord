using MedicalRecord.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace MedicalRecord.Controllers
{
    public class ShowDoctorProfileController : Controller
    {
        string connectionString = "Data Source=DESKTOP-M5LLFFV;Initial Catalog=WPF;Integrated Security=True";

        public IActionResult Index()
        {
            // Replace this with the logic to get the currently logged-in doctor's ID
            int loggedInDoctorId = 1; // Example ID

            // Get the doctor's information using the logged-in doctor's ID
            Doctor doctor = GetDoctorById(loggedInDoctorId);

            return View(doctor);
        }

        private Doctor GetDoctorById(int id)
        {
            // Logic to fetch doctor's information based on the provided ID
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
                                Email = reader["email"].ToString(),
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
