using Database;
using Entities.Models.Paddle;
using MentorInterface.Paddle;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MentorInterfaceTests.Paddle
{
    [TestClass]
    public class PaddleUserManagerTest
    {

        private readonly DbContextOptions<ApplicationContext> applicationContextOptions;

        public PaddleUserManagerTest()
        {
            // Use a GUID to ensure a new Database is used for each test.
            applicationContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        }

        [TestMethod]
        public void RemoveUserTest()
        {
            // ARRANGE
            int userId = 50;

            PaddleUser newUser = new PaddleUser
            {
                Email = "test@mentor.gg",
                UserId = userId,
                MarketingConsent = false,
                SubscriptionId = 999,
                SubscriptionPlanId = 999,
                Status = "active",
                CancelUrl = "x",
                UpdateUrl = "x",
                ApplicationUserId = 999
            };

            // ACT
            // Add the User
            using (var context = new ApplicationContext(applicationContextOptions))
            {
                var paddleUserMananger = new PaddleUserMananger(context);
                paddleUserMananger.AddUserAsync(newUser).Wait();
            }

            // Remove the User
            using (var context = new ApplicationContext(applicationContextOptions))
            {
                var paddleUserMananger = new PaddleUserMananger(context);
                paddleUserMananger.RemoveUserAsync(userId).Wait();
            }

            // ASSERT
            var ex = Assert.ThrowsException<AggregateException>(() =>
                {
                    using (var context = new ApplicationContext(applicationContextOptions))
                    {
                        var paddleUserMananger = new PaddleUserMananger(context);
                        paddleUserMananger.GetUserAsync(userId).Wait();
                    }
                }
            );

            Assert.IsTrue(ex.InnerException is InvalidOperationException);
        }

        [TestMethod]
        public void AddNewUserTest()
        {
            // ARANGE
            int userId = 50;

            PaddleUser newUser = new PaddleUser
            {
                Email = "test@mentor.gg",
                UserId = userId,
                MarketingConsent = false,
                SubscriptionId = 999,
                SubscriptionPlanId = 999,
                Status = "active",
                CancelUrl = "x",
                UpdateUrl = "x",
                ApplicationUserId = 999
            };

            // ACT
            // Add the User
            using (var context = new ApplicationContext(applicationContextOptions))
            {
                var paddleUserMananger = new PaddleUserMananger(context);
                paddleUserMananger.AddUserAsync(newUser).Wait();
            }

            // Get the User
            PaddleUser returnedUser;
            using (var context = new ApplicationContext(applicationContextOptions))
            {
                var paddleUserMananger = new PaddleUserMananger(context);
                returnedUser = paddleUserMananger.GetUserAsync(userId).Result;
            }

            // ASSERT
            Assert.AreEqual(newUser.SubscriptionPlanId, returnedUser.SubscriptionPlanId);
        }

        [TestMethod]
        public void UpdateUserSubscriptionPlanTest()
        {
            // ARANGE
            int userId = 100;
            int plan1 = 10;
            int plan2 = 20;


            PaddleUser newUser = new PaddleUser
            {
                Email = "test@mentor.gg",
                UserId = userId,
                MarketingConsent = false,
                SubscriptionId = 999,
                SubscriptionPlanId = plan1,
                Status = "active",
                CancelUrl = "x",
                UpdateUrl = "x",
                ApplicationUserId = 999
            };


            PaddleUser updatedUser = new PaddleUser
            {
                UserId = userId,
                SubscriptionPlanId = plan2,
            };


            // ACT

            // Add the User
            using (var context = new ApplicationContext(applicationContextOptions))
            {
                var paddleUserMananger = new PaddleUserMananger(context);
                paddleUserMananger.AddUserAsync(newUser).Wait();

            }

            // Update the User
            using (var context = new ApplicationContext(applicationContextOptions))
            {
                var paddleUserMananger = new PaddleUserMananger(context);
                paddleUserMananger.UpdateUserAsync(updatedUser).Wait();
            }

            // Get the User
            PaddleUser returnedUser;
            using (var context = new ApplicationContext(applicationContextOptions))
            {
                var paddleUserMananger = new PaddleUserMananger(context);
                returnedUser = paddleUserMananger.GetUserAsync(userId).Result;
            }

            // ASSERT
            Assert.AreEqual(returnedUser.SubscriptionPlanId, plan2);
        }
    }


}
