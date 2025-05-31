using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Api.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability=retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var sevices = scope.ServiceProvider;
                var configuration= sevices.GetRequiredService<IConfiguration>();
                var logger=sevices.GetRequiredService<ILogger<TContext>>();

                //migrate database
                try
                {
                    logger.LogInformation("migrating postgresql database");
                    using var connection = new NpgsqlConnection
               (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };
                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon (Id SERIAL PRIMARY KEY,
                                                                 ProductName VARCHAR(200) NOT NULL,
                                                                 Description TEXT,
                                                                 Amount INT)";
                    command.ExecuteNonQuery();

                    //seed data 
                    command.CommandText = "INSERT INTO Coupon(ProductName,Description,Amount) VALUES('Iphone x','Iphone Discount',150)";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(ProductName,Description,Amount) VALUES('Sumsung 10','Sumsung Discount',150)";
                    command.ExecuteNonQuery();

                    logger.LogInformation("migration has been completed!!!");
                }

                catch (NpgsqlException ex) 
                {
                    logger.LogError("an errore has been occured");
                    if (retryForAvailability < 50) 
                    {
                        retryForAvailability++;
                        Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvailability);

                    }
                }
            }
            return host;    
        }
       
    }
}
