namespace APBD_TASK7.DTOs;

public class CreatePcRequestDto
{
    public string Name { get; set; } = null!;
    public double Weight { get; set; }
    public int Warranty { get; set; }
    public int Stock { get; set; }

    public List<CreatePcComponentDto> Components { get; set; } = [];
}

public class CreatePcComponentDto
{
    public string ComponentCode { get; set; } = null!;
    public int Amount { get; set; }
}