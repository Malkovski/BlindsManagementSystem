namespace Blinds.Data.Repositories
{
    using Microsoft.Practices.Unity;
    using Models;
    using System.Linq;

    public class PictureRepository : BaseRepository<Picture>
    {
        [InjectionConstructor]
        public PictureRepository(IBlindsDbContext context)
            : base(context)
        {
        }

        public IQueryable<Picture> GetAll()
        {
            return this.All();
        }

        public IQueryable<Picture> GetActive()
        {
            return this.All().Where(bt => bt.Deleted == false);
        }
    }
}
