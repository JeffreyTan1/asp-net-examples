using System.Diagnostics.CodeAnalysis;

namespace wdt_banking_cli.Models;

public class Customer
{
    public required int CustomerID { get; init; }
    public required string Name { get; init; }
    public string? Address { get; }
    public string? City { get; }
    public string? PostCode { get; }

    [SetsRequiredMembers]
    public Customer(int customerID, string name, string? address, string? city, string? postCode)
    {
        CustomerID = customerID;
        Name = name;
        Address = address;
        City = city;
        PostCode = postCode;
    }
}