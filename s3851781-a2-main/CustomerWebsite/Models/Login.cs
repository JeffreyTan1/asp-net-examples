using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerWebsite.Models;

public partial class Login
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None), StringLength(8, MinimumLength = 8)]
    [Display(Name = "Login ID")]
    public string LoginID { get; set; }

    [Required]
    [ForeignKey(nameof(Customer))]
    public int CustomerID { get; set; }
    public Customer Customer { get; set; }

    [Required]
    [StringLength(94)]
    public string PasswordHash { get; set; }

    [Required]
    public bool Blocked { get; set; }
}
