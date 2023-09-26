using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MedicalRecord.Models;

namespace MedicalRecord.Controllers
{
    public class LoginController : Controller
    {
        private readonly string _connectionString = "Data Source=DESKTOP-M5LLFFV;Initial Catalog=WPF;Integrated Security=True";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginPost(DoctorLogin doctorLogin)
        {
            int loggedInDoctorId = AuthenticateDoctor(doctorLogin);

            if (loggedInDoctorId != -1)
            {
                // Set a cookie with the doctor's ID
                Response.Cookies.Append("LoggedInDoctorId", loggedInDoctorId.ToString());

                return RedirectToAction("Index", "MainMenu");
            }
            else
            {
                ViewBag.ErrorMessage = "Λανθασμένα Στοιχεία Χρήστη!";
                return View("Index");
            }
        }

        private int AuthenticateDoctor(DoctorLogin doctorLogin)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT Id FROM doctors WHERE username=@Username AND password=@Password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", doctorLogin.Username);
                    command.Parameters.AddWithValue("@Password", doctorLogin.Password);

                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return (int)result;
                    }
                }
            }

            return -1; // Authentication failed
        }

        [HttpGet]
        public IActionResult PasswordReset(string email, string token)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(token))
            {
                if (VerifyResetToken(email, token))
                {
                    var resetRequest = new DoctorPasswordResetRequest { Token = token, Email = email };
                    return View("ResetPassword", resetRequest);
                }
                else
                {
                    TempData["ResetErrorMessage"] = "Μη έγκυρος ή ληγμένος σύνδεσμος. Παρακαλώ ζητήστε έναν νέο σύνδεσμο επαναφοράς κωδικού.";
                    return RedirectToAction("Index");
                }
            }

            TempData["ResetErrorMessage"] = "Λείπει το email ή το token.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult PasswordReset(DoctorPasswordResetRequest resetRequest)
        {
            // Verify the token and check if the new password matches the confirm password
            if (VerifyResetToken(resetRequest.Email, resetRequest.Token) &&
                resetRequest.NewPassword == resetRequest.ConfirmPassword)
            {
                if (UpdateDoctorPassword(resetRequest.Email, resetRequest.NewPassword))
                {
                    TempData["ResetSuccessMessage"] = "Ο κωδικός αλλάχθηκε επιτυχώς."; // Success message
                    return RedirectToAction("Index"); // Redirect to a suitable page
                }
                else
                {
                    ViewBag.ResetErrorMessage = "Σφάλμα κατά την ενημέρωση του κωδικού. Παρακαλώ δοκιμάστε ξανά."; // Error message
                }
            }
            else
            {
                ViewBag.ResetErrorMessage = "Ο κωδικός δεν ταιριάζει ή το token δεν είναι έγκυρο. Παρακαλώ δοκιμάστε ξανά."; // Error message
            }

            // Redirect back to the "Set New Password" view with email and token
            return RedirectToAction("PasswordReset", new { email = resetRequest.Email, token = resetRequest.Token });
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(DoctorPasswordResetRequest resetRequest)
        {
            if (EmailExistsInDatabase(resetRequest.Email))
            {
                string resetToken = Guid.NewGuid().ToString();

                DateTime createdAt = DateTime.Now;
                if (StoreResetTokenInDatabase(resetRequest.Email, resetToken, createdAt))
                {
                    if (await SendPasswordResetEmailAsync(resetRequest.Email, resetToken))
                    {
                        TempData["ResetSuccessMessage"] = "Το email για επαναφορά του κωδικού στάλθηκε επιτυχώς.";
                    }
                    else
                    {
                        TempData["ResetFailureMessage"] = "Σφάλμα κατά την αποστολή email. Παρακαλώ δοκιμάστε ξανά.";
                    }
                }
                else
                {
                    TempData["ResetFailureMessage"] = "Σφάλμα κατά την αποθήκευση του token.";
                }
            }

            return RedirectToAction("Index");
        }



        private bool EmailExistsInDatabase(string email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM doctors WHERE email=@Email";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        int count = (int)command.ExecuteScalar();

                        // Return true if the email exists
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking email existence: {ex.Message}");
                ViewBag.ResetErrorMessage = "Σφάλμα κατά τον έλεγχο της ύπαρξης του email.";
                return false;
            }
        }

        private bool StoreResetTokenInDatabase(string email, string resetToken, DateTime createdAt)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO PasswordResetTokens (Email, Token, CreatedAt) VALUES (@Email, @Token, @CreatedAt)";
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Token", resetToken);
                        command.Parameters.AddWithValue("@CreatedAt", createdAt); // Include the CreatedAt value

                        int rowsAffected = command.ExecuteNonQuery();

                        // Return true if the token is stored successfully
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error storing reset token in the database: {ex.Message}");
                ViewBag.ResetErrorMessage = "Σφάλμα κατά την αποθήκευση του token. Σφάλμα βάσης δεδομένων.";
                return false;
            }
        }

        private async Task<bool> SendPasswordResetEmailAsync(string recipientEmail, string resetToken)
        {
            try
            {
                var fromAddress = new MailAddress("medicalrecord194@gmail.com", "MedicalRecord");
                var toAddress = new MailAddress(recipientEmail); // Use the recipient's email address
                const string fromPassword = "joyhiktnelgpuxyr"; // Replace with your email password
                const string subject = "Επαναφορά Κωδικού";
                string resetUrl = Url.Action("PasswordReset", "Login", new { email = recipientEmail, token = resetToken }, Request.Scheme);
                string body = $"<a href=\"{resetUrl}\">Κάντε κλικ εδώ για να επαναφέρετε τον κωδικό πρόσβασής σας</a>";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com", // Update with your SMTP server details
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                await smtp.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending password reset email: {ex.Message}");
                return false;
            }
        }

        private bool VerifyResetToken(string email, string token)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Verify that the token exists and is not expired (within 1 hour)
                    string query = "SELECT COUNT(*) FROM PasswordResetTokens WHERE Email=@Email AND Token=@Token AND CreatedAt >= DATEADD(HOUR, -1, GETDATE())";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Token", token);

                        int count = (int)command.ExecuteScalar();

                        // Return true if the token is valid and not expired
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying reset token: {ex.Message}");
                return false;
            }
        }

        private bool UpdateDoctorPassword(string email, string newPassword)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE doctors SET password=@NewPassword WHERE email=@Email";
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@NewPassword", newPassword);

                        int rowsAffected = command.ExecuteNonQuery();

                        // Return true if the password is updated successfully
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating doctor password: {ex.Message}");
                return false;
            }
        }
        [HttpPost]
        public IActionResult CheckEmailExists(string email)
        {
            bool emailExists = EmailExists(email);
            return Json(new { exists = emailExists });
        }

        private bool EmailExists(string email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string checkEmailQuery = "SELECT COUNT(*) FROM doctors WHERE email=@Email";
                    using (SqlCommand command = new SqlCommand(checkEmailQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        int count = (int)command.ExecuteScalar();

                        // Return true if the email exists (count > 0)
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking email existence: {ex.Message}");
                return false;
            }
        }

    }
}
