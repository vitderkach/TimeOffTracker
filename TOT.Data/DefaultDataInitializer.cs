using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TOT.Entities;
using TOT.Interfaces;

namespace TOT.Data
{
    public static class DefaultDataInitializer
    {

        public static Dictionary<string, int> SeedUsersInfo(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {

                if (context.UserInformations.Any())
                {
                    return default;
                }

                var userInfo = new List<UserInformation>()
                {
                    new UserInformation(){ FirstName = "Matthew", LastName = "Brown",
                    VacationTypes = new List<VacationType>()
                    {
                        new VacationType() {TimeOffType = TimeOffType.AdministrativeLeave, StatutoryDays = 20, UsedDays = 1, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.StudyLeave, StatutoryDays = 10, UsedDays = 2, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.ConfirmedSickLeave, StatutoryDays = 30, UsedDays = 3, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.UnofficialSickLeave, StatutoryDays = 3, UsedDays = 4, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.GiftLeave, StatutoryDays = 5, UsedDays = 5, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.PaidLeave, StatutoryDays = 5, UsedDays = 0, Year = DateTime.Now.Year }
                    }
                    },

                    new UserInformation(){ FirstName = "Joseph", LastName = "White",
                    VacationTypes = new List<VacationType>()
                    {
                        new VacationType() {TimeOffType = TimeOffType.AdministrativeLeave, StatutoryDays = 20, UsedDays = 1, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.StudyLeave, StatutoryDays = 10, UsedDays = 2, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.ConfirmedSickLeave, StatutoryDays = 30, UsedDays = 3, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.UnofficialSickLeave, StatutoryDays = 3, UsedDays = 4, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.GiftLeave, StatutoryDays = 5, UsedDays = 5, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.PaidLeave, StatutoryDays = 5, UsedDays = 0, Year = DateTime.Now.Year }
                    }
                    },

                    new UserInformation(){ FirstName = "Nadia", LastName = "Campbell",
                    VacationTypes = new List<VacationType>()
                    {
                        new VacationType() {TimeOffType = TimeOffType.AdministrativeLeave, StatutoryDays = 20, UsedDays = 1, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.StudyLeave, StatutoryDays = 10, UsedDays = 2, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.ConfirmedSickLeave, StatutoryDays = 30, UsedDays = 3, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.UnofficialSickLeave, StatutoryDays = 3, UsedDays = 4, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.GiftLeave, StatutoryDays = 5, UsedDays = 5, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.PaidLeave, StatutoryDays = 5, UsedDays = 0, Year = DateTime.Now.Year }
                    }
                    },

                    new UserInformation(){ FirstName = "Alex", LastName = "Johnson",
                    VacationTypes = new List<VacationType>()
                    {
                        new VacationType() {TimeOffType = TimeOffType.AdministrativeLeave, StatutoryDays = 20, UsedDays = 1, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.StudyLeave, StatutoryDays = 10, UsedDays = 2, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.ConfirmedSickLeave, StatutoryDays = 30, UsedDays = 3, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.UnofficialSickLeave, StatutoryDays = 3, UsedDays = 4, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.GiftLeave, StatutoryDays = 5, UsedDays = 5, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.PaidLeave, StatutoryDays = 5, UsedDays = 0, Year = DateTime.Now.Year }
                    }
                    },

                    new UserInformation(){ FirstName = "Vitaliy", LastName = "Derkach",
                    VacationTypes = new List<VacationType>()
                    {
                        new VacationType() {TimeOffType = TimeOffType.AdministrativeLeave, StatutoryDays = 20, UsedDays = 1, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.StudyLeave, StatutoryDays = 10, UsedDays = 2, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.ConfirmedSickLeave, StatutoryDays = 30, UsedDays = 3, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.UnofficialSickLeave, StatutoryDays = 3, UsedDays = 4, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.GiftLeave, StatutoryDays = 5, UsedDays = 5, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.PaidLeave, StatutoryDays = 5, UsedDays = 0, Year = DateTime.Now.Year }
                    }
                    },

                    new UserInformation(){ FirstName = "Valeriya", LastName = "Alekseienko",
                    VacationTypes = new List<VacationType>()
                    {
                        new VacationType() {TimeOffType = TimeOffType.AdministrativeLeave, StatutoryDays = 20, UsedDays = 1, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.StudyLeave, StatutoryDays = 10, UsedDays = 2, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.ConfirmedSickLeave, StatutoryDays = 30, UsedDays = 3, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.UnofficialSickLeave, StatutoryDays = 3, UsedDays = 4, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.GiftLeave, StatutoryDays = 5, UsedDays = 5, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.PaidLeave, StatutoryDays = 5, UsedDays = 0, Year = DateTime.Now.Year }
                    }
                    },
                    new UserInformation(){ FirstName = "", LastName = "NAME001", RecruitmentDate = new System.DateTime(2018, 04, 12),
                    VacationTypes = new List<VacationType>()
                    {
                        new VacationType() {TimeOffType = TimeOffType.AdministrativeLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.StudyLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.ConfirmedSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.UnofficialSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.GiftLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.PaidLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year }
                    }
                    },
                    new UserInformation(){ FirstName = "", LastName = "NAME002", RecruitmentDate = new System.DateTime(2017, 07, 19),
                    VacationTypes = new List<VacationType>()
                    {
                        new VacationType() {TimeOffType = TimeOffType.AdministrativeLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.StudyLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.ConfirmedSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.UnofficialSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.GiftLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.PaidLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year }
                    }
                    },
                    new UserInformation(){ FirstName = "", LastName = "NAME003", RecruitmentDate = new System.DateTime(2019, 07, 23),
                    VacationTypes = new List<VacationType>()
                    {
                        new VacationType() {TimeOffType = TimeOffType.AdministrativeLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.StudyLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.ConfirmedSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.UnofficialSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.GiftLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.PaidLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year }
                    }
                    },
                    new UserInformation(){ FirstName = "", LastName = "NAME004", RecruitmentDate = new System.DateTime(2017, 01, 25),
                    VacationTypes = new List<VacationType>()
                    {
                        new VacationType() {TimeOffType = TimeOffType.AdministrativeLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.StudyLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.ConfirmedSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.UnofficialSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.GiftLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.PaidLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year }
                    }
                    },
                    new UserInformation(){ FirstName = "", LastName = "NAME005", IsFired = true, RecruitmentDate = new System.DateTime(2019, 10, 21),
                    VacationTypes = new List<VacationType>()
                    {
                        new VacationType() {TimeOffType = TimeOffType.AdministrativeLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.StudyLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.ConfirmedSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.UnofficialSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.GiftLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.PaidLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year }
                    }
                    },
                    new UserInformation(){ FirstName = "", LastName = "NAME006", RecruitmentDate = new System.DateTime(2019, 09, 04),
                    VacationTypes = new List<VacationType>()
                    {
                        new VacationType() {TimeOffType = TimeOffType.AdministrativeLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.StudyLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.ConfirmedSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.UnofficialSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.GiftLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.PaidLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year }
                    }
                    },
                    new UserInformation(){ FirstName = "", LastName = "NAME007", RecruitmentDate = new System.DateTime(2017, 07, 26),
                    VacationTypes = new List<VacationType>()
                    {
                        new VacationType() {TimeOffType = TimeOffType.AdministrativeLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.StudyLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.ConfirmedSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.UnofficialSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.GiftLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.PaidLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year }
                    }
                    },
                    new UserInformation(){ FirstName = "", LastName = "NAME008", RecruitmentDate = new System.DateTime(2007, 03, 01),
                    VacationTypes = new List<VacationType>()
                    {
                        new VacationType() {TimeOffType = TimeOffType.AdministrativeLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.StudyLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.ConfirmedSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.UnofficialSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.GiftLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.PaidLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year }
                    }
                    },
                    new UserInformation(){ FirstName = "", LastName = "NAME009", RecruitmentDate = new System.DateTime(2017, 12, 06),
                    VacationTypes = new List<VacationType>()
                    {
                        new VacationType() {TimeOffType = TimeOffType.AdministrativeLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.StudyLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.ConfirmedSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.UnofficialSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.GiftLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.PaidLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year }
                    }
                    },
                    new UserInformation(){ FirstName = "", LastName = "NAME010", RecruitmentDate = new System.DateTime(2006, 09, 01),
                    VacationTypes = new List<VacationType>()
                    {
                        new VacationType() {TimeOffType = TimeOffType.AdministrativeLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.StudyLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.ConfirmedSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.UnofficialSickLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.GiftLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year },
                        new VacationType() {TimeOffType = TimeOffType.PaidLeave, StatutoryDays = 0, UsedDays = 0, Year = DateTime.Now.Year }
                    }
                    },

                };

                context.UserInformations.AddRange(userInfo);
                context.SaveChanges();

                var info = context.UserInformations
                    .ToDictionary(pk => pk.LastName, v => v.ApplicationUserId);
                return info;
            }
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
            if (userManager.FindByEmailAsync("manager1@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = "manager1@gmail.com";
                user.UserName = "Manager1";
                user.RegistrationDate = new System.DateTime(2019, 10, 21, 09, 04, 17);
                user.UserInformationId = userInfo["Johnson"];

                IdentityResult result = userManager.CreateAsync(user, "Manager1TOT-159").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Manager").Wait();
                }
            }
            if (userManager.FindByEmailAsync("manager2@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = "manager2@gmail.com";
                user.UserName = "Manager2";
                user.RegistrationDate = new System.DateTime(2019, 10, 21, 09, 04, 17);
                user.UserInformationId = userInfo["Derkach"];

                IdentityResult result = userManager.CreateAsync(user, "Manager2TOT-543").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Manager").Wait();
                }
            }
            if (userManager.FindByEmailAsync("manager3@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = "manager3@gmail.com";
                user.UserName = "Manager3";
                user.RegistrationDate = new System.DateTime(2019, 10, 21, 09, 04, 17);
                user.UserInformationId = userInfo["Alekseienko"];

                IdentityResult result = userManager.CreateAsync(user, "Manager3TOT-836").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Manager").Wait();
                }
            }

            if (userManager.FindByEmailAsync("name001@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = "name001@gmail.com";
                user.UserName = "NAME001";
                user.RegistrationDate = new System.DateTime(2018, 04, 12);
                user.UserInformationId = userInfo["NAME001"];

                IdentityResult result = userManager.CreateAsync(user, "Example123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }
            if (userManager.FindByEmailAsync("name002@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = "name002@gmail.com";
                user.UserName = "NAME002";
                user.RegistrationDate = new System.DateTime(2017, 07, 19);
                user.UserInformationId = userInfo["NAME002"];

                IdentityResult result = userManager.CreateAsync(user, "Example123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }
            if (userManager.FindByEmailAsync("name003@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = "name003@gmail.com";
                user.UserName = "NAME003";
                user.RegistrationDate = new System.DateTime(2019, 07, 23);
                user.UserInformationId = userInfo["NAME003"];

                IdentityResult result = userManager.CreateAsync(user, "Example123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }
            if (userManager.FindByEmailAsync("name004@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = "name004@gmail.com";
                user.UserName = "NAME004";
                user.RegistrationDate = new System.DateTime(2017, 01, 25);
                user.UserInformationId = userInfo["NAME004"];

                IdentityResult result = userManager.CreateAsync(user, "Example123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }
            if (userManager.FindByEmailAsync("name005@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = "name005@gmail.com";
                user.UserName = "NAME005";
                user.RegistrationDate = new System.DateTime(2019, 10, 21);
                user.UserInformationId = userInfo["NAME005"];
                IdentityResult result = userManager.CreateAsync(user, "Example123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }
            if (userManager.FindByEmailAsync("name006@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = "name006@gmail.com";
                user.UserName = "NAME006";
                user.RegistrationDate = new System.DateTime(2019, 09, 04);
                user.UserInformationId = userInfo["NAME006"];

                IdentityResult result = userManager.CreateAsync(user, "Example123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }
            if (userManager.FindByEmailAsync("name007@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = "name007@gmail.com";
                user.UserName = "NAME007";
                user.RegistrationDate = new System.DateTime(2017, 07, 26);
                user.UserInformationId = userInfo["NAME007"];

                IdentityResult result = userManager.CreateAsync(user, "Example123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }
            if (userManager.FindByEmailAsync("name008@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = "name008@gmail.com";
                user.UserName = "NAME008";
                user.RegistrationDate = new System.DateTime(2007, 03, 01);
                user.UserInformationId = userInfo["NAME008"];

                IdentityResult result = userManager.CreateAsync(user, "Example123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }
            if (userManager.FindByEmailAsync("name009@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = "name009@gmail.com";
                user.UserName = "NAME009";
                user.RegistrationDate = new System.DateTime(2017, 12, 06);
                user.UserInformationId = userInfo["NAME009"];

                IdentityResult result = userManager.CreateAsync(user, "Example123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }
            if (userManager.FindByEmailAsync("name010@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = "name010@gmail.com";
                user.UserName = "NAME010";
                user.RegistrationDate = new System.DateTime(2006, 09, 01);
                user.UserInformationId = userInfo["NAME010"];

                IdentityResult result = userManager.CreateAsync(user, "Example123").Result;

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
