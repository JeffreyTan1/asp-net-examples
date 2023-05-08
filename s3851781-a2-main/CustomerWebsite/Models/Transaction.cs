using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerWebsite.Models;

public partial class Transaction
{
    [Key]
    [Required]
    public int TransactionID { get; set; }

    [Required]
    [StringLength(1, MinimumLength = 1)]
    [RegularExpression(@"^(D|W|T|S)$", ErrorMessage = "Transaction type must be either Deposit, Withdraw, Transfer or ServiceCharge")]
    public string TransactionType { get; set; }

    [Required]
    [ForeignKey(nameof(Account))]
    public int AccountNumber { get; set; }
    public Account Account { get; set; }

    [ForeignKey(nameof(DestinationAccount))]
    public int? DestinationAccountNumber { get; set; }
    public Account? DestinationAccount { get; set; }

    [Required]
    [Column(TypeName = "money")]
    [Range(0.01, 9999999999999999.99, ErrorMessage = "Amount must be between 0.01 and 9999999999999999.99")]
    public decimal Amount { get; set; }

    [StringLength(30)]
    public string? Comment { get; set; }

    [Required]
    public DateTime TransactionTimeUtc { get; set; }
}
