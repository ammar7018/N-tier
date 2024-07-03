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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _appDbContext;

        private DbSet<Product> _dbSet;
        public ProductRepository(AppDbContext db) : base(db)
        {
            _appDbContext = db;
            _dbSet = db.Product;
        }

        public void Update(Product obj)
        {
            var objFromDb = _appDbContext.Product.FirstOrDefault(x => obj.Id == x.Id);

            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Price100 = obj.Price100;
                objFromDb.Description = obj.Description;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.Author = obj.Author;
                if (obj.imageUrl != null)
                {
                    objFromDb.imageUrl = obj.imageUrl;
                }

            }
        }
    }
}
