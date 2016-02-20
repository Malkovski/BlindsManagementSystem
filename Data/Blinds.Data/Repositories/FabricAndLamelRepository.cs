namespace Blinds.Data.Repositories
{
    using System.Linq;

    using Blinds.Data.Models;
    using Microsoft.Practices.Unity;
    using Models.Enumerations;

    public class FabricAndLamelRepository : BaseRepository<FabricAndLamel>
    {
        [InjectionConstructor]
        public FabricAndLamelRepository(IBlindsDbContext context)
            : base(context)
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

        public bool GetIfExists(int blindTypeId, Color color, Material material, int fabricAndLamelsId)
        {
            return this.All().Where(x => x.BlindTypeId == blindTypeId && x.Color == color && x.Material == material && x.Id != fabricAndLamelsId).Any();
        }

        public FabricAndLamel Get(int blindTypeId, Color color, Material material)
        {
            return this.All().Where(x => x.BlindTypeId == blindTypeId && x.Color == color && x.Material == material).FirstOrDefault();
        }
    }
}
