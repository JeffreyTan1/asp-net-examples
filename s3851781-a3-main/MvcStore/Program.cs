using System.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MvcStore.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MvcStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MvcStoreContext")));


// Configure the default client.
builder.Services.AddHttpClient(Options.DefaultName, client =>
{
    client.BaseAddress = new Uri("http://localhost:5200");
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
});

builder.Services.AddControllersWithViews();

// Ignore JSON reference cycles during serialisation.
//builder.Services.AddControllersWithViews().AddJsonOptions(options =>
//    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// Seed data.
using(var scope = app.Services.CreateScope())
{
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

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
