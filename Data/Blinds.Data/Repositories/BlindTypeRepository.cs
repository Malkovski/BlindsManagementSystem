namespace Blinds.Data.Repositories
{
    using System.Linq;

    using Blinds.Data.Models;
    using Microsoft.Practices.Unity;  

    public class BlindTypeRepository : BaseRepository<BlindType>
    {
        [InjectionConstructor]
        public BlindTypeRepository(IBlindsDbContext context) : base(context)
        {
        }

        public IQueryable<BlindType> GetAll()
        {
            return this.All();
        }

        public IQueryable<BlindType> GetActive()
        {
            return this.All().Where(bt => bt.Deleted == false);
        }

        public bool GetIfExists(string name, int id)
        {
            return this.All().Where(bt => bt.Name == name && bt.Id != id).Any();
        }
    }
}
