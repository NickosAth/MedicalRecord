using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using MedicalRecord.Models;

namespace MedicalRecord.Controllers
{
    public class MedicalHistoryController : Controller
    {
        string connectionString = "Data Source=DESKTOP-M5LLFFV;Initial Catalog=WPF;Integrated Security=True";

        public IActionResult Index(int clientId)
        {
            MedicalHistoryModel medicalHistory = GetMedicalHistoryFromDatabase(clientId);

            return View(medicalHistory);
        }

        private MedicalHistoryModel GetMedicalHistoryFromDatabase(int clientId)
        {
            MedicalHistoryModel medicalHistory = new MedicalHistoryModel();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Vaccines, Diagnosis, Treatment, VisitDateTime " +
                               "FROM clients " +
                               "WHERE id = @ClientId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClientId", clientId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            medicalHistory.Vaccines = reader["Vaccines"].ToString();
                            medicalHistory.Diagnosis = reader["Diagnosis"].ToString();
                            medicalHistory.Treatment = reader["Treatment"].ToString();
                            medicalHistory.VisitDateTime = (DateTime)reader["VisitDateTime"];
                        }
                    }
                }
            }

            return medicalHistory;
        }
    }
}
