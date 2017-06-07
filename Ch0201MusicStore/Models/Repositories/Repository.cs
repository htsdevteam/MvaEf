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

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

    }
}