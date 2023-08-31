using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using MedicalRecord.Models;
using System.Data;

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

                string query = "SELECT firstName, lastName, vaccines, diagnosis, treatment, visitDatetime " +
                               "FROM clients " +
                               "WHERE id = @ClientId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClientId", clientId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            client.FirstName = reader["firstName"].ToString();
                            client.LastName = reader["lastName"].ToString();
                            client.Vaccines = reader["vaccines"].ToString();
                            client.Diagnosis = reader["diagnosis"].ToString();
                            client.Treatment = reader["treatment"].ToString();
                            client.VisitDateTime = (DateTime)reader["visitDatetime"];
                        }
                    }
                }
            }

            return client;
        }
        public IActionResult Edit(int clientId)
        {
            Client client = GetClientFromDatabase(clientId);
            return View(client);
        }

        [HttpPost]
        public IActionResult Update(Client updatedClient)
        {
            UpdateClientInDatabase(updatedClient);
            return Json(new { success = true });
        }

        private void UpdateClientInDatabase(Client updatedClient)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE clients " +
                               "SET firstName = @FirstName, lastName = @LastName, " +
                               "vaccines = @Vaccines, diagnosis = @Diagnosis, treatment = @Treatment " +
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