﻿using System;

namespace WorkPlaces.DataModel.Models
{
    public class UserWorkPlaceDTO
    {
        public int Id { get; set; }

        public string User { get; set; }

        public string WorkPlace { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}