﻿using MedicalRecord.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Add this
using System.Data.SqlClient;
using System.Security.Claims; // Add this

namespace MedicalRecord.Controllers
{
    public class DoctorProfileController : Controller
    {
        string connectionString = "Data Source=DESKTOP-M5LLFFV;Initial Catalog=WPF;Integrated Security=True";

        [Authorize] // Restrict access to authenticated users
        public IActionResult Index()
        {
            // Get the currently logged-in doctor's ID
            string loggedInDoctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get the doctor's information using the logged-in doctor's ID
            Doctor doctor = GetDoctorById(loggedInDoctorId);

            return View(doctor);
        }

        private Doctor GetDoctorById(string id)

        {
            string connectionString = "Data Source=DESKTOP-M5LLFFV;Initial Catalog=WPF;Integrated Security=True";

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
