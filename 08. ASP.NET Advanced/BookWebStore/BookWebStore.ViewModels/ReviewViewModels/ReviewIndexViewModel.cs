﻿using System.ComponentModel.DataAnnotations;

namespace BookWebStore.ViewModels;

public class ReviewIndexViewModel
{
    public Guid Id { get; set; }

    public Guid BookId { get; set; }

    public string UserId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateOnly ReviewDate { get; set; }

    public string ReviewText { get; set; } = null!;

    public int Rating { get; set; }

    public bool IsEdited { get; set; } = false;
}