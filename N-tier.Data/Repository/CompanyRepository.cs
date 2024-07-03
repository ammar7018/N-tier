using Microsoft.EntityFrameworkCore;
using N_tier.Data.Repository.IRepository;
using N_tier.Models;
using N_tier.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_tier.Data.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private AppDbContext _db;
        private DbSet<Company> _dbSet;

        public CompanyRepository(AppDbContext db) 
            : base(db)
        {
            _db = db;
            _dbSet = _db.Companies;

        }

        void ICompanyRepository.Update(Company obj)
        {
            _dbSet.Update(obj);
        }
    }
}
