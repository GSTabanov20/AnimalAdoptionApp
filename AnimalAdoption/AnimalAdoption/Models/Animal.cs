namespace AnimalAdoption.Models;

public class Animal
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public string Species { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Image { get; set; }
    public bool IsAdopted { get; set; }
    
    // Navigation property
    public ICollection<AdoptionRequest> AdoptionRequests { get; set; } = new List<AdoptionRequest>();
}