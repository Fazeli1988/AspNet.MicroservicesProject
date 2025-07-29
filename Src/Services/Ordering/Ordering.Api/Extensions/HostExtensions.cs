using Microsoft.EntityFrameworkCore;

namespace Ordering.Api.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host
            , Action<TContext, IServiceProvider> seeder,
            int? retry = 0) where TContext : DbContext
        {
            int retryForAvailability = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("migrating started for sql server");
                    InvokeSeeder(seeder, context,services);
                    logger.LogInformation("migrating has done for sql server");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "an error occurred while migrating database");
                    if (retryForAvailability > 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase(host, seeder, retryForAvailability);
                    }
                    throw;
                }
            }
            return host;
        }
        private static void InvokeSeeder<Tcontext>(
            Action<Tcontext, IServiceProvider> seeder,
            Tcontext context,
            IServiceProvider services)
            where Tcontext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
