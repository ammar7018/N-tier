using N_tier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_tier.Data.Repository.IRepository
{
    public interface ICategoryRepository :IRepository<Category>
    {
        void Update(Category obj);
    }
}
