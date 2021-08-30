﻿using System;
using System.ComponentModel.DataAnnotations;
using WorkPlaces.Common.ValidationAttributes;

namespace WorkPlaces.DataModel.Models
{
    public class UserForCreationDTO
    {
        [Required]
        [MinLength(2), MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2), MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DateOfBirth]
        public DateTime DateOfBirth { get; set; }
    }
}