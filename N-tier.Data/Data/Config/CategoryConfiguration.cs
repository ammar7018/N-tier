using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using N_tier.Models;


namespace N_tier.Data.Config
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    { 
        public void Configure(EntityTypeBuilder<Category> builder)
        {
/*            builder.HasKey(x => x.Id);

            builder.Property(x=>x.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.DisplayOrder);
*/
            builder.HasData(GetCategories());
        } 

        private static List<Category> GetCategories()
        {
            return new List<Category>()
            {
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }
            };
        }
    }
}
