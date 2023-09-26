using Microsoft.AspNetCore.Mvc;
using MedicalRecord.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MedicalRecord.Controllers
{
    public class MedicalHistoryController : Controller
    {
        private readonly string connectionString = "Data Source=DESKTOP-M5LLFFV;Initial Catalog=WPF;Integrated Security=True";

        public IActionResult Index(int clientId)
        {
            Client client = GetClientFromDatabase(clientId);
            List<MedicalHistoryEntry> medicalHistory = GetMedicalHistoryFromDatabase(clientId);

            // Assign the retrieved medical history to the client
            client.MedicalHistoryEntries = medicalHistory;

            return View(client);
        }

        private Client GetClientFromDatabase(int clientId)
        {
            Client client = new Client();

            // Retrieve client information from the database
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
                        }
                    }
                }
            }

            return client;
        }

        private List<MedicalHistoryEntry> GetMedicalHistoryFromDatabase(int clientId)
        {
            List<MedicalHistoryEntry> medicalHistory = new List<MedicalHistoryEntry>();

            // Retrieve medical history entries for the client from the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT VisitDateTime, Vaccines, Diagnosis, Treatment " +
                               "FROM medical_history " +
                               "WHERE ClientId = @ClientId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClientId", clientId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MedicalHistoryEntry entry = new MedicalHistoryEntry
                            {
                                VisitDateTime = (DateTime)reader["VisitDateTime"],
                                Vaccines = reader["Vaccines"].ToString(),
                                Diagnosis = reader["Diagnosis"].ToString(),
                                Treatment = reader["Treatment"].ToString()
                            };
                            medicalHistory.Add(entry);
                        }
                    }
                }
            }

            return medicalHistory;
        }

        [HttpPost]
        public IActionResult AddMedicalHistoryEntry(int clientId, MedicalHistoryEntry entry)
        {
            // Add the new medical history entry to the database
            AddMedicalHistoryEntryToDatabase(clientId, entry);

            // Redirect back to the medical history page
            return RedirectToAction("Index", new { clientId });
        }

        private void AddMedicalHistoryEntryToDatabase(int clientId, MedicalHistoryEntry entry)
        {
            // Insert the new medical history entry into the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO medical_history (ClientId, VisitDateTime, Vaccines, Diagnosis, Treatment) " +
                                    "VALUES (@ClientId, @VisitDateTime, @Vaccines, @Diagnosis, @Treatment)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@ClientId", clientId);
                    command.Parameters.AddWithValue("@VisitDateTime", entry.VisitDateTime);
                    command.Parameters.AddWithValue("@Vaccines", entry.Vaccines);
                    command.Parameters.AddWithValue("@Diagnosis", entry.Diagnosis);
                    command.Parameters.AddWithValue("@Treatment", entry.Treatment);

                    command.ExecuteNonQuery();
                }
            }
        }
        [HttpPost]
        public IActionResult DeleteMedicalHistoryEntry(int clientId, DateTime visitDateTime)
        {
            // Delete the medical history entry from the database
            DeleteMedicalHistoryEntryFromDatabase(clientId, visitDateTime);

            // Redirect back to the medical history page
            return RedirectToAction("Index", new { clientId });
        }

        private void DeleteMedicalHistoryEntryFromDatabase(int clientId, DateTime visitDateTime)
        {
            // Delete the medical history entry from the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM medical_history " +
                                     "WHERE ClientId = @ClientId AND VisitDateTime = @VisitDateTime";
                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@ClientId", clientId);
                    command.Parameters.AddWithValue("@VisitDateTime", visitDateTime);

                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
