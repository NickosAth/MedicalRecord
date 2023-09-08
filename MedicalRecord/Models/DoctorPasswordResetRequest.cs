using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalRecord.Models
{
    public class DoctorPasswordResetRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; } 
        public DateTime CreatedAt { get; set; } 
    }
}
