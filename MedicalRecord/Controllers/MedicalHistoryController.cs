using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using MedicalRecord.Models;

namespace MedicalRecord.Controllers
{
    public class MedicalHistoryController : Controller
    {
        private readonly string connectionString = "Data Source=DESKTOP-M5LLFFV;Initial Catalog=WPF;Integrated Security=True";

        public IActionResult Index(int clientId)
        {
            Client client = GetClientFromDatabase(clientId);

            return View(client);
        }

        private Client GetClientFromDatabase(int clientId)
        {
            Client client = new Client();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM clients WHERE id = @ClientId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClientId", clientId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            client.Id = (int)reader["id"];
                            client.FirstName = reader["firstName"].ToString();
                            client.LastName = reader["lastName"].ToString();
                            client.Vaccines = reader["vaccines"].ToString();
                            client.Diagnosis = reader["diagnosis"].ToString();
                            client.Treatment = reader["treatment"].ToString();
                            client.VisitDateTime = (DateTime)reader["visitDateTime"];
                        }
                    }
                }
            }

            return client;
        }

        [HttpPost]
        public IActionResult Update(Client updatedClient)
        {
            UpdateClientInDatabase(updatedClient); // Update the database record
            return Json(new { success = true });   // Return a JSON response indicating success
        }

        private void UpdateClientInDatabase(Client updatedClient)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE clients " +
                               "SET firstName = @FirstName, lastName = @LastName, vaccines = @Vaccines, diagnosis = @Diagnosis, treatment = @Treatment " +
                               "WHERE id = @ClientId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClientId", updatedClient.Id);
                    command.Parameters.AddWithValue("@FirstName", updatedClient.FirstName);
                    command.Parameters.AddWithValue("@LastName", updatedClient.LastName);
                    command.Parameters.AddWithValue("@Vaccines", updatedClient.Vaccines);
                    command.Parameters.AddWithValue("@Diagnosis", updatedClient.Diagnosis);
                    command.Parameters.AddWithValue("@Treatment", updatedClient.Treatment);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
