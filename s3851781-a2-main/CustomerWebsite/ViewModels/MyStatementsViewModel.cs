using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerWebsite.Models;

public class MyStatementsViewModel
{
    public IEnumerable<Transaction> Transactions { get; set; }
    public decimal Balance { get; set; }
    public int AccountNumber { get; set; }

}
