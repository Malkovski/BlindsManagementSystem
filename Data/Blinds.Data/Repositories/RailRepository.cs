namespace Blinds.Data.Repositories
{
    using System.Linq;

    using Blinds.Data.Models;
    using Microsoft.Practices.Unity;
    using Models.Enumerations;

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

        public bool GetIfExists(int blindTypeId, Color color, int railId)
        {
            return this.All().Where(r => r.BlindTypeId == blindTypeId && r.Color == color && r.Id != railId).Any();
        }
    }
}
