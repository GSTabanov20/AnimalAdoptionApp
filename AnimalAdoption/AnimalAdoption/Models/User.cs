using Microsoft.AspNetCore.Identity;

namespace AnimalAdoption.Models;

public class User : IdentityUser
{
    
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public bool IsAdmin { get; set; }
    
    // Navigation property
    public ICollection<AdoptionRequest> AdoptionRequests { get; set; } = new List<AdoptionRequest>();
}