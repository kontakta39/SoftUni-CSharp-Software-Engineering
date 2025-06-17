﻿using System.ComponentModel.DataAnnotations;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.Data.Models;

public class Genre
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(GenreNameMaxLength, MinimumLength = GenreNameMinLength)]
    public string Name { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;
}