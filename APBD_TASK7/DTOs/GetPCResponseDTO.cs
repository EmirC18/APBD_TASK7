namespace APBD_TASK7.DTOs;

public class GetPCResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double Weight { get; set; }
    public int Warranty { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public int Stock { get; set; }

    public List<ComponentDTO> Components { get; set; } = new();
}