using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerWebsite.Models;

public class CreateBillPayViewModel
{
    [Required]
    public int AccountNumber { get; set; }
    
    [Required]
    public int PayeeID { get; set; }

    [Required]
    [Column(TypeName = "money")]
    [Range(0.01, 9999999999999999.99, ErrorMessage = "Amount must be between 0.01 and 9999999999999999.99")]
    public decimal Amount { get; set; }

    [Required]
    public DateTime ScheduleTimeUtc { get; set; }

    [Required]
    [StringLength(1, MinimumLength = 1)]
    [RegularExpression(@"^(O|M)$", ErrorMessage = "Period must be either One-off or Monthly")]
    public string Period { get; set; }

}
