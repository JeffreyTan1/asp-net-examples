namespace CustomerWebsite.Services;
using CustomerWebsite.Models;
using CustomerWebsite.Data;
using Microsoft.EntityFrameworkCore;
using CustomerWebsite.Models.Types;

public class McbaTransactionService
{
    const int FreeTransactionLimit = 2;
    const decimal WithdrawServiceCharge = 0.05M;
    const decimal TransferServiceCharge = 0.10M;
    const decimal TransactionsMinimum = 0M;
    const decimal SavingsBalanceMinimum = 0M;
    const decimal CheckingBalanceMinimum = 300M;
    const decimal MoneyDataLimit = 922_337_203_685_477.5807M;

    private readonly McbaContext _context;

    public McbaTransactionService(McbaContext _context)
    {
        this._context = _context;
    }
    
    public async Task<string?> Deposit(Account account, decimal amount, string? comment)
    {
        var invalidAmountError = GetInvalidInputError(amount, comment);
        if (invalidAmountError != null) { return invalidAmountError; }

        _context.Transaction.Add(
            new Transaction
            {
                AccountNumber = account.AccountNumber,
                Amount = amount,
                Comment = comment,
                TransactionType = TransactionType.Deposit,
                TransactionTimeUtc = DateTime.UtcNow
            }
        );

        await _context.SaveChangesAsync();
        return null;
    }

    public async Task<string?> Withdraw(Account account, decimal amount, string? comment)
    {
        var invalidAmountError = GetInvalidInputError(amount, comment);
        if (invalidAmountError != null) { return invalidAmountError; }
        var insufficientFundsError = GetInsufficientFundsError(account, amount);
        if (insufficientFundsError != null) { return insufficientFundsError; }

        _context.Transaction.Add(
            new Transaction
            {
                AccountNumber = account.AccountNumber,
                Amount = amount,
                Comment = comment,
                TransactionType = TransactionType.Withdraw,
                TransactionTimeUtc = DateTime.UtcNow
            }
        );
  
        await ApplyServiceCharge(account, WithdrawServiceCharge);
        await _context.SaveChangesAsync();
        return null;
    }

    public async Task<string?> Transfer(Account account, Account destinationAccount, decimal amount, string? comment)
    {
        var invalidAmountError = GetInvalidInputError(amount, comment);
        if (invalidAmountError != null) { return invalidAmountError; }
        var insufficientFundsError = GetInsufficientFundsError(account, amount);
        if (insufficientFundsError != null) { return insufficientFundsError; }
        if (account.AccountNumber == destinationAccount.AccountNumber){return "Cannot transfer to the same account."; }

        _context.Transaction.Add(
            new Transaction
            {
                AccountNumber = account.AccountNumber,
                DestinationAccountNumber = destinationAccount.AccountNumber,
                Amount = amount,
                Comment = comment,
                TransactionType = TransactionType.Transfer,
                TransactionTimeUtc = DateTime.UtcNow
            }
        );

        _context.Transaction.Add(
            new Transaction
            {
                AccountNumber = destinationAccount.AccountNumber,
                Amount = amount,
                Comment = comment,
                TransactionType = TransactionType.Transfer,
                TransactionTimeUtc = DateTime.UtcNow
            }
        );

        await ApplyServiceCharge(account, TransferServiceCharge);
        await _context.SaveChangesAsync();
        return null;
    }

    private async Task ApplyServiceCharge(Account account, decimal amount)
    {
        var accountHasFreeTransactions = await AccountHasFreeTransactions(account);
        if ( accountHasFreeTransactions ) { return; }

        _context.Transaction.Add(
            new Transaction
            {
                AccountNumber = account.AccountNumber,
                Amount = amount,
                TransactionType = TransactionType.ServiceCharge,
                TransactionTimeUtc = DateTime.UtcNow
            }
        );
    }

    private string? GetInsufficientFundsError(Account account, decimal amount)
    {
        var accountBalance = account.GetBalance();
        var accountType = account.AccountType;
        var res = false;

        if (accountType == AccountType.Saving)
        {
            res = accountBalance - amount >= SavingsBalanceMinimum;
        }
        else if (accountType == AccountType.Checking)
        {
            res = accountBalance - amount >= CheckingBalanceMinimum;
        }

        if (!res)
        {
            return "Insufficient funds.";
        }

        return null;
    }
    
    private string? GetInvalidInputError(decimal amount, string? comment)
    {
        var isMoreThanMinimum = amount > TransactionsMinimum;
        if (!isMoreThanMinimum)
        {
            return $"Amount must be greater than {TransactionsMinimum}";
        }

        var isWithinMoneyRange = amount >= -MoneyDataLimit && amount <= MoneyDataLimit;
        if (!isWithinMoneyRange)
        {
            return $"Amount must be within -{MoneyDataLimit} and {MoneyDataLimit}";
        }

        if (comment != null && comment.Length > 30)
        {
            return "Comment must be less than 30 characters.";
        }
        
        return null;
    }

    private async Task<bool> AccountHasFreeTransactions(Account account)
    {
        var accountWithTransactions = await _context.Account.Include(x => x.AccountTransactions).FirstOrDefaultAsync(x => x.AccountNumber == account.AccountNumber);
        if (accountWithTransactions == null) { return false; }
        var transactions = accountWithTransactions.AccountTransactions;

        var withdrawOrTransferTransactions = transactions.Where(x => (x.TransactionType == TransactionType.Withdraw || x.TransactionType == TransactionType.Transfer)).ToList();
        if (withdrawOrTransferTransactions.Count <= FreeTransactionLimit) { return true; }
        return false;
    }
};