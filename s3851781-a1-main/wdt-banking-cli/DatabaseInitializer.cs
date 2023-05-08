namespace wdt_banking_cli;
using wdt_banking_cli.Models;
using wdt_banking_cli.DTO;
using Newtonsoft.Json;
using wdt_banking_cli.DAO;

public class DatabaseInitializer
{
    const string Endpoint = "https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";
    
    private ICustomerDAO CustomerDAO = new MCBACustomerDAO();
    private IAccountDAO AccountDAO = new MCBAAccountDAO();
    private ILoginDAO LoginDAO = new MCBALoginDAO();
    private ITransactionDAO TransactionDAO = new MCBATransactionDAO();

    public async Task SeedDatabase()
    {
        var webServiceResponse = await GetWebServiceResponse();
        if (webServiceResponse == null)
        {
            Console.WriteLine("No data returned from web service");
            return;
        }

        var customers = new List<Customer>();
        var logins = new List<Login>();
        var accounts = new List<Account>();
        var transactions = new List<TransactionCreate>();

        // Traverse the response to create objects
        foreach (var customer in webServiceResponse)
        {
            customers.Add(new Customer(customer.CustomerID, customer.Name, customer.Address, customer.City, customer.PostCode));
            var login = customer.Login;
            logins.Add(new Login(login.LoginID, customer.CustomerID, login.PasswordHash));

            foreach (var account in customer.Accounts)
            {
                decimal balanceAccumulator = 0;
                foreach (var transaction in account.Transactions)
                {
                    balanceAccumulator += transaction.Amount;
                    transactions.Add(new TransactionCreate(TransactionType.D, account.AccountNumber, null, transaction.Amount, transaction.Comment, transaction.TransactionTimeUtc));
                }
                accounts.Add(new Account(account.AccountNumber, (AccountType)account.AccountType, customer.CustomerID, balanceAccumulator));
            }
        }
  
        await Task.WhenAll(customers.Select(x => CustomerDAO.CreateCustomerAsync(x)));
        await Task.WhenAll(logins.Select(x => LoginDAO.CreateLoginAsync(x)));
        await Task.WhenAll(accounts.Select(x => AccountDAO.CreateAccountAsync(x)));
        await Task.WhenAll(transactions.Select(x => TransactionDAO.CreateTransactionAsync(x)));
    }

    private async Task<List<WebServiceResponseDTO>?> GetWebServiceResponse()
    {
        var client = new HttpClient();
        var json = await client.GetStringAsync(Endpoint);
        return JsonConvert.DeserializeObject<List<WebServiceResponseDTO>>(json);
    }
}
