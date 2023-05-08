using System.ComponentModel.DataAnnotations;

namespace CustomerWebsite.Models;

public class UpdateCustomerViewModel
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [RegularExpression(@"^\d{3} \d{3} \d{3}$", ErrorMessage = "TFN must be in the format XXX XXX XXX where X is a number between 0 and 9")]
    [StringLength(11, MinimumLength = 11)]
    public string? TFN { get; set; }

    [StringLength(50, MinimumLength = 1)]
    public string? Address { get; set; }

    [StringLength(40, MinimumLength = 1)]
    public string? City { get; set; }

    [StringLength(3, MinimumLength = 2)]
    [RegularExpression(@"^(ACT|NSW|NT|QLD|SA|TAS|VIC|WA)$", ErrorMessage = "State must be a valid Australian state")]
    public string? State { get; set; }

    [RegularExpression(@"^\d{4}$", ErrorMessage = "Postcode must be in the format XXXX where X is a number between 0 and 9")]
    [StringLength(4, MinimumLength = 4)]
    public string? PostCode { get; set; }

    [RegularExpression(@"^04\d{2} \d{3} \d{3}$", ErrorMessage = "Phone number must be in the format 04XX XXX XXX where X is a number between 0 and 9")]
    [StringLength(12, MinimumLength = 12)]
    public string? Mobile { get; set; }
}
