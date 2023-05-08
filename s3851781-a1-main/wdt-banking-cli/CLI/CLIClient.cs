namespace wdt_banking_cli.CLI;
using wdt_banking_cli.Services;
using System.Text;
using wdt_banking_cli.DAO;

public class CLIClient
{
    private CLISession? activeSession = null;
    private bool CLIShouldExit = false;
    private LoginService LoginService = new (
        new MCBALoginDAO(),
        new MCBACustomerDAO(),
        new MCBAAccountDAO()
        );

    public void Run()
    {
        while (!CLIShouldExit)
        {
            if (activeSession == null)
            {
                Login();
            }
            else
            {
                // CLIShouldExit can be set to true in this method, breaking the loop
                activeSession.RunMainMenu();
            }
        }
    }

    private void SessionOnLogout()
    {
        Console.Clear();
        activeSession = null;
    }

    private void SessionOnExit()
    {
        Console.WriteLine("Program ending.");
        CLIShouldExit = true;
    }

    private void Login()
    {
        Console.Write("Enter Login ID: ");
        var loginID = Console.ReadLine();
        Console.Write("Enter Password: ");
        var password = new StringBuilder("");
        do
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
            password.Append(key.KeyChar);
            Console.Write("*");
        } while (true);
        Console.Write("\n");

        // loginID can be null if user inputs Ctrl-Z
        if (loginID == null)
        {
            Console.WriteLine("An error occured with your Login ID. Please try again.");
            return;
        }

        var response = LoginService.Login(loginID, password.ToString());
        if (response == null)
        {
            Console.WriteLine("Login failed");
        }
        else
        {
            activeSession = new CLISession(response.Item1, response.Item2, SessionOnLogout, SessionOnExit);
            Console.WriteLine("Login successful");
        }
    }
}