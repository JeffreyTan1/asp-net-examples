namespace wdt_banking_cli.Services;
using wdt_banking_cli.Models;
using SimpleHashing.Net;
using wdt_banking_cli.DAO;

public class LoginService
{
    private ILoginDAO LoginDAO;
    private ICustomerDAO CustomerDAO;
    private IAccountDAO AccountDAO;
    
    public LoginService(ILoginDAO LoginDAO, ICustomerDAO CustomerDAO, IAccountDAO AccountDAO)
    {
        this.LoginDAO = LoginDAO;
        this.CustomerDAO = CustomerDAO;
        this.AccountDAO = AccountDAO;
    }

    public Tuple<Customer, List<Account>>? Login(string loginID, string password)
    {
        var login = LoginDAO.GetLogin(loginID);
        if (login == null)
        {
            return null;
        }

        var passwordMatchesHash = new SimpleHash().Verify(password, login.PasswordHash);
        if (!passwordMatchesHash)
        {
            return null;
        }

        var customer = CustomerDAO.GetCustomerByCustomerID(login.CustomerID);
        if (customer == null)
        {
            return null;
        }

        var account = AccountDAO.GetAccountsByCustomerID(customer.CustomerID);

        return Tuple.Create(customer, account);
    }

};