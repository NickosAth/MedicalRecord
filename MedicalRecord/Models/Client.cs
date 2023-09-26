using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MedicalRecord.Models;

[Table("clients")]
[Index("Email", Name = "UQ__clients__AB6E6164FCDA4DE3", IsUnique = true)]
public partial class Client
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("firstName")]
    [StringLength(100)]
    public string FirstName { get; set; } 

    [Column("lastName")]
    [StringLength(100)]
    public string LastName { get; set; } 

    [Column("fathersName")]
    [StringLength(100)]
    public string FathersName { get; set; } 

    [Column("gender")]
    [StringLength(100)]
    public string Gender { get; set; } 

    [Column("dob", TypeName = "date")]
    public DateTime Dob { get; set; }

    [Column("amka")]
    [StringLength(100)]
    public string Amka { get; set; } 

    [Column("job")]
    [StringLength(100)]
    public string Job { get; set; } 

    [Column("insurance")]
    [StringLength(100)]
    public string Insurance { get; set; }

    [Column("familyStatus")]
    [StringLength(100)]
    public string FamilyStatus { get; set; } 

    [Column("phone")]
    [StringLength(150)]
    public string Phone { get; set; } 

    [Column("email")]
    [StringLength(150)]
    public string Email { get; set; } 

    [Column("areaOfResidence")]
    [StringLength(150)]
    public string AreaOfResidence { get; set; } 

    [Column("cityOfResidence")]
    [StringLength(150)]
    public string CityOfResidence { get; set; } 

    [Column("addressOfResidence")]
    [StringLength(150)]
    public string AddressOfResidence { get; set; } 

    [Column("zipCodeOfResidence")]
    [StringLength(150)]
    public string ZipCodeOfResidence { get; set; }

    public List<MedicalHistoryEntry> MedicalHistoryEntries { get; set; }

}
