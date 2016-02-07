namespace Blinds.Data.Repositories
{
    using System.Linq;

    using Blinds.Data.Models;

    public class BlindRepository : BaseRepository<Blind>
    {
        public BlindRepository(IBlindsDbContext context) 
            : base(context)
        {
        }
    }
}
