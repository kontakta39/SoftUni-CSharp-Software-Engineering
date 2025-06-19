using System.ComponentModel.DataAnnotations;

namespace BookWebStore.ViewModels;

public class GenreDeleteViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
}