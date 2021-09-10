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
    public class AdoptMessageController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public AdoptMessageController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        /// <summary>
        /// Gets adoption message 
        /// </summary>
        [HttpGet("{id:guid}/adopt")]
        public async Task<IActionResult> GetAdoptMessage(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            return Ok(await _queryDispatcher.QueryAsync(new GetAdoptPetMessage
            {
                MessageId = id,
                UserId = userId
            }));
        }

        /// <summary>
        /// Get all adopt shelters messages
        /// </summary>
        [HttpGet("shelter/{id:guid}/adopt")]
        public async Task<IActionResult> GetShelterAdoptMessages(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            return Ok(await _queryDispatcher.QueryAsync(new GetAdoptPetMessages
            {
                ShelterId = id,
                UserId = userId
            }));
        }

        /// <summary>
        /// Creates message for adoption
        /// </summary>
        [HttpPost("adopt")]
        public async Task<IActionResult> CreateAdoptMessage([FromBody] CreateAdoptPetMessageRequest message)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            Guid id = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new CreateAdoptPetMessage(id, userId, message.PetId, message.Description,
                message.FullName, message.PhoneNumber));

            return Created($"api/message/{id}/adopt", null);
        }

        /// <summary>
        /// Deletes a adopt pet message
        /// </summary>
        [HttpDelete("{id:guid}/adopt")]
        public async Task<IActionResult> DeleteAdoptPetMessage(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (Guid.Empty == userId)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new DeleteAdoptPetMessage(id, userId));

            return NoContent();
        }
    }
}