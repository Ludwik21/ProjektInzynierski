using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjektInzynierski.Infrastructure.Models
{
    public class ProjektContextFactory : IDesignTimeDbContextFactory<ProjektContext>
    {
        public ProjektContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjektContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-6R2A645\\SQLEXPRESS;Database=ProjektInzynierski4;Trusted_Connection=True;TrustServerCertificate=True;");

            return new ProjektContext(optionsBuilder.Options);
        }
    }
}
