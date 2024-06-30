using AnimalAdoption.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnimalAdoption.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Animal> Animals { get; set; }
        public DbSet<AdoptionRequest> AdoptionRequests { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}