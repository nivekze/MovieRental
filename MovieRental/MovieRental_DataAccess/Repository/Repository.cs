using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using Microsoft.EntityFrameworkCore;

using MovieRental_Infrastructure;
using MovieRental_DataAccess.Context;

namespace MovieRental_DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected readonly MovieRentalContext _context;

        public Repository(MovieRentalContext context)
        {
            _context = context;
            _context.ChangeTracker.LazyLoadingEnabled = false;
        }

        protected void Save() => _context.SaveChanges();

        public int Count(Func<T, bool> predicate)
        {
            return _context.Set<T>().Where(predicate).Count();
        }

        public void Create(T entity)
        {
            _context.Add(entity);
            Save();
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
            Save();
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            Save();
        }
    }
}
