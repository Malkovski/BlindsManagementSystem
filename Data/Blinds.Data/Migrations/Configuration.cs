namespace Blinds.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Common;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public sealed class Configuration : DbMigrationsConfiguration<BlindsDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(BlindsDbContext context)
        {
            this.SeedRoles(context);
            this.SeedUsers(context);
        }

        private void SeedUsers(BlindsDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var store = new UserStore<User>(context);
            var manager = new UserManager<User>(store);
            var user = new User { UserName = GlobalConstants.AdministratorRoleName };

            manager.Create(user, GlobalConstants.InitialPassword);
            manager.AddToRole(user.Id, GlobalConstants.AdministratorRoleName);

            context.SaveChanges();
        }

        private void SeedRoles(BlindsDbContext context)
        {
            if (context.Roles.Any())
            {
                return;
            }

            context.Roles.Add(new IdentityRole { Name = GlobalConstants.AdministratorRoleName });
            context.SaveChanges();
        }
    }
}
