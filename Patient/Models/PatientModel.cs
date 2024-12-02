using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Patient.Models;

public class PatientModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? SSN { get; set; }
    [Required]
    public string? Mail { get; set; }
    [Required]
    public string? Name { get; set; }
}
