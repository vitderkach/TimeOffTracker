﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TOT.Data;

namespace TOT.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TOT.Entities.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ApplicationUserId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<DateTime?>("RegistrationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<int>("UserInformationId");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("UserInformationId")
                        .IsUnique();

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TOT.Entities.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Id")
                        .HasName("PK_Location");

                    b.HasAlternateKey("Name");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("TOT.Entities.ManagerResponse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Approval");

                    b.Property<DateTime>("DateResponse")
                        .HasColumnType("datetime2(0)");

                    b.Property<int>("ForStageOfApproving")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(1);

                    b.Property<int>("ManagerId");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("VacationRequestId");

                    b.HasKey("Id")
                        .HasName("PK_ManagerResponse");

                    b.HasIndex("ManagerId");

                    b.HasIndex("VacationRequestId");

                    b.ToTable("ManagerResponses");
                });

            modelBuilder.Entity("TOT.Entities.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Id")
                        .HasName("PK_Team");

                    b.HasAlternateKey("Name");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("TOT.Entities.UserInformation", b =>
                {
                    b.Property<int>("ApplicationUserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(70)");

                    b.Property<bool>("IsFired");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(70)");

                    b.Property<int?>("LocationId");

                    b.Property<DateTime?>("RecruitmentDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int?>("TeamId");

                    b.HasKey("ApplicationUserId")
                        .HasName("PK_UserInformation");

                    b.HasIndex("LocationId");

                    b.HasIndex("TeamId");

                    b.HasIndex("FirstName", "LastName");

                    b.ToTable("UserInformations");
                });

            modelBuilder.Entity("TOT.Entities.VacationRequest", b =>
                {
                    b.Property<int>("VacationRequestId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Approval");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("date");

                    b.Property<bool?>("ExcelFormat");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool?>("SelfCancelled");

                    b.Property<int>("StageOfApproving")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(1);

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.Property<int?>("TakenDays");

                    b.Property<int>("UserInformationId");

                    b.Property<string>("VacationType")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("VacationRequestId")
                        .HasName("PK_VacationRequest");

                    b.HasIndex("EndDate");

                    b.HasIndex("StartDate");

                    b.HasIndex("UserInformationId");

                    b.HasIndex("VacationType");

                    b.ToTable("VacationRequests");
                });

            modelBuilder.Entity("TOT.Entities.VacationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("StatutoryDays");

                    b.Property<string>("TimeOffType")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("UsedDays")
                        .HasDefaultValue(0);

                    b.Property<int>("UserInformationId");

                    b.Property<int>("Year");

                    b.HasKey("Id")
                        .HasName("PK_ VacationType");

                    b.HasAlternateKey("UserInformationId", "Year", "TimeOffType")
                        .HasName("PK_ VacationType_UserInformationId_Year_TimeOffType");

                    b.ToTable("VacationTypes");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("TOT.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("TOT.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TOT.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("TOT.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TOT.Entities.ApplicationUser", b =>
                {
                    b.HasOne("TOT.Entities.UserInformation", "UserInformation")
                        .WithOne("ApplicationUser")
                        .HasForeignKey("TOT.Entities.ApplicationUser", "UserInformationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TOT.Entities.ManagerResponse", b =>
                {
                    b.HasOne("TOT.Entities.UserInformation", "Manager")
                        .WithMany("ManagerResponses")
                        .HasForeignKey("ManagerId")
                        .HasConstraintName("FK_ManagerResponse_UserInformation")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TOT.Entities.VacationRequest", "VacationRequest")
                        .WithMany("ManagersResponses")
                        .HasForeignKey("VacationRequestId")
                        .HasConstraintName("FK_ManagerResponse_VacationRequest")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TOT.Entities.UserInformation", b =>
                {
                    b.HasOne("TOT.Entities.Location", "Location")
                        .WithMany("UserInformations")
                        .HasForeignKey("LocationId");

                    b.HasOne("TOT.Entities.Team", "Team")
                        .WithMany("UserInformations")
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("TOT.Entities.VacationRequest", b =>
                {
                    b.HasOne("TOT.Entities.UserInformation", "UserInformation")
                        .WithMany("VacationRequests")
                        .HasForeignKey("UserInformationId")
                        .HasConstraintName("FK_VacationRequest_UserInformation")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TOT.Entities.VacationType", b =>
                {
                    b.HasOne("TOT.Entities.UserInformation", "UserInformation")
                        .WithMany("VacationTypes")
                        .HasForeignKey("UserInformationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
