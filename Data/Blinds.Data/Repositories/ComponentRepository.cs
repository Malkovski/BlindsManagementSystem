namespace Blinds.Data.Repositories
{
    using System.Linq;

    using Blinds.Data.Models;
    using Microsoft.Practices.Unity;
    
    public class ComponentRepository : BaseRepository<Component>
    {
        [InjectionConstructor]
        public ComponentRepository(IBlindsDbContext context) : base(context)
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
    }
}
