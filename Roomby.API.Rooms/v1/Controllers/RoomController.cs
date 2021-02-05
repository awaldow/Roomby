using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Roomby.API.Models;
using Roomby.API.Rooms.Infrastructure.Exceptions;
using Roomby.API.Rooms.v1.Mediators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Roomby.API.Rooms.v1.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class RoomController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly ILogger<RoomController> _logger;

        public RoomController(IMediator mediator, ILogger<RoomController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region V1
        /// <summary>
        /// GetRoomsForHouseholdAsync(Guid householdId)
        /// </summary>
        /// <remarks>
        /// Returns a list of Rooms (sorted by name) for a given Household ID
        /// </remarks>
        /// <param name="householdId">The Household ID to return a list of Rooms for</param>
        /// <returns>A List of Room objects for the given Household ID</returns>
        [HttpGet("forHousehold/{householdId}", Name = "GetRoomsForHouseholdAsync")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(List<Room>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Room>>> GetRoomsForHouseholdAsync(Guid householdId)
        {
            try
            {
                var rooms = await _mediator.Send(new GetRoomsForHousehold { HouseholdId = householdId });
                return Ok(rooms);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// GetRoomAsync(Guid roomId)
        /// </summary>
        /// <remarks>
        /// Returns the Room object for <paramref name="roomId"/>
        /// </remarks>
        /// <param name="roomId">Room ID for the Room to get</param>
        /// <returns>Room object with id <paramref name="roomId"/></returns>
        [HttpGet("{roomId}", Name = "GetRoomAsync")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Room), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Room>> GetRoomAsync(Guid roomId)
        {
            try
            {
                var room = await _mediator.Send(new GetRoom { RoomId = roomId });
                if (room == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(room);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// CreateRoomAsync(Room roomToCreate)
        /// </summary>
        /// <remarks>
        /// Creates the provided <paramref name="roomToCreate"/>
        /// </remarks>
        /// <param name="roomToCreate">A Room object. See <see cref="Roomby.API.Rooms.Mediators.CreateRoomValidator"/> for validation information</param>
        /// <returns>The created Room object</returns>
        [HttpPost(Name = "CreateRoomAsync")]
        [MapToApiVersion("1")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Room), StatusCodes.Status201Created)]
        
        public async Task<ActionResult<Room>> CreateRoomAsync(Room roomToCreate)
        {
            // TODO: use bad requests here to test global 500 filter, maybe we don't need those try catches in the other controller actions
            var created = await _mediator.Send(new CreateRoom { RoomToCreate = roomToCreate });

            return CreatedAtAction(nameof(this.GetRoomAsync), new { id = created.Id }, created);
        }

        /// <summary>
        /// UpdateRoomAsync(Guid roomId, [FromBody] Room roomToUpdate)
        /// </summary>
        /// <remarks>
        /// Updates the Room <paramref name="roomId"/> with the values from <paramref name="roomToUpdate"/> if it exists; if <paramref name="roomId"/>
        /// is omitted, a new Room will be created instead.
        /// </remarks>
        /// <param name="roomId">Guid ID for the Room to update</param>
        /// <param name="roomToUpdate">Room object with the request changes filled in</param>
        /// <returns>The updated room object, or a newly created one if <paramref name="roomId"/> is omitted</returns>
        [HttpPut("{roomId}", Name = "UpdateRoom")]
        [MapToApiVersion("1")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Room), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Room), StatusCodes.Status200OK)]
        public async Task<ActionResult<Room>> UpdateRoomAsync(Guid roomId, [FromBody] Room roomToUpdate)
        {
            try
            {
                (Room updated, Room created) = await _mediator.Send(new UpdateRoom { RoomId = roomId, RoomToUpdate = roomToUpdate });
                if (updated != null)
                {
                    return Ok(updated);
                }
                else if (created != null)
                {
                    return CreatedAtAction(nameof(this.GetRoomAsync), new { id = created.Id }, created);
                }
                else
                {
                    _logger.LogError($"During attempt to update {roomId}, mediator returned that Room {roomId} was neither created nor updated");
                    return StatusCode(500, $"During attempt to update {roomId}, mediator returned that Room {roomId} was neither created nor updated");
                }
            }
            catch (UpdateRoomException e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// DeleteRoomAsync(Guid roomId)
        /// </summary>
        /// <remarks>
        /// Deletes the Room with the given <paramref name="roomId"/>
        /// </remarks>
        /// <param name="roomId">Room ID for the Room to delete</param>
        /// <returns>NoContent if successfully deleted; if ID can't be found or is not provided, BadRequest is returned.</returns>
        [HttpDelete("{roomId}", Name = "DeleteRoomAsync")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteRoomAsync(Guid roomId)
        {
            (bool roomWasDeleted, string message, Room deletedRoom) = await _mediator.Send(new DeleteRoom { RoomId = roomId });
            if (roomWasDeleted)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(message);
            }
        }
    }
    #endregion
}
