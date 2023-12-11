using System.ComponentModel.DataAnnotations;

namespace Footballers.DataProcessor.ImportDto;

public class ImportTeamDto
{
    [Required]
    [StringLength(40, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9\s.-]+$")]
    public string Name { get; set; }

    [Required]
    [StringLength(40, MinimumLength = 2)]
    public string Nationality { get; set; }

    [Required]
    public int Trophies { get; set; }

    public int[] Footballers { get; set; }
}