using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjektInzynierski.Infrastructure.Models.Configuration
{
    public class ReservationItemConfiguration : IEntityTypeConfiguration<ReservationItemDao>
    {
        public void Configure(EntityTypeBuilder<ReservationItemDao> builder)
        {
            builder.HasKey(x => new { x.EquipmentId, x.ReservationId });
            builder.HasOne(x => x.Reservation).WithMany(c => c.Items).HasForeignKey(u => u.ReservationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Equipment).WithMany(c => c.ReservationItems).HasForeignKey(u => u.EquipmentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
