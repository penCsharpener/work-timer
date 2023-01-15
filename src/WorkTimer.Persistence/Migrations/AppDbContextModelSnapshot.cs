﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkTimer.Persistence.Data;

#nullable disable

namespace WorkTimer.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0-rc.1.22426.7");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ConcurrencyStamp = "cd40199e-54cd-4717-8901-e3f3efefbf59",
                            Name = "User",
                            NormalizedName = "USER"
                        },
                        new
                        {
                            Id = 2,
                            ConcurrencyStamp = "7cdbc6c1-ca8c-4b83-ac45-9101ac88661f",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        });
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.Contract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Employer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("HoursPerWeek")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsCurrent")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("TotalOverhours")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ContractId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue(new DateTime(2022, 10, 23, 17, 0, 11, 565, DateTimeKind.Local).AddTicks(7377));

                    b.Property<string>("Description")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("Medium");

                    b.Property<bool>("IsContractIndependent")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ReminderAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tags")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.HasIndex("UserId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.Todo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ContractId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue(new DateTime(2022, 10, 23, 17, 0, 11, 566, DateTimeKind.Local).AddTicks(2737));

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsContractIndependent")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Priority")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(1);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.HasIndex("UserId");

                    b.ToTable("Todos");
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.WorkDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ContractId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<double>("RequiredHours")
                        .HasColumnType("REAL");

                    b.Property<double>("TotalHours")
                        .HasColumnType("REAL");

                    b.Property<int>("WorkDayType")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("WorkMonthId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("WorkWeekId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WorkMonthId");

                    b.HasIndex("WorkWeekId");

                    b.HasIndex("ContractId", "Date")
                        .IsUnique();

                    b.ToTable("WorkDays");
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.WorkMonth", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ContractId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DaysOffWork")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DaysWorked")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Month")
                        .HasColumnType("INTEGER");

                    b.Property<double>("TotalHours")
                        .HasColumnType("REAL");

                    b.Property<double>("TotalOverhours")
                        .HasColumnType("REAL");

                    b.Property<double>("TotalRequiredHours")
                        .HasColumnType("REAL");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ContractId", "Month", "Year")
                        .IsUnique();

                    b.ToTable("WorkMonths");
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.WorkWeek", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ContractId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DaysOffWork")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DaysWorked")
                        .HasColumnType("INTEGER");

                    b.Property<double>("TotalHours")
                        .HasColumnType("REAL");

                    b.Property<double>("TotalOverhours")
                        .HasColumnType("REAL");

                    b.Property<double>("TotalRequiredHours")
                        .HasColumnType("REAL");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WeekNumber")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("WeekStart")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("ContractId", "WeekStart")
                        .IsUnique();

                    b.ToTable("WorkWeeks");
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.WorkingPeriod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("WorkDayId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WorkDayId");

                    b.ToTable("WorkingPeriods");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("WorkTimer.Domain.Models.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("WorkTimer.Domain.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("WorkTimer.Domain.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("WorkTimer.Domain.Models.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkTimer.Domain.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("WorkTimer.Domain.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.Contract", b =>
                {
                    b.HasOne("WorkTimer.Domain.Models.AppUser", "User")
                        .WithMany("Contracts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.Note", b =>
                {
                    b.HasOne("WorkTimer.Domain.Models.Contract", "Contract")
                        .WithMany("Notes")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkTimer.Domain.Models.AppUser", "AppUser")
                        .WithMany("Notes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.Todo", b =>
                {
                    b.HasOne("WorkTimer.Domain.Models.Contract", "Contract")
                        .WithMany("Todos")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkTimer.Domain.Models.AppUser", "AppUser")
                        .WithMany("Todos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.WorkDay", b =>
                {
                    b.HasOne("WorkTimer.Domain.Models.Contract", "Contract")
                        .WithMany("WorkDays")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("WorkTimer.Domain.Models.WorkMonth", "WorkMonth")
                        .WithMany("WorkDays")
                        .HasForeignKey("WorkMonthId");

                    b.HasOne("WorkTimer.Domain.Models.WorkWeek", "WorkWeek")
                        .WithMany("WorkDays")
                        .HasForeignKey("WorkWeekId");

                    b.Navigation("Contract");

                    b.Navigation("WorkMonth");

                    b.Navigation("WorkWeek");
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.WorkMonth", b =>
                {
                    b.HasOne("WorkTimer.Domain.Models.Contract", "Contract")
                        .WithMany()
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.WorkWeek", b =>
                {
                    b.HasOne("WorkTimer.Domain.Models.Contract", "Contract")
                        .WithMany()
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkTimer.Domain.Models.AppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Contract");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.WorkingPeriod", b =>
                {
                    b.HasOne("WorkTimer.Domain.Models.WorkDay", "WorkDay")
                        .WithMany("WorkingPeriods")
                        .HasForeignKey("WorkDayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkDay");
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.AppUser", b =>
                {
                    b.Navigation("Contracts");

                    b.Navigation("Notes");

                    b.Navigation("Todos");
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.Contract", b =>
                {
                    b.Navigation("Notes");

                    b.Navigation("Todos");

                    b.Navigation("WorkDays");
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.WorkDay", b =>
                {
                    b.Navigation("WorkingPeriods");
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.WorkMonth", b =>
                {
                    b.Navigation("WorkDays");
                });

            modelBuilder.Entity("WorkTimer.Domain.Models.WorkWeek", b =>
                {
                    b.Navigation("WorkDays");
                });
#pragma warning restore 612, 618
        }
    }
}
