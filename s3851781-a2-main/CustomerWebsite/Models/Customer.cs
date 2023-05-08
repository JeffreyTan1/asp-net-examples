using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerWebsite.Models;

public partial class Customer
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None), StringLength(4, MinimumLength = 4)]
    public int CustomerID { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; }

    [StringLength(11, MinimumLength = 11)]
    [RegularExpression(@"^\d{3} \d{3} \d{3}$", ErrorMessage = "TFN must be in the format XXX XXX XXX where X is a number between 0 and 9")]
    public string? TFN { get; set; }

    [StringLength(50, MinimumLength = 1)]
    public string? Address { get; set; }

    [StringLength(40, MinimumLength = 1)]
    public string? City { get; set; }

    [StringLength(3, MinimumLength = 2)]
    [RegularExpression(@"^(ACT|NSW|NT|QLD|SA|TAS|VIC|WA)$", ErrorMessage = "State must be a valid Australian state")]
    public string? State { get; set; }

    [StringLength(4, MinimumLength = 4)]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "Postcode must be in the format XXXX where X is a number between 0 and 9")]
    public string? PostCode { get; set; }

    [StringLength(12, MinimumLength = 12)]
    [RegularExpression(@"^04\d{2} \d{3} \d{3}$", ErrorMessage = "Phone number must be in the format 04XX XXX XXX where X is a number between 0 and 9")]
    public string? Mobile { get; set; }

    public string? ImageBase64 { get; set; }

    [InverseProperty(nameof(Account.Customer))]
    public ICollection<Account> Accounts { get; } = new List<Account>();

    [InverseProperty(nameof(Login.Customer))]
    public ICollection<Login> Logins { get; } = new List<Login>();
}