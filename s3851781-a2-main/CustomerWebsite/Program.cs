using CustomerWebsite.Data;
using CustomerWebsite.Filters;
using CustomerWebsite.Models;
using CustomerWebsite.Models.Types;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<McbaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(McbaContext)));
});

// Store session into Web-Server memory.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    // Make the session cookie essential.
    options.Cookie.IsEssential = true;
});

// Add global authorization check.
builder.Services.AddControllersWithViews(options => options.Filters.Add(new AuthorizeCustomerAttribute()));

var app = builder.Build();

// Initialisation Steps
using(var scope = app.Services.CreateScope())
{
    // Seed Data
    var services = scope.ServiceProvider;
    try
    {
        SeedData.Initialize(services);
    }
    catch(Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

async void ProcessBillPays()
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var _context = services.GetRequiredService<McbaContext>();
        Console.WriteLine("Processing BillPays...");
        var billPays = _context.BillPay.Where(x => x.Active && x.ScheduleTimeUtc <= DateTime.UtcNow).ToList();
        foreach (var billPay in billPays)
        {
            var account = _context.Account.Include(x => x.AccountTransactions).FirstOrDefault(x => x.AccountNumber == billPay.AccountNumber);
            if (account == null) { continue; }
            var payee = _context.Payee.FirstOrDefault(x => x.PayeeID == billPay.PayeeID);
            if (payee == null) { continue; }

            if (account.GetBalance() <= billPay.Amount)
            {
                billPay.Failed = true;
                _context.Update(billPay);
                _context.SaveChanges();
                continue;
            }

            var transaction = new Transaction
            {
                AccountNumber = account.AccountNumber,
                TransactionType = TransactionType.BillPay,
                Amount = billPay.Amount,
                Comment = $"BillPay to {payee.PayeeID}",
                TransactionTimeUtc = DateTime.UtcNow,
            };
            account.AccountTransactions.Add(transaction);

            if (billPay.Period == "O")
            {
                billPay.Active = false;
            }
            else
            {
                billPay.ScheduleTimeUtc = DateTime.UtcNow.AddMonths(1);
            }
            billPay.Failed = false;
            _context.Update(billPay);
            await _context.SaveChangesAsync();
        }
    }
}

// Setup Cron Job to process BillPays
// Will also run ProcessBillPays at the start of the server
var startTimeSpan = TimeSpan.Zero;
var periodTimeSpan = TimeSpan.FromMinutes(1);
var timer = new Timer(async (e) =>
{
   ProcessBillPays();
}, null, startTimeSpan, periodTimeSpan);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.MapDefaultControllerRoute();

app.Run();
