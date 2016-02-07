namespace Blinds.Data.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;

    using Blinds.Contracts;
    using Blinds.Data;  

    public class BaseRepository<T> : IRepository<T> where T : class, IDeletableEntity
    {
        protected readonly IBlindsDbContext Context;
        private readonly IDbSet<T> set;

        public BaseRepository(IBlindsDbContext context)
        {
            this.Context = context;
            this.set = context.Set<T>();
        }

        public IQueryable<T> All()
        {
            return this.set.AsQueryable().Where(s => !s.Deleted);
        }

        public virtual T GetById(object id)
        {
            return this.set.Find(id);
        }

        public IQueryable<T> AllWithDeleted()
        {
            return this.set.AsQueryable();
        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> conditions)
        {
            return this.All().Where(conditions);
        }

        public void Add(T entity)
        {
            var entry = this.AttachIfDetached(entity);
            entry.State = EntityState.Added;
        }

        public void Update(T entity)
        {
            var entry = this.AttachIfDetached(entity);
            entry.State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            var entry = this.Context.Entry(entity);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                this.set.Attach(entity);
                this.set.Remove(entity);
            }
        }

        public void Detach(T entity)
        {
            var entry = this.Context.Entry(entity);
            entry.State = EntityState.Detached;
        }

        public virtual int SaveChanges()
        {
            return this.Context.SaveChanges();
        }

        private DbEntityEntry AttachIfDetached(T entity)
        {
            var entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.set.Attach(entity);
            }

            return entry;
        }
    }
}
