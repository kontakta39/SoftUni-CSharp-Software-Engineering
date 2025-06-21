﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookWebStore.Data.Models;

namespace BookWebStore.ViewModels;

public class BookDetailsViewModel
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Publisher { get; set; } = null!;

    public int? ReleaseYear { get; set; }

    public int? PagesNumber { get; set; }

    public string? ImageUrl { get; set; }

    public decimal? Price { get; set; }

    public int? Stock { get; set; }

    public string Genre { get; set; } = null!;

    public string Author { get; set; } = null!;

    public Guid AuthorId { get; set; }
}