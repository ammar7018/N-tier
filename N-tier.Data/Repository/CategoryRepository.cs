using Microsoft.EntityFrameworkCore;
using N_tier.Data.Repository.IRepository;
using N_tier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace N_tier.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private  AppDbContext _db;
        private DbSet<Category> _dbSet;
        public CategoryRepository(AppDbContext db) : base(db)
        {
            _db = db;
            _dbSet = _db.Category;
        }


        public void Update(Category obj)
        {
            _dbSet.Update(obj);
        }
    }
}
