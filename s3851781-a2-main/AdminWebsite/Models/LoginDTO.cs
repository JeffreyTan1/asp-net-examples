using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminWebsite.Models;

public partial class LoginDTO
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None), StringLength(8, MinimumLength = 8)]
    [Display(Name = "Login ID")]
    public string LoginID { get; set; }

    [Required]
    public int CustomerID { get; set; }

    [Required]
    [StringLength(94)]
    public string PasswordHash { get; set; }

    [Required]
    public bool Blocked { get; set; }
}
