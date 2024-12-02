using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjektInzynierski.Models.Configuration
{
    public class ReservationItemConfiguration : IEntityTypeConfiguration<ReservationItem>
    {
        public void Configure(EntityTypeBuilder<ReservationItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Reservation).WithMany(c => c.Items).HasForeignKey(u => u.ReservationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Equipment).WithMany(c => c.ReservationItems).HasForeignKey(u => u.EquipmentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
