using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextFactory : IDesignTimeDbContextFactory<OrderContext>
    {
        public OrderContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // مهم برای یافتن appsettings.json
                .AddJsonFile("appsettings.json") // یا appsettings.Development.json اگر استفاده می‌کنید
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<OrderContext>();
            var connectionString = configuration.GetConnectionString("OrderingConnectionString");

            optionsBuilder.UseSqlServer(connectionString);

            return new OrderContext(optionsBuilder.Options);
        }
    }
}
