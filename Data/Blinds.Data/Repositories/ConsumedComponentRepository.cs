namespace Blinds.Data.Repositories
{
    using System.Linq;

    using Blinds.Data.Models;
    using Microsoft.Practices.Unity;

    public class ConsumedComponentRepository : BaseRepository<ConsumedComponent>
    {
        [InjectionConstructor]
        public ConsumedComponentRepository(IBlindsDbContext context) : base(context)
        {
        }

        public IQueryable<ConsumedComponent> GetAll()
        {
            return this.All();
        }

        public IQueryable<ConsumedComponent> GetActive()
        {
            return this.All().Where(r => r.Deleted == false);
        }
    }
}
