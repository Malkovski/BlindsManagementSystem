namespace Blinds.Data.Repositories
{
    using System.Linq;

    using Blinds.Data.Models;
    using Microsoft.Practices.Unity;

    public class OrderRepository : BaseRepository<Order>
    {
        [InjectionConstructor]
        public OrderRepository(IBlindsDbContext context)
            : base(context)
        {
        }

        public IQueryable<Order> GetAll()
        {
            return this.All();
        }

        public IQueryable<Order> GetActive()
        {
            return this.All().Where(r => r.Deleted == false);
        }

        public IQueryable<Order> GetByUserId(string userId)
        {
            return this.GetActive().Where(o => o.UserId == userId);
        }

        public bool GetIfExists(int id)
        {
            return this.GetActive().Where(o => o.Id == id).Any();
        }
    }
}
