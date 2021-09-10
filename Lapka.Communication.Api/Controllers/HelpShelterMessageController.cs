using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Communication.Api.Models.Request;
using Lapka.Communication.Application.Commands;
using Lapka.Communication.Application.Queries;
using Lapka.Communication.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Communication.Api.Controllers
{
    [ApiController]
    [Route("api/message")]
    public class HelpShelterMessageController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public HelpShelterMessageController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        /// <summary>
        /// Gets help message 
        /// </summary>
        [HttpGet("{id:guid}/help")]
        public async Task<IActionResult> GetHelpMessage(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            return Ok(await _queryDispatcher.QueryAsync(new GetHelpShelterMessage
            {
                MessageId = id,
                UserId = userId
            }));
        }

        /// <summary>
        /// Get all help shelter messages
        /// </summary>
        [HttpGet("shelter/{id:guid}/help")]
        public async Task<IActionResult> GetHelpShelterMessages(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            return Ok(await _queryDispatcher.QueryAsync(new GetHelpShelterMessages
            {
                ShelterId = id,
                UserId = userId
            }));
        }

        /// <summary>
        /// Creates message for help shelter
        /// </summary>
        [HttpPost("help")]
        public async Task<IActionResult> CreateHelpShelterMessage([FromBody] CreateHelpShelterMessageRequest message)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            Guid id = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new CreateHelpShelterMessage(id, userId, message.ShelterId,
                message.HelpType, message.Description, message.FullName, message.PhoneNumber));

            return Created($"api/message/{id}/help", null);
        }

        /// <summary>
        /// Deletes a help pet message
        /// </summary>
        [HttpDelete("{id:guid}/help")]
        public async Task<IActionResult> DeleteHelpShelterMessage(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (Guid.Empty == userId)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new DeleteHelpShelterMessage(id, userId));

            return NoContent();
        }
    }
}