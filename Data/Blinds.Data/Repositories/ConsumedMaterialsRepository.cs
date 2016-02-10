namespace Blinds.Data.Repositories
{
    using System.Linq;

    using Blinds.Data.Models;
    using Microsoft.Practices.Unity;

    public class ConsumedMaterialsRepository : BaseRepository<ConsumedMaterials>
    {
        [InjectionConstructor]
        public ConsumedMaterialsRepository(IBlindsDbContext context) : base(context)
        {
        }

        public IQueryable<ConsumedMaterials> GetAll()
        {
            return this.All();
        }

        public IQueryable<ConsumedMaterials> GetActive()
        {
            return this.All().Where(r => r.Deleted == false);
        }
    }
}
