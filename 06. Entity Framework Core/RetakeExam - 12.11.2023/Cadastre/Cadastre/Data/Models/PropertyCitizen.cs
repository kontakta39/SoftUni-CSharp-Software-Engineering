using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cadastre.Data.Models;

public class PropertyCitizen
{
    [Key]
    public int PropertyId { get; set; }

    [ForeignKey(nameof(PropertyId))]
    public Property Property { get; set; }

    [Key]
    public int CitizenId { get; set; }

    [ForeignKey(nameof(CitizenId))]
    public Citizen Citizen { get; set; }
}