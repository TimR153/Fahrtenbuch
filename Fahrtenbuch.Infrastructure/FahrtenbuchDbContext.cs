using Microsoft.EntityFrameworkCore;

namespace Fahrtenbuch.Infrastructure
{
    public class FahrtenbuchDbContext : DbContext
    {
        public FahrtenbuchDbContext(DbContextOptions<FahrtenbuchDbContext> options) : base(options) { }
    }
}
