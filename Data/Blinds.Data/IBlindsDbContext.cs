namespace Blinds.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using Blinds.Data.Models;

    public interface IBlindsDbContext
    {
        IDbSet<Blind> Blinds { get; set; }

        IDbSet<BlindType> BlindTypes { get; set; }

        IDbSet<Component> Components { get; set; }

        IDbSet<FabricAndLamel> FabricAndLamels { get; set; }

        IDbSet<Order> Orders { get; set; }

        IDbSet<Rail> Rails { get; set; }

        IDbSet<T> Set<T>() where T : class;

        DbEntityEntry<T> Entry<T>(T entity) where T : class;

        int SaveChanges();

        void Dispose();
    }
}
