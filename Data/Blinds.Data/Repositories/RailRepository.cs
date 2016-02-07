namespace Blinds.Data.Repositories
{
    using System.Linq;

    using Blinds.Data.Models;
    using Microsoft.Practices.Unity;
    
    public class RailRepository : BaseRepository<Rail>
    {
        [InjectionConstructor]
        public RailRepository(IBlindsDbContext context) : base(context)
        {
        }

        public IQueryable<Rail> GetAll()
        {
            return this.All();
        }

        public IQueryable<Rail> GetActive()
        {
            return this.All().Where(r => r.Deleted == false);
        }
    }
}
