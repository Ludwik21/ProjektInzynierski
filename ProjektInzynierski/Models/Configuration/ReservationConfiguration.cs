using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjektInzynierski.Models.Configuration
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Client).WithMany(c => c.Reservations).HasForeignKey(u => u.ClientId).OnDelete(DeleteBehavior.Cascade);
            //builder.OwnsMany(x => x.Items, innerBuilder =>
            //{
            //    innerBuilder.HasKey(x => x.Id);
            //    innerBuilder.WithOwner(x => x.Reservation).HasForeignKey(r => r.ReservationId);
            //    innerBuilder.HasOne(x => x.Equipment).WithMany(r => r.ReservationItems).HasForeignKey(x => x.EquipmentId).OnDelete(DeleteBehavior.Cascade);
            //});

        }
    }
}
