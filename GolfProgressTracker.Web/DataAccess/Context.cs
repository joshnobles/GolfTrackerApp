using GolfProgressTracker.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GolfProgressTracker.Web.DataAccess
{
    public class Context(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Round> Round { get; set; }

        public DbSet<Hole> Hole { get; set; }
    }
}
