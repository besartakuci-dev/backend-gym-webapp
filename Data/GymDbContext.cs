using Microsoft.EntityFrameworkCore;
using backend_gym_webapp.Models;

namespace backend_gym_webapp.Data
{
    /// <summary>
    /// GymDbContext - This is the database connection manager
    /// It tells Entity Framework Core how to interact with our database
    /// Think of it as the "bridge" between our C# classes and the database
    /// </summary>
    public class GymDbContext : DbContext
    {
        // Constructor - called when this class is created
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {
        }

        // DbSet - represents a table in the database
        // We use this to query and manage GymClass records
        public DbSet<GymClass> GymClasses { get; set; }

        /// <summary>
        /// OnModelCreating - optional: configure the database structure here
        /// Currently not needed, but good practice to have
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: Add data validation or constraints here
            modelBuilder.Entity<GymClass>()
                .Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<GymClass>()
                .Property(g => g.Description)
                .IsRequired(false)
                .HasMaxLength(500);
        }
    }
}
