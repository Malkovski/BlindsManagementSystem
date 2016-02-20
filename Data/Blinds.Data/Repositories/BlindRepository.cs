namespace Blinds.Data.Repositories
{
    using System.Linq;

    using Blinds.Data.Models;
    using Microsoft.Practices.Unity;

    public class BlindRepository : BaseRepository<Blind>
    {
        [InjectionConstructor]
        public BlindRepository(IBlindsDbContext context)
            : base(context)
        {
        }

        public IQueryable<Blind> GetAll()
        {
            return this.All();
        }

        public IQueryable<Blind> GetActive()
        {
            return this.All().Where(r => r.Deleted == false);

        }
    }
}
