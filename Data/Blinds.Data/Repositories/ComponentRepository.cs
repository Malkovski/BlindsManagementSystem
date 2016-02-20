namespace Blinds.Data.Repositories
{
    using System.Linq;

    using Blinds.Data.Models;
    using Microsoft.Practices.Unity;

    public class ComponentRepository : BaseRepository<Component>
    {
        [InjectionConstructor]
        public ComponentRepository(IBlindsDbContext context)
            : base(context)
        {
        }

        public IQueryable<Component> GetAll()
        {
            return this.All();
        }

        public IQueryable<Component> GetActive()
        {
            return this.All().Where(r => r.Deleted == false);
        }

        public bool GetIfExists(int blindTypeId, string componentName, int componentId)
        {
             return this.All().Where(x => x.BlindTypeId == blindTypeId && x.Name == componentName && x.Id != componentId).Any();
        }

        public IQueryable<Component> GetByBlindType(int blindTypeId)
        {
            return this.All().Where(c => c.BlindTypeId == blindTypeId);
        }
    }
}
