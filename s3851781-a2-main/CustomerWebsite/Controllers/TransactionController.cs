using CustomerWebsite.Data;
using CustomerWebsite.Models;
using CustomerWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerWebsite.Controllers
{
    public class TransactionController : Controller
    {
        private readonly McbaContext _context;

        public TransactionController(McbaContext context) => _context = context;

        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public async Task<IActionResult> Deposit(int id)
        {
            var account = await _context.Account.FirstOrDefaultAsync(x => x.AccountNumber == id && x.CustomerID == CustomerID);
            if (account == null) { return NotFound(); }
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(int id, decimal amount, string? comment)
        {
            var account = await _context.Account.FirstOrDefaultAsync(x => x.AccountNumber == id && x.CustomerID == CustomerID);
            if (account == null)
            {
                ModelState.AddModelError(nameof(account), "The account you are trying to deposit to cannot be found.");
                return View(account);
            }

            var transactionService = new McbaTransactionService(_context);
            var error = await transactionService.Deposit(account, amount, comment);
            if (error != null)
            {
                ModelState.AddModelError(nameof(account), error);
                return View(account);
            }

            return RedirectToAction("Index", "Customer");
        }

        public async Task<IActionResult> Withdraw(int id)
        {
            var account = await _context.Account.FirstOrDefaultAsync(x => x.AccountNumber == id && x.CustomerID == CustomerID);
            if (account == null) { return NotFound(); }

            return View(account);
        }


        [HttpPost]
        public async Task<IActionResult> Withdraw(int id, decimal amount, string? comment)
        {
            var account = await _context.Account.Include(x => x.AccountTransactions).FirstOrDefaultAsync(x => x.AccountNumber == id && x.CustomerID == CustomerID);
            if (account == null)
            {
                ModelState.AddModelError(nameof(account), "The account you are trying to withdraw from cannot be found.");
                return View(account);
            }

            var transactionService = new McbaTransactionService(_context);
            var error = await transactionService.Withdraw(account, amount, comment);
            if (error != null)
            {
                ModelState.AddModelError(nameof(account), error);
                return View(account);
            }

            return RedirectToAction("Index", "Customer");
        }

        public async Task<IActionResult> Transfer(int id)
        {
            var account = await _context.Account.FirstOrDefaultAsync(x => x.AccountNumber == id && x.CustomerID == CustomerID);
            if (account == null) { return NotFound(); }

            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(int id, int destinationAccountNumber, decimal amount, string? comment)
        {
            var account = await _context.Account.Include(x => x.AccountTransactions).FirstOrDefaultAsync(x => x.AccountNumber == id && x.CustomerID == CustomerID);
            var destinationAccount = await _context.Account.FirstOrDefaultAsync(x => x.AccountNumber == destinationAccountNumber);

            if (account == null)
            {
                ModelState.AddModelError(nameof(account), "The account you are trying to transfer from cannot be found.");
                return View(account);
            }

            if (destinationAccount == null)
            {
                ModelState.AddModelError(nameof(account), "The account you are trying to transfer to cannot be found.");
                return View(account);
            }

            var transactionService = new McbaTransactionService(_context);
            var error = await transactionService.Transfer(account, destinationAccount, amount, comment);
            if (error != null)
            {
                ModelState.AddModelError(nameof(account), error);
                return View(account);
            }

            return RedirectToAction("Index", "Customer");
        }

        public async Task<IActionResult> MyStatements(int id)
        {
            var account = await _context.Account.Include(x => x.AccountTransactions).FirstOrDefaultAsync(x => x.AccountNumber == id && x.CustomerID == CustomerID);
            if (account == null) { return NotFound(); }

            var MyStatements = new MyStatementsViewModel
            {
                Transactions = account.AccountTransactions,
                AccountNumber = account.AccountNumber,
                Balance = account.GetBalance()
            };
            return View(MyStatements);
        }
    }
}
