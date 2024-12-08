using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjektInzynierski.Infrastructure.Models.Configuration
{
    public class ClientConfiguration : IEntityTypeConfiguration<ClientDao>
    {
        public void Configure(EntityTypeBuilder<ClientDao> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Address).HasMaxLength(256);
        }
    }
}
