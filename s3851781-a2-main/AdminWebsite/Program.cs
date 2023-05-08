using System.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.Extensions.Options;
using AdminWebsite.Filters;

var builder = WebApplication.CreateBuilder(args);

// Configure the default client.
builder.Services.AddHttpClient(Options.DefaultName, client =>
{
    client.BaseAddress = new Uri("http://localhost:5200");
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
});


builder.Services.AddControllersWithViews();

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

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.MapDefaultControllerRoute();

app.Run();
