using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using openapi_to_terraform.Extensions.Attributes;
using Roomby.API.Models;
using Roomby.API.Users.v1.Mediators;
using System;
using System.Threading.Tasks;

namespace Roomby.API.Users.v1.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [Revisions(1)]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class UsersController : ControllerBase
    {
         private readonly IMediator _mediator;

        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region V1
        /// <summary>
        /// GetUserAsync(Guid userId)
        /// </summary>
        /// <remarks>
        /// Returns the User object for <paramref name="userId"/>
        /// </remarks>
        /// <param name="userId">User ID for the User to get</param>
        /// <returns>User object with id <paramref name="userId"/></returns>
        [HttpGet("{userId}", Name = "GetUserAsync")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetUserAsync(Guid userId)
        {
            try
            {
                var room = await _mediator.Send(new GetUser { UserId = userId });
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
        /// CreateUserAsync(User userToCreate)
        /// </summary>
        /// <remarks>
        /// Creates the provided <paramref name="userToCreate"/>
        /// </remarks>
        /// <param name="userToCreate">A User object. See <see cref="Roomby.API.Users.v1.Mediators.CreateUserValidator"/> for validation information</param>
        /// <returns>The created User object</returns>
        [HttpPost(Name = "CreateUserAsync")]
        [MapToApiVersion("1")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        
        public async Task<ActionResult<User>> CreateUserAsync(User userToCreate)
        {
            // TODO: use bad requests here to test global 500 filter, maybe we don't need those try catches in the other controller actions
            var created = await _mediator.Send(new CreateUser { UserToCreate = userToCreate });

            return CreatedAtAction(nameof(this.GetUserAsync), new { id = created.Id }, created);
        }
        #endregion
    }
}
