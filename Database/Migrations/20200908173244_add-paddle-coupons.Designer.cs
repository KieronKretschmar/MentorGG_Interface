﻿// <auto-generated />
using System;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Database.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20200908173244_add-paddle-coupons")]
    partial class addpaddlecoupons
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Entities.Models.ApplicationRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("DailyMatchesLimit")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Entities.Models.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("AcceptedTermsOfService")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<long>("RefererSteamId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Registration")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<long>("SteamId")
                        .HasColumnType("bigint");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Entities.Models.Paddle.Alerts.SubscriptionCancelled", b =>
                {
                    b.Property<int>("AlertId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CancellationEffectiveDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CheckoutId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Currency")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("EventTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("MarketingConsent")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Passthrough")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Quantity")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Status")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("int");

                    b.Property<int>("SubscriptionPlanId")
                        .HasColumnType("int");

                    b.Property<string>("UnitPrice")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("AlertId");

                    b.ToTable("SubscriptionCancelled");
                });

            modelBuilder.Entity("Entities.Models.Paddle.Alerts.SubscriptionCreated", b =>
                {
                    b.Property<int>("AlertId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CancelUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CheckoutId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Currency")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("EventTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("MarketingConsent")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("NextBillDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Passthrough")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Quantity")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Source")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Status")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("int");

                    b.Property<int>("SubscriptionPlanId")
                        .HasColumnType("int");

                    b.Property<string>("UnitPrice")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UpdateUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("AlertId");

                    b.ToTable("SubscriptionCreated");
                });

            modelBuilder.Entity("Entities.Models.Paddle.Alerts.SubscriptionPaymentFailed", b =>
                {
                    b.Property<int>("AlertId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Amount")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("AttemptNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CancelUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CheckoutId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Currency")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("EventTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Installments")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("MarketingConsent")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("NextRetryDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("OrderId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Passthrough")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Quantity")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Status")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("int");

                    b.Property<string>("SubscriptionPaymentId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("SubscriptionPlanId")
                        .HasColumnType("int");

                    b.Property<string>("UnitPrice")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UpdateUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("AlertId");

                    b.ToTable("SubscriptionPaymentFailed");
                });

            modelBuilder.Entity("Entities.Models.Paddle.Alerts.SubscriptionPaymentRefunded", b =>
                {
                    b.Property<int>("AlertId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Amount")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("BalanceCurrency")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("BalanceEarningsDecrease")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("BalanceFeeRefund")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("BalanceGrossRefund")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("BalanceTaxRefund")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CheckoutId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Currency")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CustomerName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("EarningsDecrease")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("EventTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FeeRefund")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("GrossRefund")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("InitialPayment")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Instalments")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("MarketingConsent")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("OrderId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Passthrough")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Quantity")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RefundReason")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RefundType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Status")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("SubscriptionId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("SubscriptionPaymentId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("SubscriptionPlanId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("TaxRefund")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UnitPrice")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("AlertId");

                    b.ToTable("SubscriptionPaymentRefunded");
                });

            modelBuilder.Entity("Entities.Models.Paddle.Alerts.SubscriptionPaymentSucceeded", b =>
                {
                    b.Property<int>("AlertId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("BalanceCurrency")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("BalanceEarnings")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("BalanceFee")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("BalanceGross")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("BalanceTax")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CheckoutId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Country")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Coupon")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Currency")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CustomerName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Earnings")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("EventTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Fee")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("InitialPayment")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Instalments")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("MarketingConsent")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("NextBillDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NextPaymentAmount")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("OrderId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Passthrough")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PaymentTax")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PlanName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Quantity")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ReceiptUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("SaleGross")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Status")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("SubscriptionId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("SubscriptionPaymentId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("SubscriptionPlanId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UnitPrice")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("AlertId");

                    b.ToTable("SubscriptionPaymentSucceeded");
                });

            modelBuilder.Entity("Entities.Models.Paddle.Alerts.SubscriptionUpdated", b =>
                {
                    b.Property<int>("AlertId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CancelUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CheckoutId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Currency")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("EventTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("MarketingConsent")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("NewPrice")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("NewQuantity")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("NewUnitPrice")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("NextBillDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("OldNextBillDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("OldPrice")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("OldQuantity")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("OldStatus")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("OldSubscriptionPlanId")
                        .HasColumnType("int");

                    b.Property<string>("OldUnitPrice")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Passthrough")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("PausedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("PausedFrom")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Status")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("int");

                    b.Property<int>("SubscriptionPlanId")
                        .HasColumnType("int");

                    b.Property<string>("UpdateUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("AlertId");

                    b.ToTable("SubscriptionUpdated");
                });

            modelBuilder.Entity("Entities.Models.Paddle.PaddlePlan", b =>
                {
                    b.Property<int>("PlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("MonthlyPrice")
                        .HasColumnType("double");

                    b.Property<int>("Months")
                        .HasColumnType("int");

                    b.Property<byte>("SubscriptionType")
                        .HasColumnType("tinyint unsigned");

                    b.HasKey("PlanId");

                    b.ToTable("PaddlePlan");
                });

            modelBuilder.Entity("Entities.Models.Paddle.PaddlePlanRole", b =>
                {
                    b.Property<int>("PlanId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("PlanId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("PaddlePlanRole");
                });

            modelBuilder.Entity("Entities.Models.Paddle.PaddleReferralCoupon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Coupon")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<long>("SteamId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("PaddleReferralCoupon");
                });

            modelBuilder.Entity("Entities.Models.Paddle.PaddleSubscription", b =>
                {
                    b.Property<int>("SubscriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ApplicationUserId")
                        .HasColumnType("int");

                    b.Property<string>("CancelUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("SubscriptionPlanId")
                        .HasColumnType("int");

                    b.Property<string>("UpdateUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("SubscriptionId");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("SubscriptionPlanId");

                    b.ToTable("PaddleSubscription");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Value")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("Entities.Models.Paddle.PaddlePlanRole", b =>
                {
                    b.HasOne("Entities.Models.Paddle.PaddlePlan", "PaddlePlan")
                        .WithMany("PaddlePlanRoles")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Models.ApplicationRole", "Role")
                        .WithMany("PaddlePlanRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Entities.Models.Paddle.PaddleSubscription", b =>
                {
                    b.HasOne("Entities.Models.ApplicationUser", "User")
                        .WithMany("PaddleSubscriptions")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Models.Paddle.PaddlePlan", "PaddlePlan")
                        .WithMany("Subscriptions")
                        .HasForeignKey("SubscriptionPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Entities.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Entities.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Entities.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Entities.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Entities.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
