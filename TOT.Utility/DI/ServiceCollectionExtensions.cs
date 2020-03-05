using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
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
            services.AddTransient<IRepository<ManagerResponse>, ManagerResponseRepository>();
            services.AddTransient<IRepository<VacationType>, VacationTypeRepository>();

            services.AddTransient<IUserInfoService, UserInformationService>();
            services.AddTransient<IVacationService, VacationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IManagerService, ManagerService>();
            services.AddTransient<IVacationEmailSender, VacationEmailSender>();
            services.AddTransient<IAdminService, AdminService>();

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
        public static void AddTempDataProvider(this IServiceCollection services)
        {
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
        }
        public static void AddCustomIdentity(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddIdentity<ApplicationUser, IdentityRole<int>>(opts =>
            {
                opts.Password.RequireDigit = false;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase = false;

                opts.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(
                options =>
                {
                    options.LoginPath = $"/Identity/Account/Login";
                    options.LogoutPath = $"/Identity/Account/Logout";
                    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
                });
        }
    }
}
