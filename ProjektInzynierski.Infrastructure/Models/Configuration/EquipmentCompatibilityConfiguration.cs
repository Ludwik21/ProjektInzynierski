using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjektInzynierski.Infrastructure.Models.Configuration
{
    public class EquipmentCompatibilityConfiguration : IEntityTypeConfiguration<EquipmentCompatibility>
    {
        public void Configure(EntityTypeBuilder<EquipmentCompatibility> builder)
        {
            builder.HasKey(x => x.Id);

            // Relacja: Equipment -> CompatibleEquipments
            builder.HasOne(x => x.Equipment)
                .WithMany(e => e.CompatibleEquipments)
                .HasForeignKey(x => x.EquipmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacja: CompatibleEquipment -> CompatibleAsEquipment
            builder.HasOne(x => x.CompatibleEquipment)
                .WithMany(e => e.CompatibleAsEquipment)
                .HasForeignKey(x => x.CompatibleEquipmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
