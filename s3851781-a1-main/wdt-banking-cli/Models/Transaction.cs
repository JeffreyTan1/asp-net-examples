using System.Diagnostics.CodeAnalysis;

namespace wdt_banking_cli.Models;

public enum TransactionType
{
    D = 'D',
    W = 'W',
    T = 'T',
    S = 'S'
};

public class TransactionCreate
{
    public required TransactionType TransactionType { get; init; }
    public required int AccountNumber { get; init; }
    public int? DesinationAccountNumber { get; }
    public decimal? Amount { get; }
    public string? Comment { get; }
    public required DateTime TransactionTimeUtc { get; init; }

    [SetsRequiredMembers]
    public TransactionCreate(TransactionType transactionType, int accountNumber, int? desinationAccountNumber, decimal? amount, string? comment, DateTime transactionTimeUtc)
    {
        TransactionType = transactionType;
        AccountNumber = accountNumber;
        DesinationAccountNumber = desinationAccountNumber;
        Amount = amount;
        Comment = comment;
        TransactionTimeUtc = transactionTimeUtc;
    }
}

public class Transaction : TransactionCreate
{
    
    public int TransactionID { get; }

    [SetsRequiredMembers]
    public Transaction(int transactionID, TransactionType transactionType, int accountNumber, int? desinationAccountNumber, decimal? amount, string? comment, DateTime transactionTimeUtc) : base(transactionType, accountNumber, desinationAccountNumber, amount, comment, transactionTimeUtc)
    {
        TransactionID = transactionID;
    }
}