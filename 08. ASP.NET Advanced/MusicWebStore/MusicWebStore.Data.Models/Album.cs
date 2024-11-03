using MusicWebStore.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MusicWebStore.Constants.ModelConstants;

namespace MusicWebStore.Data;

public class Album
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(AlbumTitleMaxLength, MinimumLength = AlbumTitleMinLength)]
    public string Title { get; set; } = null!;

    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateOnly ReleaseDate { get; set; }

    [Required]
    [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
    public string Description { get; set; } = null!;

    public string? ImageUrl { get; set; }

    [Required]
    [Range(AlbumMinPrice, AlbumMaxPrice)]
    public decimal Price { get; set; }

    [Required]
    [StringLength(AlbumStockMaxLength, MinimumLength = AlbumStockMinLength)]
    public int Stock { get; set; } 

    [Required]
    public Guid ArtistId { get; set; }

    [ForeignKey(nameof(ArtistId))]
    public Artist Artist { get; set; } = null!;

    [Required]
    public Guid GenreId { get; set; }

    [ForeignKey(nameof(GenreId))]
    public Genre Genre { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<OrderAlbum> OrderAlbums { get; set; } = new HashSet<OrderAlbum>();
    public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
}