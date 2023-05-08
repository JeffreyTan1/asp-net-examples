using Microsoft.EntityFrameworkCore;
using MvcStore.Models;

namespace MvcStore.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new MvcStoreContext(
            serviceProvider.GetRequiredService<DbContextOptions<MvcStoreContext>>());

        // Look for any orders.
        if (context.Order.Any() || context.Product.Any() || context.OrderedProducts.Any())
            return; // DB has been seeded.

        // Products
        var kneeSleeves = new Product
        {
            Name = "SBD Knee Sleeves",
            Price = 124.95M
        };
        var belt = new Product
        {
            Name = "SBD Powerlifting Belt",
            Price = 329.95M
        };
        var lifters = new Product
        {
            Name = "Notorius Lifters Gen 2",
            Price = 114.95M
        };
        var softSuit = new Product
        {
            Name = "A7 Soft Suit",
            Price = 149.95M
        };

        context.Product.AddRange(kneeSleeves, belt, lifters, softSuit);

        // Orders
        var firstOrder = new Order
        {
            OrderDate = DateTime.Now,
            CustomerName = "Jeffrey Tan",
            DeliveryAddress = "414 LaTrobe Street, Melbourne, VIC 3000"
        };
        var secondOrder = new Order
        {
            OrderDate = DateTime.Now,
            CustomerName = "John Haack",
            DeliveryAddress = "242 Exhibition Street, Melbourne, VIC 3000"
        };

        context.Order.AddRange(firstOrder, secondOrder);

        // MTM relationship for order and product
        context.OrderedProducts.AddRange(
            new OrderedProducts
            {
                Order = firstOrder,
                Product = kneeSleeves,
                Quantity = 1
            },
            new OrderedProducts
            {
                Order = firstOrder,
                Product = belt,
                Quantity = 1
            },

            new OrderedProducts
            {
                Order = secondOrder,
                Product = lifters,
                Quantity = 1
            },
            new OrderedProducts
            {
                Order = secondOrder,
                Product = softSuit,
                Quantity = 1
            }
        );

        context.SaveChanges();
    }
}
