﻿using CinemaWebApp.ViewModels.Movie;

namespace CinemaWebApp.ViewModels.Cinema;

public class CinemaDetailsViewModel
{
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;

    public IEnumerable<CinemaMovieViewModel> Movies { get; set; }
        = new HashSet<CinemaMovieViewModel>();
}