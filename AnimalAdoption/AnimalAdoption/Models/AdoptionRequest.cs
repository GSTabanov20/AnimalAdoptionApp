namespace AnimalAdoption.Models;

public class AdoptionRequest
{
    public int Id { get; set; }
    
    public int AnimalId { get; set; }
    public string UserId { get; set; } = null!;
    public DateTime? RequestDate { get; set; }
    public string Status { get; set; } = "Pending";
    public string FormAnswers { get; set; } = null!;
    
    // Navigation properties
    public Animal Animal { get; set; } = null!;
    public User User { get; set; } = null!;
}