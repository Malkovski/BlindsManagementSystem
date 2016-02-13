namespace Blinds.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    using Microsoft.AspNet.Identity.EntityFramework;
    using Migrations;
    using Models;

    public class BlindsDbContext : IdentityDbContext<User>, IBlindsDbContext
    {
        public BlindsDbContext()
            : base("BlindsConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BlindsDbContext, Configuration>());
        }

        public IDbSet<Blind> Blinds { get; set; }

        public IDbSet<BlindType> BlindTypes { get; set; }

        public IDbSet<Component> Components { get; set; }

        public IDbSet<ConsumedMaterials> ConsumedMaterials { get; set; }

        public IDbSet<ConsumedComponent> ConsumedComponent { get; set; }

        public IDbSet<FabricAndLamel> FabricAndLamels { get; set; }

        public IDbSet<Order> Orders { get; set; }

        public IDbSet<Rail> Rails { get; set; }

        public IDbSet<Picture> Pictures { get; set; }

        public static BlindsDbContext Create()
        {
            return new BlindsDbContext();
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public new void Dispose()
        {
            base.Dispose();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //modelBuilder.Entity<Blind>()
            //    .HasKey(t => t.Id);

            //modelBuilder.Entity<ConsumedMaterials>()
            //    .HasRequired(t => t.Blind)
            //    .WithRequiredPrincipal(t => t.ConsumedMaterials);
        }
    }
}
