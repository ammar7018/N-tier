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
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private  AppDbContext _db;
        private DbSet<OrderHeader> _dbSet;
        public OrderHeaderRepository(AppDbContext db) : base(db)
        {
            _db = db;
            _dbSet = _db.OrderHeaders;
        }


        public void Update(OrderHeader obj)
        {
            _dbSet.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDB = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);

            if (orderFromDB != null)
            {
                orderFromDB.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDB.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
        {
            var orderFromDB = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            
            if (orderFromDB != null)
            {
                if (!string.IsNullOrEmpty(sessionId)){
                    orderFromDB.SessionId = sessionId;
                }

                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    orderFromDB.PaymentIntentId= paymentIntentId;
                    orderFromDB.PaymentDate = DateTime.Now;
                }
            }
        }
    }
}
