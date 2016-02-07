namespace Blinds.Data.Repositories
{
    using System.Linq;

    using Blinds.Data.Models;
    using Microsoft.Practices.Unity;

    public class FabricAndLamelRepository : BaseRepository<FabricAndLamel>
    {
        [InjectionConstructor]
        public FabricAndLamelRepository(IBlindsDbContext context) : base(context)
        {
        }

        public IQueryable<FabricAndLamel> GetAll()
        {
            return this.All();
        }

        public IQueryable<FabricAndLamel> GetActive()
        {
            return this.All().Where(r => r.Deleted == false);
        }
    }
}
