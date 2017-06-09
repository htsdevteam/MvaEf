using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Ch0201MusicStore.Models.Repositories
{
    public class Repository<T> where T : class
    {
        private readonly MusicStoreDbContext _context;
        private bool _isDisposed;

        public Repository() : this(new MusicStoreDbContext())
        {
        }

        public Repository(MusicStoreDbContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }

        protected DbSet<T> DbSet { get; set; }

        public List<T> GetAll()
        {
            return DbSet.ToList();
        }

        public T Get(int id)
        {
            return DbSet.Find(id);
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        //public void Update(T entity)
        // virtual for the self-made concurrency management
        public virtual void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            DbSet.Remove(DbSet.Find(id));
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _context.Dispose();
                _isDisposed = true;
            }
        }
    }
}