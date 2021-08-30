﻿using System.Linq;

namespace WorkPlaces.Data.Repositories
{
    public class WorkPlacesRepository : IWorkPlacesRepository
    {
        private readonly ApplicationDbContext context;

        public WorkPlacesRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool Exists(int workPlaceId)
        {
            return context.WorkPlaces.Any(wp => wp.DeletedAt == null && wp.Id == workPlaceId);
        }
    }
}