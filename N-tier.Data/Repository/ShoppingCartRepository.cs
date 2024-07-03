using Microsoft.EntityFrameworkCore;
using N_tier.Data.Repository.IRepository;
using N_tier.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace N_tier.Data.Repository
{
    public class ShoppingCartRepository :  Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly AppDbContext _appDbContext;

        private DbSet<ShoppingCart> _dbSet;
        
        public ShoppingCartRepository(AppDbContext db) : base(db)
        {
            _appDbContext = db;
            _dbSet = db.ShopingCarts;

        }

        public void Update(ShoppingCart obj)
        {
            _dbSet.Update(obj);
        }
    }
}
