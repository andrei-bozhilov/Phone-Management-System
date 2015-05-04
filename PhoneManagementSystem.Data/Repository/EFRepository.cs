
namespace PhoneManagementSystem.Data.Repository
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    public class EFRepository<T> : IRepository<T> where T : class
    {
        private DbContext context;
        private DbSet<T> set;

        public EFRepository(DbContext context)
        {
            this.context = context;
            this.set = context.Set<T>();
        }
        public IQueryable<T> All()
        {
            return this.set;
        }
        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            return this.set.Where(expression);
        }

        public T GetById(object id)
        {
            return this.set.Find(id);
        }

        public void Add(T entity)
        {
            this.ChangeState(entity, EntityState.Added);
        }

        public void AddRange(IQueryable<T> entities)
        {
            this.set.AddRange(entities);
        }

        public void Update(T entity)
        {
            this.ChangeState(entity, EntityState.Modified);
        }

        public T Delete(T entity)
        {
            this.ChangeState(entity, EntityState.Deleted);
            return entity;
        }

        public T Delete(object id)
        {
            var entity = this.GetById(id);
            this.Delete(entity);
            return entity;
        }

        public void DeleteRange(IQueryable<T> entities)
        {
            this.set.RemoveRange(entities);
        }
        public void Detach(T entity)
        {
            ChangeState(entity, EntityState.Detached);
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private void ChangeState(T entity, EntityState state)
        {
            var entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.set.Attach(entity);
            }

            entry.State = state;
        }
    }
}
