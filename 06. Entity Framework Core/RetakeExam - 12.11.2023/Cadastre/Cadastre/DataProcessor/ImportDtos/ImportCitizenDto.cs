using System.ComponentModel.DataAnnotations;

namespace Cadastre.DataProcessor.ImportDtos;

public class ImportCitizenDto
{
    [Required]
    [StringLength(30, MinimumLength = 2)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 2)]
    public string LastName { get; set; }

    [Required]
    public string BirthDate { get; set; }

    [Required]
    public string MaritalStatus { get; set; }

    public int[] Properties { get; set; }
}