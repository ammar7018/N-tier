using Microsoft.EntityFrameworkCore;
using N_tier.Data.Repository.IRepository;
using N_tier.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_tier.Data.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>,IApplicationUserRepository
    {
        AppDbContext _db;
        DbSet<ApplicationUser> _dbSet;
        public ApplicationUserRepository(AppDbContext db) : base(db)
        {
            _db = db;
            _dbSet = db.ApplicationUsers;
        }
    }
}
