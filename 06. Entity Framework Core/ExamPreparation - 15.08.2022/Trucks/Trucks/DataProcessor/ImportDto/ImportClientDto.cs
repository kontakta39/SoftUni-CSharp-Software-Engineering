using System.ComponentModel.DataAnnotations;

namespace Trucks.DataProcessor.ImportDto;

public class ImportClientDto
{
    [Required]
    [StringLength(40, MinimumLength = 3)]
    public string Name { get; set; }

    [Required]
    [StringLength(40, MinimumLength = 2)]
    public string Nationality { get; set; }

    [Required]
    public string Type { get; set; }

    public int[] Trucks { get; set; }
}