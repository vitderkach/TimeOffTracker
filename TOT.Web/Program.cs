using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TOT.Data.Exceptions;
/* Куда лучше поместить класс DefaultDataInitializer? */
using TOT.Data;

namespace TOT.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                try
                {
                    var userInfo = DefaultDataInitializer.SeedUsersInfo(serviceProvider);

                    if (userInfo != null)
                    {
                        DefaultDataInitializer.SeedData(serviceProvider, userInfo);
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while executing the sql query", ex);
                }
                catch (Exception ex)
                {
                    throw new DatabaseSeedingFailedException("An error occurred while seeding the database", ex);
                }
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
