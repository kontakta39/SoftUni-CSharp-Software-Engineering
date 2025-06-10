﻿using System.ComponentModel.DataAnnotations;

namespace BookWebStore.ViewModels;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "The Email field is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = null!;
}