using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Roomby.API.Models;
using Roomby.API.Users.Infrastructure.Exceptions;
using Roomby.API.Users.v1.Mediators;
using System;
using System.Threading.Tasks;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Roomby.API.Users.v1.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class HouseholdsController : ControllerBase
    {
         private readonly IMediator _mediator;

        private readonly ILogger<HouseholdsController> _logger;

        public HouseholdsController(IMediator mediator, ILogger<HouseholdsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region V1
        /// <summary>
        /// GetHouseholdAsync(Guid householdId)
        /// </summary>
        /// <remarks>
        /// Returns the Household object for <paramref name="householdId"/>
        /// </remarks>
        /// <param name="householdId">Household ID for the Household to get</param>
        /// <returns>Household object with id <paramref name="householdId"/></returns>
        [HttpGet("{householdId}", Name = "GetHouseholdAsync")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Household), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Household>> GetHouseholdAsync(Guid householdId)
        {
            try
            {
                var room = await _mediator.Send(new GetHousehold { HouseholdId = householdId });
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
        /// CreateHouseholdAsync(Household householdToCreate)
        /// </summary>
        /// <remarks>
        /// Creates the provided <paramref name="householdToCreate"/>
        /// </remarks>
        /// <param name="householdToCreate">A Household object. See <see cref="Roomby.API.Users.Mediators.CreateHouseholdValidator"/> for validation information</param>
        /// <returns>The created Household object</returns>
        [HttpPost(Name = "CreateHouseholdAsync")]
        [MapToApiVersion("1")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Household), StatusCodes.Status201Created)]
        
        public async Task<ActionResult<Household>> CreateRoomAsync(Household householdToCreate)
        {
            // TODO: use bad requests here to test global 500 filter, maybe we don't need those try catches in the other controller actions
            var created = await _mediator.Send(new CreateHousehold { HouseholdToCreate = householdToCreate });

            return CreatedAtAction(nameof(this.GetHouseholdAsync), new { id = created.Id }, created);
        }

        /// <summary>
        /// UpdateHouseholdAsync(Guid householdId, [FromBody] Household householdToUpdate)
        /// </summary>
        /// <remarks>
        /// Updates the Household <paramref name="householdId"/> with the values from <paramref name="householdToUpdate"/> if it exists; if <paramref name="householdId"/>
        /// is omitted, a new Household will be created instead.
        /// </remarks>
        /// <param name="householdId">Guid ID for the Household to update</param>
        /// <param name="householdToUpdate">Household object with the request changes filled in</param>
        /// <returns>The updated Household object, or a newly created one if <paramref name="householdId"/> is omitted</returns>
        [HttpPut("{householdId}", Name = "UpdateHouseholdAsync")]
        [MapToApiVersion("1")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Household), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Household), StatusCodes.Status200OK)]
        public async Task<ActionResult<Household>> UpdateRoomAsync(Guid householdId, [FromBody] Household householdToUpdate)
        {
            try
            {
                (Household updated, Household created) = await _mediator.Send(new UpdateHousehold { HouseholdId = householdId, HouseholdToUpdate = householdToUpdate });
                if (updated != null)
                {
                    return Ok(updated);
                }
                else if (created != null)
                {
                    return CreatedAtAction(nameof(this.GetHouseholdAsync), new { id = created.Id }, created);
                }
                else
                {
                    _logger.LogError($"During attempt to update {householdId}, mediator returned that Household {householdId} was neither created nor updated");
                    return StatusCode(500, $"During attempt to update {householdId}, mediator returned that Household {householdId} was neither created nor updated");
                }
            }
            catch (UpdateHouseholdException e)
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
        /// DeleteHouseholdAsync(Guid householdId)
        /// </summary>
        /// <remarks>
        /// Deletes the Household with the given <paramref name="householdId"/>
        /// </remarks>
        /// <param name="householdId">Household ID for the Household to delete</param>
        /// <returns>NoContent if successfully deleted; if ID can't be found or is not provided, BadRequest is returned.</returns>
        [HttpDelete("{householdId}", Name = "DeleteHouseholdAsync")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteHouseholdAsync(Guid householdId)
        {
            (bool householdWasDeleted, string message, Household deletedHousehold) = await _mediator.Send(new DeleteHousehold { HouseholdId = householdId });
            if (householdWasDeleted)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(message);
            }
        }
        #endregion
    }
}
