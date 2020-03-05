using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TOT.Entities;

namespace TOT.Data
{
    public static class DefaultDataInitializer
    {

        public static Dictionary<string, int> SeedUsersInfo(IServiceProvider serviceProvider)
        {
            /* TODO: Because new tables have been added to the database and some types among tables have been changed, test data should be rewritten.
            AS a base use commented below code for previous version of the database */
            //using (var context = new ApplicationDbContext(
            //    serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            //{

            //    if (context.UserInformations.Any())
            //    {
            //        return default;
            //    }

            //    var userInfo = new List<UserInformation>()
            //    {
            //        new UserInformation(){ FirstName = "Matthew", LastName = "Brown",
            //            VacationPolicy = new VacationPolicy()
            //            {
            //                VacationTypes = new List<VacationType>()
            //                {
            //                    new VacationType() { TimeOffType = TimeOffType.ConfirmedSickLeave, UsedDays = 0 },
            //                    new VacationType() { TimeOffType = TimeOffType.StudyLeave, UsedDays = 0 },
            //                    new VacationType() { TimeOffType = TimeOffType.PaidLeave, UsedDays = 0 },
            //                    new VacationType() { TimeOffType = TimeOffType.UnofficialSickLeave, UsedDays = 0 }
            //                }
            //            }
            //        },
            //        new UserInformation(){ FirstName = "Joseph", LastName = "White",
            //            VacationPolicy = new VacationPolicy()
            //            {
            //                VacationTypes = new List<VacationType>()
            //                {
            //                    new VacationType() { TimeOffType = TimeOffType.ConfirmedSickLeave, UsedDays = 0 },
            //                    new VacationType() { TimeOffType = TimeOffType.StudyLeave, UsedDays = 0 },
            //                    new VacationType() { TimeOffType = TimeOffType.PaidLeave, UsedDays = 0 },
            //                    new VacationType() { TimeOffType = TimeOffType.UnofficialSickLeave, UsedDays = 0 }
            //                }
            //            }
            //        },
            //        new UserInformation(){ FirstName = "Nadia", LastName = "Campbell",
            //            VacationPolicy = new VacationPolicy()
            //            {
            //                VacationTypes = new List<VacationType>()
            //                {
            //                    new VacationType() { TimeOffType = TimeOffType.ConfirmedSickLeave, UsedDays = 0 },
            //                    new VacationType() { TimeOffType = TimeOffType.StudyLeave, UsedDays = 0 },
            //                    new VacationType() { TimeOffType = TimeOffType.PaidLeave, UsedDays = 0 },
            //                    new VacationType() { TimeOffType = TimeOffType.UnofficialSickLeave, UsedDays = 0 }
            //                }
            //            }
            //        }
            //    };

            //    context.UserInformations.AddRange(userInfo);
            //    context.SaveChanges();

            //    var info = context.UserInformations
            //        .ToDictionary(pk => pk.LastName, v => v.UserInformationId);
            //    return info;
            //}

            return default;
        }

        public static void SeedData(IServiceProvider serviceProvider, Dictionary<string, int> userInfo)
        {
            using (var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>())
            {
                SeedRoles(roleManager);
            }

            using (var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>())
            {
                SeedUsers(userManager, userInfo);
            }
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager,
            Dictionary<string, int> userInfo)
        {
            if (userManager.FindByEmailAsync("admin@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = "admin@gmail.com";
                user.UserName = "Admin";
                user.RegistrationDate = new System.DateTime(2019, 10, 16, 18, 24, 34);
                user.UserInformationId = userInfo["Brown"];

                IdentityResult result = userManager.CreateAsync(user, "AdminTOT-123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }

            if (userManager.FindByEmailAsync("employee1@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = "employee1@gmail.com";
                user.UserName = "TestUser1";
                user.RegistrationDate = new System.DateTime(2019, 10, 19, 14, 12, 51);
                user.UserInformationId = userInfo["White"];

                IdentityResult result = userManager.CreateAsync(user, "Employee1TOT-357").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }

            if (userManager.FindByEmailAsync("employee2@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = "employee2@gmail.com";
                user.UserName = "TestUser2";
                user.RegistrationDate = new System.DateTime(2019, 10, 21, 09, 04, 17);
                user.UserInformationId = userInfo["Campbell"];

                IdentityResult result = userManager.CreateAsync(user, "Employee2TOT-159").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole<int>> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                IdentityRole<int> role = new IdentityRole<int>();
                role.Name = "Administrator";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Employee").Result)
            {
                IdentityRole<int> role = new IdentityRole<int>();
                role.Name = "Employee";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Manager").Result)
            {
                IdentityRole<int> role = new IdentityRole<int>();
                role.Name = "Manager";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
