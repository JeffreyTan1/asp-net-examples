namespace wdt_banking_cli.DTO;

public class WebServiceResponseDTO
{
    public int CustomerID { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string PostCode { get; set; }
    public List<AccountResponse> Accounts { get; set; }
    public LoginResponse Login { get; set; }
}

public class AccountResponse
{
    public int AccountNumber { get; set; }
    public char AccountType { get; set; }
    public int CustomerID { get; set; }
    public List<TransactionResponse> Transactions { get; set; }
}

public class TransactionResponse
{
    public decimal Amount { get; set; }
    public string Comment { get; set; }
    public DateTime TransactionTimeUtc { get; set; }
}

public class LoginResponse
{
    public string LoginID { get; set; }
    public string PasswordHash { get; set; }
}