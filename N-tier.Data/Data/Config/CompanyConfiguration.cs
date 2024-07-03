using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using N_tier.Models;
using N_tier.Models.Models;


namespace N_tier.Data.Config
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    { 
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x=>x.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.HasData(GetCompanies());
        }

        private static List<Company> GetCompanies()
        {
            return new List<Company>()
            {
                new Company { Id = 1, Name = "Tech Solution", StreetAddress="123 Tech St", City="Tech City",
                                PostalCode="12121", State="IL", PhoneNumber="6669990000"},
                new Company {
                    Id = 2,
                    Name = "Vivid Books",
                    StreetAddress = "999 Vid St",
                    City = "Vid City",
                    PostalCode = "66666",
                    State = "IL",
                    PhoneNumber = "7779990000"
                },
                new Company {
                    Id = 3,
                    Name = "Readers Club",
                    StreetAddress = "999 Main St",
                    City = "Lala land",
                    PostalCode = "99999",
                    State = "NY",
                    PhoneNumber = "1113335555"
                }
            };
        }


    }
}
