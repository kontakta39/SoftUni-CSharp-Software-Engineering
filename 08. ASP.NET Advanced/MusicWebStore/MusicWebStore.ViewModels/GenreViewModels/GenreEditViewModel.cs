﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MusicWebStore.Constants.ModelConstants;

namespace MusicWebStore.ViewModels;

public class GenreEditViewModel
{
    [Required]
    [StringLength(GenreNameMaxLength, MinimumLength = GenreNameMinLength)]
    public string Name { get; set; } = null!;
}