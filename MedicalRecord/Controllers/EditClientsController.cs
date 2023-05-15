using MedicalRecord.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace MedicalRecord.Controllers
{
    public class EditClientsController : Controller
    {
        string connectionString = "Data Source=DESKTOP-M5LLFFV;Initial Catalog=WPF;Integrated Security=True";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Client client)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE clients SET firstName = @FirstName, lastName = @LastName, fathersName = @FatherName, gender = @Gender, dob = @Dob, amka = @Amka, job = @Job, insurance = @Insurance, familyStatus = @FamilyStatus, phone = @Phone, email = @Email, areaOfResidence = @AreaOfResidence, cityOfResidence = @CityOfResidence, addressOfResidence = @AddressOfResidence, zipCodeOfResidence = @ZipCodeOfResidence " +
                               "WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", client.Id);
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
