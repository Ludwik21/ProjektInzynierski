using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjektInzynierski.Infrastructure.Models.Configuration
{
    public class EquipmentCompatibilityConfiguration : IEntityTypeConfiguration<EquipmentCompatibility>
    {
        public void Configure(EntityTypeBuilder<EquipmentCompatibility> builder)
        {
            builder.HasKey(x => x.Id);


            builder.HasOne(x => x.Equipment)
                .WithMany(i => i.CompatibleEquipments)
                .HasForeignKey(x => x.EquipmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
