using Database;
using Entities.Models;
using Entities.Models.Paddle;
using Entities.Models.Paddle.Alerts;
using MenterInterfaceTests;
using MentorInterface.Helpers;
using MentorInterface.Paddle;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MentorInterfaceTests.Paddle
{
    [TestClass]
    public class PaddleEndpointTests
    {
        private readonly DbContextOptions<ApplicationContext> applicationContextOptions;
        private readonly ServiceProvider serviceProvider;

        public PaddleEndpointTests()
        {
            var services = new ServiceCollection();

            services.AddControllers()
                // Serialize JSON using the Member CASE!
                .AddNewtonsoftJson(x => x.UseMemberCasing());
            services.AddApiVersioning();

            services.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<ApplicationContext>((serviceProvider, options) =>
                {
                    // Use a GUID to ensure a new Database is used for each test.
                    options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).UseInternalServiceProvider(serviceProvider);
                });

            services.AddLogging(services =>
            {
                services.AddConsole();
                services.AddDebug();
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationContext>();

            // Mock IWebhookVerifier to skip verification in tests
            var webhookVerifierMock = new Mock<IWebhookVerifier>();
            webhookVerifierMock.Setup(x => x.IsAlertValid(It.IsAny<Dictionary<string, string>>()))
                .Returns(true);

            serviceProvider = services.BuildServiceProvider();
        }

        [TestMethod]
        public void CreateSubscriptionTest()
        {
            // ARRANGE
            // ... create vars
            var steamId = 999;
            var subscriptionPlanId = 1;
            // ... create roles
            var roles = new string[] { Subscriptions.Premium };
            RoleCreator.CreateRoles(serviceProvider, roles);
            // ... create paddlePlans
            var paddlePlans = new List<PaddlePlan> {
                new PaddlePlan(subscriptionPlanId, Entities.SubscriptionType.Premium, 1, 2.99)
            };
            PaddlePlanManager.SetPaddlePlans(serviceProvider, paddlePlans);
            // ... create user
            var user = ApplicationUserFactory.FromSteamId(steamId);
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            userManager.CreateAsync(user);
            // ... create paddle request
            var requestParams = new Dictionary<string, string>
            {
                {"subscription_plan_id", subscriptionPlanId.ToString() }
                // TODO: Add passthrough
            };
            var requestJson = TestDataHelper.GetPaddleFormData("subscription_created", requestParams);

            // TODO: Finish writing test. Add other tests.

            // ACT
            // ... send paddle request to controller

            // ASSERT
            // ... verify that user now exists
            // 
        }
    }
}
