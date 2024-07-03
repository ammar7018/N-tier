using Microsoft.AspNetCore.Mvc.Rendering;
using N_tier.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_tier.Models.ViewModels
{
    public class ProductVM
    {
        public Product product { get; set; }

        public IEnumerable<SelectListItem>?CategoryList { get; set; }
    }
}
