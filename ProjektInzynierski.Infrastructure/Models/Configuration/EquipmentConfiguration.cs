using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjektInzynierski.Infrastructure.Models.Configuration
{
    public class EquipmentConfiguration : IEntityTypeConfiguration<EquipmentDao>
    {
        public void Configure(EntityTypeBuilder<EquipmentDao> builder)
        {
            builder.HasKey(e => e.Id);

            // Inne właściwości i konfiguracje
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);

            // Konfiguracja kolekcji CompatibleEquipments
            builder.HasMany(e => e.CompatibleEquipments)
                .WithOne(ec => ec.Equipment)
                .HasForeignKey(ec => ec.EquipmentId);

            // Konfiguracja kolekcji CompatibleAsEquipment
            builder.HasMany(e => e.CompatibleAsEquipment)
                .WithOne(ec => ec.CompatibleEquipment)
                .HasForeignKey(ec => ec.CompatibleEquipmentId);
        }
    }

}
