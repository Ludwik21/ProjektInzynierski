using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjektInzynierski.Models.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserName).IsRequired();
            builder.HasOne(x => x.Client).WithMany(c => c.Users).HasForeignKey(u => u.ClientId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.UserRole).HasConversion<string>().IsRequired();
        }
    }
}
