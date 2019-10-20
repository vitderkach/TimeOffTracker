using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TOT.Business.Services;
using TOT.Data;
using TOT.Data.Repositories;
using TOT.Data.UnitOfWork;
using TOT.Entities;
using TOT.Interfaces;
using TOT.Interfaces.Repositories;
using TOT.Interfaces.Services;
using TOT.Utility.AutoMapper;

namespace TOT.Utility.DI
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
        public static void RegisterRepositoriesAndServices(this IServiceCollection services)
        {
            services.AddTransient<IRepository<UserInformation>, UserInformationRepository>();

            services.AddTransient<IUserInformationService, UserInformationService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddSingleton<Interfaces.IMapper, TOTAutoMapper>();
        }
        public static void AddCustomIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole<int>>(opts =>
            {
                //opts.
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        }
    }
}
