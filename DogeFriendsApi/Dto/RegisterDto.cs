﻿using System.ComponentModel.DataAnnotations;

namespace DogeFriendsApi.Dto
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Hometown { get; set; }
        public string? Description { get; set; }
    }
}