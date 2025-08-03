using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!await orderContext.Orders.AnyAsync())
            {
                await orderContext.Orders.AddRangeAsync(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("data seed section configured");
            }
        }

        public static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    FirstName = "mahdi",
                    LastName = "fazeli",
                    UserName = "ali",
                    EmailAddress = "test@test.com",
                    City = "verona",
                    Country = "italy",
                    TotalPrice = 10000,
                    BankNumber="1000",
                    PhoneNumber="1234567890",
                    RefCode="123",
                    ZipCode="123"

                    
                }
            };
        }
    }
}
