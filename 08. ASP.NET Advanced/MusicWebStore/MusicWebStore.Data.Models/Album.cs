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

    [StringLength(AlbumLabelMaxLength, MinimumLength = AlbumLabelMinLength)]
    public string? Label { get; set; } = null!;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateOnly? ReleaseDate { get; set; } = null!;

    [Required]
    [StringLength(AlbumDescriptionMaxLength, MinimumLength = AlbumDescriptionMinLength)]
    public string Description { get; set; } = null!;

    public string? ImageUrl { get; set; } = null!;

    [Required]
    [Range(AlbumMinPrice, AlbumMaxPrice)]
    public decimal Price { get; set; }

    [Required]
    [Range(AlbumStockMinLength, AlbumStockMaxLength)]
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

    public virtual ICollection<BuyerAlbum> BuyersAlbums { get; set; } = new HashSet<BuyerAlbum>();
    public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
}