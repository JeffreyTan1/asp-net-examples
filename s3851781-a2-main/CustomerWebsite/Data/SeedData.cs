using CustomerWebsite.Data;
using CustomerWebsite.Models;
using CustomerWebsite.Models.Types;
using Newtonsoft.Json;

namespace CustomerWebsite.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<McbaContext>();

        // Look for customers.
        if (context.Customer.Any())
            return; // DB has already been seeded.

        
        var AppName = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ApiEndpoints")["CustomersWebServiceEndpoint"];
        var client = new HttpClient();
        var response = client.GetStringAsync(AppName).Result;
        var data = JsonConvert.DeserializeObject<List<WebServiceResponseDTO>>(response);

        var customers = new List<Customer>();
        var logins = new List<Login>();
        var accounts = new List<Account>();
        var transactions = new List<Transaction>();

        // Traverse the response to create objects
        foreach (var item in data)
        {
            customers.Add(
                new Customer 
                { 
                    CustomerID = item.CustomerID, 
                    Name = item.Name, 
                    Address = item.Address, 
                    City = item.City, 
                    PostCode = item.PostCode 
                }
            );
            var login = item.Login;
            logins.Add(
                new Login { 
                    LoginID = login.LoginID, 
                    CustomerID = item.CustomerID, 
                    PasswordHash = login.PasswordHash,
                    Blocked = false
                }
             );

            foreach (var account in item.Accounts)
            {
                foreach (var transaction in account.Transactions)
                {
                    transactions.Add(new Transaction
                        {
                            TransactionType = TransactionType.Deposit,
                            Amount = transaction.Amount,
                            Comment = transaction.Comment,
                            TransactionTimeUtc = transaction.TransactionTimeUtc,
                            AccountNumber = account.AccountNumber
                        }
                    );
                }
                
                accounts.Add(new Account
                    {
                        AccountNumber = account.AccountNumber,
                        CustomerID = item.CustomerID,
                        AccountType = account.AccountType,
                    }
                );
            }
        }

        context.Customer.AddRange(customers);
        context.Login.AddRange(logins);
        context.Account.AddRange(accounts);
        context.Transaction.AddRange(transactions);

        context.SaveChanges();
    }
}
