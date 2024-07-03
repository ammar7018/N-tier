using N_tier.Models;
using N_tier.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_tier.Data.Repository.IRepository
{
    public interface ICompanyRepository:IRepository<Company>
    {
        void Update(Company obj);

    }
}
