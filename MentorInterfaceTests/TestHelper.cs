using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MenterInterfaceTests
{
    static class TestHelper
    {
        /// <summary>
        /// Returns (unconfigured) Mock of UserManager.
        /// </summary>
        /// <returns></returns>
        public static Mock<UserManager<ApplicationUser>> GetUserManagerMock()
        {
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                    new Mock<IUserStore<ApplicationUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<ApplicationUser>>().Object,
                    new IUserValidator<ApplicationUser>[0],
                    new IPasswordValidator<ApplicationUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            return mockUserManager;


            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object);
            userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(() => null);
            return userManagerMock;
        }
    }
}
