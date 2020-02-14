using Database;
using Entities.Models.Paddle;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Payment
{
    public class PaddleUserMananger
    {
        private readonly ApplicationContext _applicationContext;

        private DbSet<PaddleUser> Context => _applicationContext.PaddleUser;

        public PaddleUserMananger(ApplicationContext applicationContext)
        {
            this._applicationContext = applicationContext;
        }

        public async Task<PaddleUser> GetUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddUserAsync(PaddleUser userInfo)
        {
            await Context.AddAsync(userInfo);
            return (await _applicationContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> UpdateUserAsync(PaddleUser userInfo)
        {
            // Get the existing user by the UserId
            PaddleUser existingUser = await Context.SingleAsync(
                x => x.ApplicationUserId == userInfo.ApplicationUserId);

            // Iterate over each property and assign each field in the
            // existing user -  Except the PK (Id)
            foreach (var prop in userInfo.GetType().GetProperties())
            {
                if (prop.Name == "Id")
                    continue;

                prop.SetValue(existingUser, prop.GetValue(userInfo));

            }
            //Context.Update(existingUser);

            return (await _applicationContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> RemoveUserAsync(PaddleUser userInfo)
        {
            Context.Remove(userInfo);
            return (await _applicationContext.SaveChangesAsync()) > 0;
        }
    }
}
