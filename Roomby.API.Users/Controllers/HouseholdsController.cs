using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Roomby.API.Models;
using Roomby.API.Users.Mediators;
using System;
using System.Threading.Tasks;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Roomby.API.Users.Controllers
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
        #endregion
    }
}
