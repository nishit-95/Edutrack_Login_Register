using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Edutrack.Models;

public class t_Register
{
    // [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    // public int C_UserId { get; set; }

    // [required]
    public string C_UserName { get; set; }

    // [Required]
    public string C_Password { get; set; }

    // [Required]
    public string C_UserEmail { get; set; }
}