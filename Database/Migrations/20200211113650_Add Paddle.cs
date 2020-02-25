using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class AddPaddle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaddleUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    MarketingConsent = table.Column<bool>(nullable: false),
                    SubscriptionId = table.Column<string>(nullable: true),
                    SubscriptionPlanId = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    CancelUrl = table.Column<string>(nullable: true),
                    UpdateUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaddleUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionCancelled",
                columns: table => new
                {
                    AlertId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CancellationEffectiveDate = table.Column<DateTime>(nullable: false),
                    CheckoutId = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EventTime = table.Column<DateTime>(nullable: false),
                    MarketingConsent = table.Column<bool>(nullable: false),
                    Passthrough = table.Column<string>(nullable: true),
                    Quantity = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    SubscriptionId = table.Column<string>(nullable: true),
                    SubscriptionPlanId = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionCancelled", x => x.AlertId);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionCreated",
                columns: table => new
                {
                    AlertId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CancelUrl = table.Column<string>(nullable: true),
                    CheckoutId = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EventTime = table.Column<DateTime>(nullable: false),
                    MarketingConsent = table.Column<bool>(nullable: false),
                    NextBillDate = table.Column<DateTime>(nullable: false),
                    Passthrough = table.Column<string>(nullable: true),
                    Quantity = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Source = table.Column<string>(nullable: true),
                    SubscriptionId = table.Column<string>(nullable: true),
                    SubscriptionPlanId = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    UpdateUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionCreated", x => x.AlertId);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPaymentFailed",
                columns: table => new
                {
                    AlertId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<string>(nullable: true),
                    CancelUrl = table.Column<string>(nullable: true),
                    CheckoutId = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EventTime = table.Column<DateTime>(nullable: false),
                    MarketingConsent = table.Column<bool>(nullable: false),
                    NextRetryDate = table.Column<DateTime>(nullable: false),
                    Passthrough = table.Column<string>(nullable: true),
                    Quantity = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    SubscriptionId = table.Column<string>(nullable: true),
                    SubscriptionPlanId = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<string>(nullable: true),
                    UpdateUrl = table.Column<string>(nullable: true),
                    SubscriptionPaymentId = table.Column<string>(nullable: true),
                    Installments = table.Column<string>(nullable: true),
                    OrderId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    AttemptNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPaymentFailed", x => x.AlertId);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPaymentRefunded",
                columns: table => new
                {
                    AlertId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<string>(nullable: true),
                    BalanceCurrency = table.Column<string>(nullable: true),
                    BalanceEarningsDecrease = table.Column<string>(nullable: true),
                    BalanceFeeRefund = table.Column<string>(nullable: true),
                    BalanceGrossRefund = table.Column<string>(nullable: true),
                    BalanceTaxRefund = table.Column<string>(nullable: true),
                    CheckoutId = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    EarningsDecrease = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EventTime = table.Column<DateTime>(nullable: false),
                    FeeRefund = table.Column<string>(nullable: true),
                    GrossRefund = table.Column<string>(nullable: true),
                    InitialPayment = table.Column<string>(nullable: true),
                    Instalments = table.Column<string>(nullable: true),
                    MarketingConsent = table.Column<bool>(nullable: false),
                    OrderId = table.Column<string>(nullable: true),
                    Passthrough = table.Column<string>(nullable: true),
                    Quantity = table.Column<string>(nullable: true),
                    RefundReason = table.Column<string>(nullable: true),
                    RefundType = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    SubscriptionId = table.Column<string>(nullable: true),
                    SubscriptionPaymentId = table.Column<string>(nullable: true),
                    SubscriptionPlanId = table.Column<string>(nullable: true),
                    TaxRefund = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPaymentRefunded", x => x.AlertId);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPaymentSucceeded",
                columns: table => new
                {
                    AlertId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BalanceCurrency = table.Column<string>(nullable: true),
                    BalanceEarnings = table.Column<string>(nullable: true),
                    BalanceFee = table.Column<string>(nullable: true),
                    BalanceGross = table.Column<string>(nullable: true),
                    BalanceTax = table.Column<string>(nullable: true),
                    CheckoutId = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Coupon = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    Earnings = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EventTime = table.Column<DateTime>(nullable: false),
                    Fee = table.Column<string>(nullable: true),
                    InitialPayment = table.Column<string>(nullable: true),
                    Instalments = table.Column<string>(nullable: true),
                    MarketingConsent = table.Column<bool>(nullable: false),
                    NextBillDate = table.Column<DateTime>(nullable: false),
                    NextPaymentAmount = table.Column<string>(nullable: true),
                    OrderId = table.Column<string>(nullable: true),
                    Passthrough = table.Column<string>(nullable: true),
                    PaymentTax = table.Column<string>(nullable: true),
                    PlanName = table.Column<string>(nullable: true),
                    Quantity = table.Column<string>(nullable: true),
                    ReceiptUrl = table.Column<string>(nullable: true),
                    SaleGross = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    SubscriptionId = table.Column<string>(nullable: true),
                    SubscriptionPaymentId = table.Column<string>(nullable: true),
                    SubscriptionPlanId = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPaymentSucceeded", x => x.AlertId);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionUpdated",
                columns: table => new
                {
                    AlertId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CancelUrl = table.Column<string>(nullable: true),
                    CheckoutId = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EventTime = table.Column<DateTime>(nullable: false),
                    MarketingConsent = table.Column<bool>(nullable: false),
                    Passthrough = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    UpdateUrl = table.Column<string>(nullable: true),
                    SubscriptionId = table.Column<string>(nullable: true),
                    PausedAt = table.Column<DateTime>(nullable: false),
                    PausedFrom = table.Column<DateTime>(nullable: false),
                    NewPrice = table.Column<string>(nullable: true),
                    OldPrice = table.Column<string>(nullable: true),
                    NewQuantity = table.Column<string>(nullable: true),
                    OldQuantity = table.Column<string>(nullable: true),
                    NewUnitPrice = table.Column<string>(nullable: true),
                    OldUnitPrice = table.Column<string>(nullable: true),
                    NextBillDate = table.Column<DateTime>(nullable: false),
                    OldNextBillDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    OldStatus = table.Column<string>(nullable: true),
                    SubscriptionPlanId = table.Column<string>(nullable: true),
                    OldSubscriptionPlanId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionUpdated", x => x.AlertId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaddleUser");

            migrationBuilder.DropTable(
                name: "SubscriptionCancelled");

            migrationBuilder.DropTable(
                name: "SubscriptionCreated");

            migrationBuilder.DropTable(
                name: "SubscriptionPaymentFailed");

            migrationBuilder.DropTable(
                name: "SubscriptionPaymentRefunded");

            migrationBuilder.DropTable(
                name: "SubscriptionPaymentSucceeded");

            migrationBuilder.DropTable(
                name: "SubscriptionUpdated");
        }
    }
}
