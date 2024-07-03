using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using N_tier.Models.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace N_tier.Models
{
    /*    public class Category
        {
            public int ID { get; set; }
            public string Name { get; set; }
            [DisplayName("Display Order")]
            [Range(1,100,ErrorMessage="Enter valid Range")]
            public int DisplayOrder { get; set; }
    *//*        [ValidateNever]
            public List<Product>? products { get; set; }
    *//*    }
    */

    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1-100")]
        public int DisplayOrder { get; set; }
    }


}
