namespace AnimalAdoption.Models;

public class Animal
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public string Breed { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Image { get; set; } = null!;
    public string Status { get; set; } = null!;
    public bool Adopted { get; set; }
}