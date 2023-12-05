﻿using System.ComponentModel.DataAnnotations;

namespace Boardgames.Data.Models;

public class Creator
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(7, MinimumLength = 2)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(7, MinimumLength = 2)]
    public string LastName { get; set; }

    public ICollection<Boardgame> Boardgames { get; set; } = new List<Boardgame>();
}