﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MusicHub.Data.Models;

public class Album
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(40)]
    public string Name { get; set; }

    [Required]
    public DateTime ReleaseDate { get; set; }

    public decimal Price => Songs.Sum(p => p.Price);

    public int? ProducerId { get; set; }

    [ForeignKey(nameof(ProducerId))]
    public virtual Producer Producer { get; set; }

    public virtual ICollection<Song> Songs { get; set; }
}