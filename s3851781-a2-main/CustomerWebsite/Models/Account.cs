using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CustomerWebsite.Models.Types;
namespace CustomerWebsite.Models;

public partial class Account
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [StringLength(4, MinimumLength = 4)]
    [Display(Name = "Account Number")]
    public int AccountNumber { get; set; }

    [Required]
    [StringLength(1, MinimumLength = 1)]
    [RegularExpression(@"^(C|S)$", ErrorMessage = "Account type must be either Checkings or Savings")]
    [Display(Name = "Account Type")]
    public string AccountType { get; set; }

    [ForeignKey(nameof(Customer))]
    [Required]
    public int CustomerID { get; set; }
    public Customer Customer { get; set; }

    [InverseProperty(nameof(Transaction.Account))]
    public ICollection<Transaction> AccountTransactions { get; } = new List<Transaction>();
    
    [InverseProperty(nameof(Transaction.DestinationAccount))]
    public ICollection<Transaction> DestinationAccountTransactions { get; } = new List<Transaction>();
    
    public decimal GetBalance()
    {
        decimal balance = 0;
        foreach (var transaction in AccountTransactions)
        {
            switch (transaction.TransactionType)
            {
                case TransactionType.Deposit:
                    balance += transaction.Amount;
                    break;
                case TransactionType.Withdraw:
                    balance -= transaction.Amount;
                    break;
                case TransactionType.BillPay:
                    balance -= transaction.Amount;
                    break;
                case TransactionType.Transfer:
                    if (transaction.DestinationAccountNumber == null)
                    {
                        balance += transaction.Amount;
                    }
                    else
                    {
                        balance -= transaction.Amount;
                    }
                    break;
            }
        }
        return balance;
    }
    public string GetBalanceString()
    {
        var balance = GetBalance();
        return balance.ToString("C");
    }
}