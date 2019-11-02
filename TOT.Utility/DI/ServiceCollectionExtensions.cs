using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
using TOT.Utility.EmailNotification;

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
            services.AddTransient<IRepository<VacationRequest>, VacationRequestRepository>();

            services.AddTransient<IUserInfoService, UserInformationService>();
            services.AddTransient<IVacationService, VacationService>();
            services.AddTransient<IUserService, UserService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        public static void RegisterEmailSender(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEmailSendConfiguration>(
                configuration.GetSection("EmailConfiguration").Get<EmailSendConfiguration>());

            services.AddTransient<IEmailSender, EmailSender>();
        }
        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddSingleton<Interfaces.IMapper, TOTAutoMapper>();
        }
        public static void AddCustomIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole<int>>(opts =>
            {
                opts.Password.RequireDigit = false;
                opts.Password.RequiredLength = 4;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        }
    }
}
