using MedicalRecord.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;


namespace MedicalRecord.Controllers
{
    public class ShowClientsController : Controller
    {
        string connectionString = "Data Source=DESKTOP-M5LLFFV;Initial Catalog=WPF;Integrated Security=True";

        [HttpGet]
        [Route("ShowClients")]
        public IActionResult ShowClients(string searchString)
        {
            List<Client> clients = new List<Client>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM clients";
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Client client = new Client
                        {
                            Id = (int)reader["id"],
                            FirstName = reader["firstName"].ToString(),
                            LastName = reader["lastName"].ToString(),
                            FathersName = reader["fathersName"].ToString(),
                            Gender = reader["gender"].ToString(),
                            Dob = (DateTime)reader["dob"],
                            Amka = reader["amka"].ToString(),
                            Job = reader["job"].ToString(),
                            Insurance = reader["insurance"].ToString(),
                            FamilyStatus = reader["familyStatus"].ToString(),
                            Phone = reader["phone"].ToString(),
                            Email = reader["email"].ToString(),
                            AreaOfResidence = reader["areaOfResidence"].ToString(),
                            CityOfResidence = reader["cityOfResidence"].ToString(),
                            AddressOfResidence = reader["addressOfResidence"].ToString(),
                            ZipCodeOfResidence = reader["zipCodeOfResidence"].ToString()
                        };

                        clients.Add(client);
                    }
                }
            }
            // Apply search filter if searchString is provided
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower(); // Convert to lowercase for case-insensitive search
                clients = clients.Where(c =>
                    c.FirstName.ToLower().Contains(searchString) ||
                    c.LastName.ToLower().Contains(searchString) ||
                    c.Phone.ToLower().Contains(searchString) ||
                    c.Amka.ToLower().Contains(searchString)
                ).ToList();
            }


            return View("~/Views/ShowClients/Index.cshtml", clients);
        }


        [HttpGet]
        [Route("EditClient/{id}")]
        public IActionResult EditClient(int id)
        {
            // Retrieve the client from the database based on the ID
            Client client = GetClientById(id);

            if (client == null)
            {
                // Client not found, handle the error accordingly
                return RedirectToAction("ShowClients");
            }

            return View("~/Views/EditClient/Index.cshtml", client);
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "DELETE FROM Clients WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("ShowClients");
        }

        private Client GetClientById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Clients WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Client client = new Client();
                            client.Id = (int)reader["id"];
                            client.FirstName = reader["firstName"].ToString();
                            client.LastName = reader["lastName"].ToString();
                            client.FathersName = reader["fathersName"].ToString();
                            client.Gender = reader["gender"].ToString();
                            client.Dob = (DateTime)reader["dob"];
                            client.Amka = reader["amka"].ToString();
                            client.Job = reader["job"].ToString();
                            client.Insurance = reader["insurance"].ToString();
                            client.FamilyStatus = reader["familyStatus"].ToString();
                            client.Phone = reader["phone"].ToString();
                            client.Email = reader["email"].ToString();
                            client.AreaOfResidence = reader["areaOfResidence"].ToString();
                            client.CityOfResidence = reader["cityOfResidence"].ToString();
                            client.AddressOfResidence = reader["addressOfResidence"].ToString();
                            client.ZipCodeOfResidence = reader["zipCodeOfResidence"].ToString();

                            return client;
                        }
                    }
                }
            }

            return null; // Client with the given ID not found
        }
    }
}