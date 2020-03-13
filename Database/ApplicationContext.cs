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
        public DbSet<PaddlePlan> PaddlePlan { get; set; }
        public DbSet<PaddlePlanRole> PaddlePlanRole { get; set; }
        public DbSet<PaddleSubscription> PaddleSubscription { get; set; }

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

            // "Join"-table for many to many relationship between PaddlePlan and Role
            builder.Entity<PaddlePlanRole>(b =>
            {
                b.HasKey(ppr => new { ppr.PlanId, ppr.RoleId});

                b.HasOne(ppr => ppr.PaddlePlan)
                .WithMany(pp => pp.PaddlePlanRoles)
                .HasForeignKey(ppr => ppr.PlanId);

                b.HasOne(ppr => ppr.Role)
                .WithMany(r => r.PaddlePlanRoles)
                .HasForeignKey(ppr => ppr.RoleId);
            });

            builder.Entity<PaddlePlan>(b =>
            {
                b.HasKey(x => x.PlanId);
            });

            builder.Entity<PaddleSubscription>(b =>
            {
                b.HasKey(x => x.SubscriptionId);

                // n:1 relationship to PaddlePlan
                b.HasOne(x => x.PaddlePlan)
                .WithMany(x => x.Subscriptions)
                .HasForeignKey(x => x.SubscriptionPlanId)
                .IsRequired();

            });

            builder.Entity<SubscriptionCreated>(b => b.HasKey(x => x.AlertId));
            builder.Entity<SubscriptionUpdated>(b => b.HasKey(x => x.AlertId));
            builder.Entity<SubscriptionCancelled>(b => b.HasKey(x => x.AlertId));

            builder.Entity<SubscriptionPaymentFailed>(b => b.HasKey(x => x.AlertId));
            builder.Entity<SubscriptionPaymentRefunded>(b => b.HasKey(x => x.AlertId));
            builder.Entity<SubscriptionPaymentSucceeded>(b => b.HasKey(x => x.AlertId));
        }
    }
}
