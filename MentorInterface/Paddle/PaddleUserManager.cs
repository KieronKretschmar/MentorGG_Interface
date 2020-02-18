using Database;
using Entities.Models.Paddle;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Paddle
{
    /// <summary>
    /// Manages PaddleUsers in the DatabaseContext
    /// </summary>
    public class PaddleUserManager
    {
        private readonly ApplicationContext _applicationContext;

        private DbSet<PaddleUser> Context => _applicationContext.PaddleUser;

        /// <summary>
        /// Stores a reference to the DatabaseContext
        /// </summary>
        /// <param name="applicationContext"></param>
        public PaddleUserManager(ApplicationContext applicationContext)
        {
            this._applicationContext = applicationContext;
        }

        /// <summary>
        /// Return a PaddleUser
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<PaddleUser> GetUserAsync(int userId)
        {
            return await Context.SingleOrDefaultAsync<PaddleUser>(x => x.UserId == userId);
        }

        /// <summary>
        /// Add a PaddleUser
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public async Task<bool> AddUserAsync(PaddleUser userInfo)
        {
            await Context.AddAsync(userInfo);
            return (await _applicationContext.SaveChangesAsync()) > 0;
        }

        /// <summary>
        /// Retreive an existing PaddleUser and update.
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUserAsync(PaddleUser userInfo)
        {
            // Get the existing user by the UserId
            PaddleUser existingUser = await Context.SingleAsync(
                x => x.UserId == userInfo.UserId);

            // Iterate over each property and assign each field in the
            // existing user -  Except the PK (Id)
            foreach (var prop in userInfo.GetType().GetProperties())
            {
                if (prop.Name == "Id")
                    continue;

                prop.SetValue(existingUser, prop.GetValue(userInfo));

            }

            return (await _applicationContext.SaveChangesAsync()) > 0;
        }

        /// <summary>
        /// Remove a PaddleUser.
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public async Task<bool> RemoveUserAsync(int userId)
        {
            Context.Remove(await GetUserAsync(userId));
            return (await _applicationContext.SaveChangesAsync()) > 0;
        }
    }
}
