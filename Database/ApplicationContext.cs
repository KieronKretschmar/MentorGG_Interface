using Entities.Models;
using Entities.Models.Paddle;
using Entities.Models.Paddle.Alerts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Database
{
    /// <summary>
    ///
    /// </summary>
    public class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        #region Paddle
        public DbSet<PaddleUser> PaddleUser { get; set; }
        public DbSet<PaddlePlan> PaddlePlan { get; set; }

        public DbSet<SubscriptionCreated> SubscriptionCreated { get; set; }
        public DbSet<SubscriptionUpdated> SubscriptionUpdated { get; set; }
        public DbSet<SubscriptionCancelled> SubscriptionCancelled { get; set; }

        public DbSet<SubscriptionPaymentSucceeded> SubscriptionPaymentSucceeded { get; set; }
        public DbSet<SubscriptionPaymentFailed> SubscriptionPaymentFailed { get; set; }
        public DbSet<SubscriptionPaymentRefunded> SubscriptionPaymentRefunded { get; set; }

        #endregion


        /// <summary>
        ///
        /// </summary>
        /// <param name="options"></param>
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Table names
            // Where <int> is the <ApplicationUser> Primary Key
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");

            builder.Entity<ApplicationRole>().ToTable("Roles");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");


            // Required Fields
            builder.Entity<ApplicationUser>(b =>
                {
                    b.Property(x => x.SteamId).IsRequired();
                    b.Property(x => x.Registration).IsRequired();
                }
            );

            // Paddle
            builder.Entity<PaddleUser>(b =>
                {
                    b.Property(x => x.Id).ValueGeneratedOnAdd();

                    // A PaddleUser can have ONE ApplicationUser
                    // An ApplicationUser can have MANY PaddleUsers
                    b.HasOne(x => x.User)
                        .WithMany(x => x.PaddleUser)
                        .HasForeignKey(x => x.SteamId)
                        .HasPrincipalKey(x => x.SteamId)
                        .IsRequired();
                }
            );

            builder.Entity<PaddlePlan>(b =>
                {
                    b.HasKey(x => x.PlanId);

                    // A PaddlePlan can have ONE ApplicationRole
                    // An ApplicationRole can have MANY PaddlePlanss
                    b.HasOne(x => x.Role)
                        .WithMany(x => x.PaddlePlan)
                        .HasForeignKey(x => x.RoleId)
                        .IsRequired();
                }
            );

            builder.Entity<SubscriptionCreated>(b => b.HasKey(x => x.AlertId));
            builder.Entity<SubscriptionUpdated>(b => b.HasKey(x => x.AlertId));
            builder.Entity<SubscriptionCancelled>(b => b.HasKey(x => x.AlertId));

            builder.Entity<SubscriptionPaymentFailed>(b => b.HasKey(x => x.AlertId));
            builder.Entity<SubscriptionPaymentRefunded>(b => b.HasKey(x => x.AlertId));
            builder.Entity<SubscriptionPaymentSucceeded>(b => b.HasKey(x => x.AlertId));
        }

        public void UpdatePaddleUser(PaddleUser user)
        {
            // Get the existing user by the UserId
            PaddleUser existingUser = PaddleUser.Single(
                x => x.UserId == user.UserId);

            // Iterate over each property and assign each field in the
            // existing user -  Except the PK (Id)
            foreach (var prop in user.GetType().GetProperties())
            {
                if (prop.Name == "Id")
                    continue;

                prop.SetValue(existingUser, prop.GetValue(user));
            }

            SaveChanges();
        }
    }
}
