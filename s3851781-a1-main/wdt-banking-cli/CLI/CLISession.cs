namespace wdt_banking_cli.CLI;

using System.Globalization;
using wdt_banking_cli.DAO;
using wdt_banking_cli.Models;
using wdt_banking_cli.Services;
using UtilityLibraries;

public class CLISession
{
    private const int TransactionsPerPage = 4;
    private Customer LoggedInCustomer;
    private List<Account> CustomerAccount;
    private Action OnLogout;
    private Action OnExit;
    private TransactionService TransactionService = new TransactionService(new MCBATransactionDAO(), new MCBAAccountDAO());

    public CLISession(Customer customer, List<Account> account, Action onLogout, Action onExit)
    {
        LoggedInCustomer = customer;
        CustomerAccount = account;
        OnLogout = onLogout;
        OnExit = onExit;
    }

    public void RunMainMenu()
    {
        string menu =
            $"""
            --- {LoggedInCustomer.Name} ---
            [1] Deposit
            [2] Withdraw
            [3] Transfer
            [4] My Statement
            [5] Logout
            [6] Exit

            Enter an option: 
            """;

        Console.WriteLine(menu);

        bool validInput = int.TryParse(Console.ReadLine(), out int choice);
        if (!validInput)
        {
            Console.WriteLine("Choice must be a number. Please try again.");
            return;
        }

        switch (choice)
        {
            case 1:
                Deposit();
                break;
            case 2:
                Withdraw();
                break;
            case 3:
                Transfer();
                break;
            case 4:
                MyStatement();
                break;
            case 5:
                OnLogout();
                break;
            case 6:
                OnExit();
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }

    private void Deposit()
    {
        var account = ChooseAccount();
        if (account == null) { return; }
        Console.WriteLine("Enter the amount to deposit: ");
        var validInput = decimal.TryParse(Console.ReadLine(), out decimal amount);
        if (!validInput)
        {
            Console.WriteLine("Amount must be a number. Please try again.");
            return;
        }

        Console.WriteLine("Enter a comment: ");
        var comment = Console.ReadLine();

        TransactionService.Deposit(account.AccountNumber, amount, comment);
    }

    private void Withdraw()
    {
        var account = ChooseAccount();
        if (account == null) { return; }

        Console.WriteLine("Enter the amount to withdraw: ");
        var validInput = decimal.TryParse(Console.ReadLine(), out decimal amount);
        if (!validInput)
        {
            Console.WriteLine("Amount must be a number. Please try again.");
            return;
        }

        Console.WriteLine("Enter a comment: ");
        var comment = Console.ReadLine();

        TransactionService.Withdraw(account.AccountNumber, amount, comment);
    }

    private void Transfer()
    {
        var account = ChooseAccount();
        if (account == null) { return; }

        Console.WriteLine("Enter the account number to transfer to: ");
        var validInput = int.TryParse(Console.ReadLine(), out int destinationAccountNumber);
        if (!validInput)
        {
            Console.WriteLine("Account number must be a number. Please try again.");
            return;
        }

        Console.WriteLine("Enter the amount to transfer: ");
        validInput = decimal.TryParse(Console.ReadLine(), out decimal amount);
        if (!validInput)
        {
            Console.WriteLine("Amount must be a number. Please try again.");
            return;
        }

        Console.WriteLine("Enter a comment: ");
        var comment = Console.ReadLine();

        TransactionService.Transfer(account.AccountNumber, destinationAccountNumber, amount, comment);
    }

    private void MyStatement()
    {
        var account = ChooseAccount();
        if (account == null) { return; }

        var transactionHistoryRes = TransactionService.GetTransactionHistory(account.AccountNumber);
        if (transactionHistoryRes == null) { return; }

        var balance = transactionHistoryRes.Item1;
        var transactions = transactionHistoryRes.Item2;
        
        var exitTable = false;
        var pages = (int)Math.Ceiling((decimal)transactions.Count / TransactionsPerPage);
        var page = 1;
        while (!exitTable)
        {
            Console.WriteLine($"Balance: {balance.ToString("C", CultureInfo.CurrentCulture)}\n");
            Console.WriteLine($"Transaction History (Page {page})");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("| Transaction ID | Transaction Type | Account Number | Destination Account Number | Amount               | Comment              | Transaction Time    |");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------");

            var startIndex = (page - 1) * TransactionsPerPage;
            var endIndex = startIndex + TransactionsPerPage;
            if (endIndex > transactions.Count)
            {
                endIndex = transactions.Count;
            }
            var filteredTransactions = transactions.GetRange(startIndex, endIndex - startIndex);

            foreach (var transaction in filteredTransactions)
            {
                Console.WriteLine($"| {transaction.TransactionID.ToString().PadRight(14)} | {transaction.TransactionType.ToString().PadRight(16)} | {transaction.AccountNumber.ToString().PadRight(14)} | {transaction.DesinationAccountNumber.ToString().PadRight(26)} | {transaction.Amount?.ToString("C", CultureInfo.CurrentCulture).Truncate(20).PadRight(20)} | {transaction.Comment?.Truncate(20).PadRight(20) ?? "".PadRight(20)} | {transaction.TransactionTimeUtc.FormatDateTime().PadRight(19)} |");
            }

            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("[1] Previous Page");
            Console.WriteLine("[2] Next Page");
            Console.WriteLine("[3] Exit");

            var validInput = int.TryParse(Console.ReadLine(), out var choice);
            if (!validInput)
            {
                Console.WriteLine("Choice must be a number. Please try again.");
                return;
            }

            switch (choice)
            {
                case 1:
                    if (page > 1)
                    {
                        page--;
                    }
                    break;
                case 2:
                    if (page < pages)
                    {
                        page++;
                    }
                    break;
                case 3:
                    exitTable = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }

        }
    }

    private Account? ChooseAccount()
    {
        Console.WriteLine("Choose an account: ");
        for (int i = 0; i < CustomerAccount.Count; i++)
        {
            Console.WriteLine($"[{i + 1}] {CustomerAccount[i].AccountNumber}");
        }

        var validInput = int.TryParse(Console.ReadLine(), out int choice);
        if (!validInput)
        {
            Console.WriteLine("Choice must be a number. Please try again.");
            return null;
        }

        if (choice < 1 || choice > CustomerAccount.Count)
        {
            Console.WriteLine("Invalid choice. Please try again.");
            return null;
        }

        return CustomerAccount[choice - 1];
    }
}