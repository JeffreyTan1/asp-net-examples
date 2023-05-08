using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerWebsite.Models;

public partial class Payee
{
    [Key]
    [Required]
    public int PayeeID { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Address { get; set; }

    [Required]
    [StringLength(40, MinimumLength = 1)]
    public string City { get; set; }

    [Required]
    [RegularExpression(@"^(ACT|NSW|NT|QLD|SA|TAS|VIC|WA)$", ErrorMessage = "State must be a valid Australian state")]
    [StringLength(3, MinimumLength = 2)]
    public string State { get; set; }

    [Required]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "Postcode must be in the format XXXX where X is a number between 0 and 9")]
    [StringLength(4, MinimumLength = 4)]
    public string PostCode { get; set; }

    [Required]
    [RegularExpression(@"^(\(0\d\)|0\d) \d{4} \d{4}$", ErrorMessage = "Phone number must be in the format (0X) XXXX XXXX where X is a number between 0 and 9")]
    [StringLength(14, MinimumLength = 14)]
    
    public string Phone { get; set; }
}
