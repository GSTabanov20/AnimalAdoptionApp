using AnimalAdoption.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnimalAdoption.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<AdoptionRequest> AdoptionRequests { get; set; }
    public DbSet<Animal> Animals { get; set; }
    public new DbSet<User> Users { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}