﻿using System;
using System.ComponentModel.DataAnnotations;
using WorkPlaces.Common.ValidationAttributes;
using WorkPlaces.DataModel.ValidationAttributes;

namespace WorkPlaces.DataModel.Models
{
    public class UserForManipulationDTO
    {
        [Required]
        [PersonName]
        [MinLength(2), MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [PersonName]
        [MinLength(2), MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [DateOfBirth]
        public DateTime DateOfBirth { get; set; }
    }
}
