﻿using System.ComponentModel.DataAnnotations;

namespace MusicWebStore.ViewModels;

public class ReviewIndexViewModel
{
    public Guid Id { get; set; }
    public Guid AlbumId { get; set; }
    public string UserId { get; set; } = null!;

    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateOnly ReviewDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public string ReviewText { get; set; } = null!;
    public int Rating { get; set; }
    public bool isCommented { get; set; } = false;
    public bool IsEdited { get; set; } = false;
}