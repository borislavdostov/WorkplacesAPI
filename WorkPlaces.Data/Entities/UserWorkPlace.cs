﻿using System;
using System.ComponentModel.DataAnnotations;
using WorkPlaces.Data.Common;

namespace WorkPlaces.Data.Entities
{
    public class UserWorkPlace : BaseModel
    {
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public int WorkPlaceId { get; set; }

        public virtual WorkPlace WorkPlace { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }
    }
}