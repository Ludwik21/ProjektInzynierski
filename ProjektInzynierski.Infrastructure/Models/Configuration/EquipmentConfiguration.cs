using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjektInzynierski.Infrastructure.Models.Configuration
{
    public class EquipmentConfiguration : IEntityTypeConfiguration<EquipmentDao>
    {
        public void Configure(EntityTypeBuilder<EquipmentDao> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
