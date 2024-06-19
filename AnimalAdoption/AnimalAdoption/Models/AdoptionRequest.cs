namespace AnimalAdoption.Models;

public class AdoptionRequest
{
    public int Id { get; set; }
    
    public DateOnly RequestDate { get; set; }
    public string Status { get; set; } = null!;
    
    public int AnimalId { get; set; }
    public int UserId { get; set; }
    
    public virtual Animal Animal { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}