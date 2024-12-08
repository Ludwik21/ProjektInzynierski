using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjektInzynierski.Infrastructure.Models.Configuration
{
    public class ReservationConfiguration : IEntityTypeConfiguration<ReservationDao>
    {
        public void Configure(EntityTypeBuilder<ReservationDao> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Client).WithMany(c => c.Reservations).HasForeignKey(u => u.ClientId).OnDelete(DeleteBehavior.Cascade);        }
    }
}
