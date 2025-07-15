using System.ComponentModel.DataAnnotations;

namespace BookWebStore.ViewModels;

public class BlogDetailsViewModel
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? ImageUrl { get; set; } 

    public string Publisher { get; set; } = null!;

    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateOnly PublishDate { get; set; }

    public string Content { get; set; } = null!;
}