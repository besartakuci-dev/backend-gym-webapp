using Microsoft.EntityFrameworkCore;
using backend_gym_webapp.Models;

namespace backend_gym_webapp.Data
{
    /// <summary>
    /// GymDbContext - manages connection between C# and database
    /// </summary>
    public class GymDbContext : DbContext
    {
        // Constructor
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {
        }

        // TABLES in database
        public DbSet<GymClass> GymClasses { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<MembershipPlan> MembershipPlans { get; set; } // NEW TABLE

        /// <summary>
        /// Configure database rules (validation, constraints)
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------------------------
            // GymClass configuration
            // ---------------------------
            modelBuilder.Entity<GymClass>()
                .Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<GymClass>()
                .Property(g => g.Description)
                .IsRequired(false)
                .HasMaxLength(500);

            // ---------------------------
            // Trainer configuration
            // ---------------------------
            modelBuilder.Entity<Trainer>()
                .Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Trainer>()
                .Property(t => t.Specialty)
                .IsRequired()
                .HasMaxLength(100);

            // ---------------------------
            // MembershipPlan configuration
            // ---------------------------
            modelBuilder.Entity<MembershipPlan>()
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<MembershipPlan>()
                .Property(m => m.Description)
                .IsRequired(false)
                .HasMaxLength(300);
        }
    }
}