using Microsoft.EntityFrameworkCore;
using N_tier.Data.Repository.IRepository;
using N_tier.Models;
using N_tier.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace N_tier.Data.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private  AppDbContext _db;
        private DbSet<OrderDetail> _dbSet;
        public OrderDetailRepository(AppDbContext db) : base(db)
        {
            _db = db;
            _dbSet = _db.OrderDetails;
        }


        public void Update(OrderDetail obj)
        {
            _dbSet.Update(obj);
        }
    }
}
