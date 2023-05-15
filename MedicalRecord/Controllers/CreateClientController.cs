using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using MedicalRecord.Models;
using System.Data.SqlTypes;

namespace MedicalRecord.Controllers
{
    public class CreateClientController : Controller
    {
        string connectionString = "Data Source=DESKTOP-M5LLFFV;Initial Catalog=WPF;Integrated Security=True";

        public IActionResult Index()
        {
            return View(new Client());
        }

        [HttpPost]
        public ActionResult Create(Client client)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO clients (firstName, lastName, fathersName, gender, dob, amka, job, insurance, familyStatus, phone, email, areaOfResidence, cityOfResidence, addressOfResidence, zipCodeOfResidence) " +
                               "VALUES (@FirstName, @LastName, @FatherName, @Gender, @Dob, @Amka, @Job, @Insurance, @FamilyStatus, @Phone, @Email, @AreaOfResidence, @CityOfResidence, @AddressOfResidence, @ZipCodeOfResidence)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", client.FirstName);
                    command.Parameters.AddWithValue("@LastName", client.LastName);
                    command.Parameters.AddWithValue("@FatherName", client.FathersName);
                    command.Parameters.AddWithValue("@Gender", client.Gender);
                    command.Parameters.AddWithValue("@Dob", client.Dob);
                    command.Parameters.AddWithValue("@Amka", client.Amka);
                    command.Parameters.AddWithValue("@Job", client.Job);
                    command.Parameters.AddWithValue("@Insurance", client.Insurance);
                    command.Parameters.AddWithValue("@FamilyStatus", client.FamilyStatus);
                    command.Parameters.AddWithValue("@Phone", client.Phone);
                    command.Parameters.AddWithValue("@Email", client.Email);
                    command.Parameters.AddWithValue("@AreaOfResidence", client.AreaOfResidence);
                    command.Parameters.AddWithValue("@CityOfResidence", client.CityOfResidence);
                    command.Parameters.AddWithValue("@AddressOfResidence", client.AddressOfResidence);
                    command.Parameters.AddWithValue("@ZipCodeOfResidence", client.ZipCodeOfResidence);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index", "ShowClients");
        }
    }
}
