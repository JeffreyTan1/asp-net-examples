namespace wdt_banking_cli.DAO;
using wdt_banking_cli.Models;
    
public interface IAccountDAO
{
    Task CreateAccountAsync(Account account);
    Account? GetAccountByAccountNumber(int accountNumber);
    List<Account> GetAccountsByCustomerID(int customerID);

    void UpdateAccount(Account account);
}


public interface ICustomerDAO
{
    int GetCustomerCount();
    Task CreateCustomerAsync(Customer customer);
    Customer? GetCustomerByCustomerID(int customerID);
}

public interface ILoginDAO
{
    Login? GetLogin(string loginID);
    Task CreateLoginAsync(Login login);

}

public interface ITransactionDAO
{
    Task CreateTransactionAsync(TransactionCreate transaction);
    List<Transaction> GetTransactionsByAccountNumber(int accountNumber);
}