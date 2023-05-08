using System.Diagnostics.CodeAnalysis;

namespace wdt_banking_cli.Models;

public class Login
{
    public required string LoginID { get; init; }
    public required int CustomerID { get; init; }
    public required string PasswordHash { get; init; }

    [SetsRequiredMembers]
    public Login(string loginID, int customerID, string passwordHash)
    {
        LoginID = loginID;
        CustomerID = customerID;
        PasswordHash = passwordHash;
    }
}