using CustomerWebsite.Data;
using CustomerWebsite.Models;
using CustomerWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerWebsite.Controllers
{
    public class BillPayController : Controller
    {
        private readonly McbaContext _context;

        public BillPayController(McbaContext context) => _context = context;

        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public async Task<IActionResult> Index()
        {
            var accounts = await _context.Account.Where(x => x.CustomerID == CustomerID).ToListAsync();
            var billPays = await _context.BillPay.Where(x => accounts.Select(a => a.AccountNumber).Contains(x.AccountNumber)).ToListAsync();
            var failedBillPays = billPays.Where(x => x.Failed && x.Active).ToList();
            return View(new BillPayAndAlertsViewModel()
            {
                BillPays = billPays,
                HasAlerts = failedBillPays.Count > 0
            });
        }

        public async Task<IActionResult> Alerts()
        {
            var accounts = await _context.Account.Where(x => x.CustomerID == CustomerID).ToListAsync();
            var billPays = await _context.BillPay.Where(x => accounts.Select(a => a.AccountNumber).Contains(x.AccountNumber)).ToListAsync();
            var failedBillPays = billPays.Where(x => x.Failed && x.Active).ToList();
            
            return View(failedBillPays);
        }

        [HttpPost]
        public async Task<IActionResult> Alerts(int BillPayID)
        {
            var accounts = await _context.Account.Where(x => x.CustomerID == CustomerID).ToListAsync();
            var accountNumbers = accounts.Select(x => x.AccountNumber);
            var billPays = await _context.BillPay.Where(x => accountNumbers.Contains(x.AccountNumber) && x.BillPayID == BillPayID).ToListAsync();

            if (billPays.Count == 0)
            {
                return NotFound();
            }
            var billPay = billPays.First();
            billPay.Active = false;
            _context.Update(billPay);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Alerts));
        }

        public IActionResult CreateBillPay()
        {
            return View(new CreateBillPayViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateBillPay(CreateBillPayViewModel payload)
        {
            if (!ModelState.IsValid)
            {
                return View(payload);
            }

            var account = _context.Account.FirstOrDefault(x => x.AccountNumber == payload.AccountNumber && x.CustomerID == CustomerID);
            
            if (account == null) {
                ModelState.AddModelError(nameof(payload.AccountNumber), "Account number can't be found");
                return View(payload);
            }

            var payee = _context.Payee.FirstOrDefault(x => x.PayeeID == payload.PayeeID);
            if (payee == null) {
                ModelState.AddModelError(nameof(payload.PayeeID), "Payee not found");
                return View(payload);
            }
            
            _context.BillPay.Add(
                new BillPay
                {
                    AccountNumber = account.AccountNumber,
                    PayeeID = payload.PayeeID,
                    Amount = payload.Amount > 0 ? payload.Amount : -payload.Amount,
                    ScheduleTimeUtc = payload.ScheduleTimeUtc,
                    Period = payload.Period,
                    Active = true,
                    Failed = false
                });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Payees()
        {
            var payees = await _context.Payee.ToListAsync();
            return View(payees);
        }

        public IActionResult CreatePayee()
        {
            return View(new Payee());
        }
        
        [HttpPost]
        public async Task<IActionResult> CreatePayee(Payee payee)
        {
            if (!ModelState.IsValid)
            {
                return View(payee);
            }
              
            _context.Payee.Add(payee);
            await _context.SaveChangesAsync();

            return Redirect(nameof(Payees));
        }

        
        
    }
}
