namespace Blinds.Data.Repositories
{
    using System.Linq;

    using Blinds.Data.Models;
    using Microsoft.Practices.Unity;
    using Models.Enumerations;

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

        public bool GetIfExists(int blindTypeId, Color color, int fabricAndLamelsId)
        {
            return this.All().Where(x => x.BlindTypeId == blindTypeId && x.Color == color && x.Id != fabricAndLamelsId).Any();
        }
    }
}
