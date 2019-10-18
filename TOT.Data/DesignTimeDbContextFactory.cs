using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace TOT.Data
{
    /* Пусть этот интерфейс просто будет. У меня без него не работают миграции:
     * Unable to create an object of type 'ApplicationDbContext'. Add an implementation of 
     * 'IDesignTimeDbContextFactory<ApplicationDbContext>' to the project, or see
     * https://go.microsoft.com/fwlink/?linkid=851728 for additional patterns supported at design time. */

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Environment.CurrentDirectory).ToString())
                .AddJsonFile("TOT.Web\\appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new ApplicationDbContext(builder.Options);
        }
    }
}
