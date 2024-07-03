using Microsoft.EntityFrameworkCore;
using N_tier.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace N_tier.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly AppDbContext _db;
        private DbSet<T> _dbSet;
        public Repository(AppDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();

        }
        public void Add(T obj)
        {
            _dbSet.Add(obj);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null, string? incudeProp=null, bool tracked = false)
        {
            IQueryable<T> lst;

            if (tracked==true)
            {
                lst = _dbSet;
            }
            else
            {
                lst = _dbSet.AsNoTracking();
            }

            if (filter != null)
            {
                lst=lst.Where(filter);
            }

            if (!string.IsNullOrEmpty(incudeProp))
            {

                foreach (var prop in incudeProp.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    lst = lst.Include(prop);
                }
            }
            return lst.ToList();
        }

        public T Get(Expression<Func<T, bool>> filter, string? incudeProp = null,bool tracked=false)
        {

            IQueryable<T> val = _dbSet;

            if (tracked)
            {
                val = _dbSet;
            }
            else
            {
                val = _dbSet.AsNoTracking();
            }

            if (!string.IsNullOrEmpty(incudeProp))
            {

                foreach (var prop in incudeProp.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    val = val.Include(prop);
                }
            }

            val = val.Where(filter);

            return val.FirstOrDefault();

        }

        public void Remove(T obj)
        {
            _dbSet.Remove(obj);
        }

        public void RemoveRange(IEnumerable<T> obj)
        {
            _dbSet.RemoveRange(obj);
        }
    }
}
