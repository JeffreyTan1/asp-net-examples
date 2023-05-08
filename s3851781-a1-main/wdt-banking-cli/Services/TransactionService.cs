namespace wdt_banking_cli.Services;
using wdt_banking_cli.Models;
using wdt_banking_cli.DAO;

public class TransactionService
{
    const int FreeTransactionLimit = 2;
    const decimal WithdrawServiceCharge = 0.05M;
    const decimal TransferServiceCharge = 0.10M;
    const decimal TransactionsMinimum = 0M;
    const decimal SavingsBalanceMinimum = 0M;
    const decimal CheckingBalanceMinimum = 300M;
    const decimal MoneyDataLimit = 922_337_203_685_477.5807M;

    private ITransactionDAO TransactionDAO;
    private IAccountDAO AccountDAO;

    public TransactionService(ITransactionDAO TransactionDAO, IAccountDAO accountDAO)
    {
        this.TransactionDAO = TransactionDAO;
        AccountDAO = accountDAO;
    }

    public void Deposit(int accountNumber, decimal amount, string? comment)
    {
        if (!ValidAmount(amount)) { return; }
        var account = GetAccount(accountNumber);
        if (account == null) { return; }

        TransactionDAO.CreateTransactionAsync(new TransactionCreate
        (TransactionType.D, account.AccountNumber, null, amount, comment, DateTime.UtcNow));
        ModifyAccountBalance(account, amount);
    }

    public void Withdraw(int accountNumber, decimal amount, string? comment)
    {
        if (!ValidAmount(amount) ) { return; }
        var account = GetAccount(accountNumber);
        if (account == null) { return; };
        if (!AccountCanSubtractAmount(account, amount)) { return; };
   
        TransactionDAO.CreateTransactionAsync(new TransactionCreate
        (TransactionType.W, account.AccountNumber, null, amount, comment, DateTime.UtcNow));
        var updatedAccount = ModifyAccountBalance(account, -amount);
        ApplyServiceCharge(updatedAccount, WithdrawServiceCharge);
    }

    public void Transfer(int accountNumber, int destinationAccountNumber, decimal amount, string? comment)
    {
        if (!ValidAmount(amount)) { return; }
        if (accountNumber == destinationAccountNumber)
        {
            Console.WriteLine("Cannot transfer to the same account.");
            return;
        }
        var account = GetAccount(accountNumber);
        var destinationAccount = GetAccount(destinationAccountNumber);
        if (account == null || destinationAccount == null) { return; };
        if (!AccountCanSubtractAmount(account, amount)) { return; };

        TransactionDAO.CreateTransactionAsync(new TransactionCreate
        (TransactionType.T, account.AccountNumber, destinationAccount.AccountNumber, amount, comment, DateTime.UtcNow));
        TransactionDAO.CreateTransactionAsync(new TransactionCreate
        (TransactionType.T, destinationAccount.AccountNumber, null, amount, comment, DateTime.UtcNow));

        var updatedAccount = ModifyAccountBalance(account, -amount);
        ApplyServiceCharge(updatedAccount, TransferServiceCharge);

        ModifyAccountBalance(destinationAccount, amount);
    }

    public Tuple<decimal, List<Transaction>>? GetTransactionHistory(int accountNumber)
    {
        var transactions = TransactionDAO.GetTransactionsByAccountNumber(accountNumber);
        var account = AccountDAO.GetAccountByAccountNumber(accountNumber);
        if (account == null) { return null; }

        return Tuple.Create(account.Balance, transactions);
    }

    // Methods to update account balances
    private Account ModifyAccountBalance(Account account, decimal amountDelta)
    {
        var newBalance = account.Balance + amountDelta;
        var updatedAccount = new Account(account.AccountNumber, account.AccountType, account.CustomerID, newBalance);
        AccountDAO.UpdateAccount(
            updatedAccount
        );
        return updatedAccount;
    }

    private void ApplyServiceCharge(Account account, decimal amount)
    {
        var accountHasFreeTransactions = HasFreeTransactions(account);
        if (accountHasFreeTransactions) { return; }

        TransactionDAO.CreateTransactionAsync(new TransactionCreate
        (TransactionType.S, account.AccountNumber, null, amount, null, DateTime.UtcNow));
        ModifyAccountBalance(account, -amount);
    }

    // Methods for validation
    private Account? GetAccount(int accountNumber)
    {
        var account = AccountDAO.GetAccountByAccountNumber(accountNumber);
        if (account == null)
        {
            Console.WriteLine($"{accountNumber} is not a valid account number");
        }
        return account;
    }

    private bool ValidAmount(decimal amount)
    {
        var isMoreThanMinimum = amount > TransactionsMinimum;
        if (!isMoreThanMinimum)
        {
            Console.WriteLine($"Amount must be greater than {TransactionsMinimum}");
            return false;
        }

        var isWithinMoneyRange = amount >= -MoneyDataLimit && amount <= MoneyDataLimit;
        if (!isWithinMoneyRange)
        {
            Console.WriteLine($"Amount must be within -{MoneyDataLimit} and {MoneyDataLimit}");
            return false;
        }
        return true;
    }

    private bool AccountCanSubtractAmount(Account account, decimal amount)
    {
        var balance = account.Balance;
        var accountType = account.AccountType;
        var res = false;

        if (accountType == AccountType.S)
        {
            res = balance - amount >= SavingsBalanceMinimum;
        } else if (accountType == AccountType.C)
        {
            res = balance - amount >= CheckingBalanceMinimum;
        }

        if (!res)
        {
            Console.WriteLine("Insufficient funds to perform this operation.");
        }
        
        return res;
    }

    private bool HasFreeTransactions(Account account)
    {
        var transactions = TransactionDAO.GetTransactionsByAccountNumber(account.AccountNumber);
        if (transactions == null) { return false; }

        var withdrawOrTransferTransactions = transactions.Where(x => (x.TransactionType == TransactionType.W || x.TransactionType == TransactionType.T)).ToList();
        if (withdrawOrTransferTransactions.Count <= FreeTransactionLimit) { return true; }
        return false;
    }
};