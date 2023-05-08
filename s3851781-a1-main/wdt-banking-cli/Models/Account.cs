using System.Diagnostics.CodeAnalysis;

namespace wdt_banking_cli.Models;

public enum AccountType {
    S = 'S',
    C = 'C'
}

public class Account
{
    public required int AccountNumber { get; init;  }
    public required AccountType AccountType { get; init; }
    public required int CustomerID { get; init; }
    public required decimal Balance { get; init; }

    [SetsRequiredMembers]
    public Account(int accountNumber, AccountType accountType, int customerID, decimal balance)
    {
        AccountNumber = accountNumber;
        AccountType = accountType;
        CustomerID = customerID;
        Balance = balance;
    }
}