using AnimalAdoption.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnimalAdoption.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Animal> Animals { get; set; }
        public DbSet<AdoptionRequest> AdoptionRequests { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AdoptionRequest>(entity =>
            {
                entity.Property(e => e.FormAnswers).HasColumnType("json");
            });
        }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}