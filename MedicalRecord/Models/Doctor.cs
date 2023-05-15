using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MedicalRecord.Models;

[Keyless]
[Table("doctor")]
public partial class Doctor
{
    [Column("firstName")]
    public string FirstName { get; set; }

    [Column("lastName")]
    public string LastName { get; set; }

    [Column("username")]
    public string Username { get; set; }

    [Column("password")]
    public string Password { get; set; }

    [Column("gender")]
    public string Gender { get; set; }

    [Column("phone")]
    public string Phone { get; set; }


    [Column("email")]
    public string Email { get; set; }


}



