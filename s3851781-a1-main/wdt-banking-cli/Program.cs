namespace wdt_banking_cli;
using wdt_banking_cli.CLI;
using wdt_banking_cli.DAO;

public static class Program
{
    private static async Task Main()
    {
        ICustomerDAO CustomerDAO = new MCBACustomerDAO();
        var customersInDb = CustomerDAO.GetCustomerCount();
        if (customersInDb == 0)
        {
            var databaseInitializer = new DatabaseInitializer();
            await databaseInitializer.SeedDatabase();
        }

        var CLIClient = new CLIClient();
        CLIClient.Run();

        return;
    }

}
