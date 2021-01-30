using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Roomby.API.Models;
using Roomby.API.Rooms.Mediators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Roomby.API.Rooms.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public class RoomController : ControllerBase
    {
         private readonly IMediator _mediator;

        private readonly ILogger<RoomController> _logger;

        public RoomController(IMediator mediator, ILogger<RoomController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{householdId}", Name = "GetRoomsForHouseholdAsync")]
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

        [HttpGet("{roomId}", Name = "GetRoom")]
        [ProducesResponseType(typeof(Room), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Room>> GetRoom(Guid roomId)
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

        [HttpPost(Name = "CreateRoom")]
        public async Task<ActionResult<Room>> CreateRoom(Room roomToCreate)
        {
            // TODO: use bad requests here to test global 500 filter, maybe we don't need those try catches in the other controller actions
            var created = await _mediator.Send(new CreateRoom { RoomToCreate = roomToCreate });

            return CreatedAtAction(nameof(this.GetRoom), new { id = created.Id }, created);
        }
    }
}
