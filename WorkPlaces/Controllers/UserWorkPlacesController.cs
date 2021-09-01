﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkPlaces.DataModel.Models;
using WorkPlaces.Service.Interfaces;

namespace WorkPlaces.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserWorkPlacesController : ControllerBase
    {
        private readonly IUserWorkPlacesService userWorkPlacesService;
        private readonly IUsersService usersService;
        private readonly IWorkPlacesService workPlacesService;

        public UserWorkPlacesController(
            IUserWorkPlacesService userWorkPlacesService,
            IUsersService usersService,
            IWorkPlacesService workPlacesService)
        {
            this.userWorkPlacesService = userWorkPlacesService;
            this.usersService = usersService;
            this.workPlacesService = workPlacesService;
        }

        /// <summary>
        /// Gets all user workplaces
        /// </summary>
        /// <returns>All user work places(without deleted)</returns>
        /// <response code="200">Returns all user work places(without deleted) or empty collection</response>
        [HttpGet(Name = nameof(GetUserWorkPlaces))]
        public ActionResult<IEnumerable<UserWorkPlaceDTO>> GetUserWorkPlaces()
        {
            var userWorkPlaces = userWorkPlacesService.GetUserWorkPlaces();
            return Ok(userWorkPlaces);
        }

        /// <summary>
        /// Gets specific user workplace
        /// </summary>
        /// <param name="userWorkPlaceId">Id of the user workplace</param>
        /// <returns>The user workplace with the given Id</returns>
        /// <response code="200">Returns a user workplace with the given Id</response>
        /// <response code="404">If a user workplace with the given id does not exist</response>
        [HttpGet("{userWorkPlaceId}")]
        public async Task<ActionResult<UserWorkPlaceDTO>> GetUserWorkPlace(int userWorkPlaceId)
        {
            if (!await userWorkPlacesService.UserWorkPlaceExists(userWorkPlaceId))
            {
                return NotFound();
            }

            var userWorkPlace = await userWorkPlacesService.GetUserWorkPlace(userWorkPlaceId);
            return Ok(userWorkPlace);
        }

        /// <summary>
        /// Creates new user workplace
        /// </summary>
        /// <param name="userWorkPlace"></param>
        /// <returns>A newly created user workplace</returns>
        /// <response code="201">Returns the newly created user workplace</response>
        [HttpPost]
        public async Task<ActionResult<UserWorkPlaceDTO>> CreateUserWorkPlace(UserWorkPlaceForManipulationDTO userWorkPlace)
        {
            if (!await usersService.UserExists(userWorkPlace.UserId) ||
                !await workPlacesService.WorkPlaceExists(userWorkPlace.WorkPlaceId))
            {
                return NotFound();
            }

            var userWorkPlaceToReturn = await userWorkPlacesService.CreateUserWorkPlaceAsync(userWorkPlace);
            return CreatedAtRoute(nameof(GetUserWorkPlaces),
                new { userWorkPlaceId = userWorkPlaceToReturn.Id }, userWorkPlaceToReturn);
        }

        /// <summary>
        /// Updates user workplace
        /// </summary>
        /// <param name="userWorkPlaceId">Id of the user workplace</param>
        /// <param name="userWorkPlace"></param>
        /// <returns>No content</returns>
        /// <response code="204">If the user workplace is updated successfully</response>
        /// <response code="404">If a user workplace, user or workplace with the given id does not exist</response>
        [HttpPut("{userWorkPlaceId}")]
        public async Task<IActionResult> UpdateUserWorkPlace(int userWorkPlaceId, UserWorkPlaceForManipulationDTO userWorkPlace)
        {
            if (!await userWorkPlacesService.UserWorkPlaceExists(userWorkPlaceId) ||
                !await usersService.UserExists(userWorkPlace.UserId) ||
                !await workPlacesService.WorkPlaceExists(userWorkPlace.WorkPlaceId))
            {
                return NotFound();
            }

            await userWorkPlacesService.UpdateUserWorkPlace(userWorkPlaceId, userWorkPlace);
            return NoContent();
        }

        /// <summary>
        /// Deletes user workplace
        /// </summary>
        /// <param name="userWorkPlaceId">Id of the user workplace</param>
        /// <returns>No content</returns>
        /// <response code="204">If the user workplace is deleted successfully</response>
        /// <response code="404">If a user workplace with the given id does not exist</response>
        [HttpDelete("{userWorkPlaceId}")]
        public async Task<IActionResult> DeleteUserWorkPlace(int userWorkPlaceId)
        {
            if (!await userWorkPlacesService.UserWorkPlaceExists(userWorkPlaceId))
            {
                return NotFound();
            }

            await userWorkPlacesService.DeleteUserWorkPlace(userWorkPlaceId);
            return NoContent();
        }
    }
}
