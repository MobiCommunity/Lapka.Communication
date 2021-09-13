using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Communication.Api.Models;
using Lapka.Communication.Api.Models.Request;
using Lapka.Communication.Application.Commands;
using Lapka.Communication.Application.Queries;
using Lapka.Communication.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Communication.Api.Controllers
{
    [ApiController]
    [Route("api/message")]
    public class StrayPetMessageController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public StrayPetMessageController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        /// <summary>
        /// Gets stray pet message 
        /// </summary>
        [HttpGet("{id:guid}/stray")]
        public async Task<IActionResult> GetStrayPetMessage(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            return Ok(await _queryDispatcher.QueryAsync(new GetStrayPetMessage
            {
                MessageId = id,
                UserId = userId
            }));
        }
        
        /// <summary>
        /// Get all stray pet shelters messages
        /// </summary>
        [HttpGet("shelter/{id:guid}/stray")]
        public async Task<IActionResult> GetShelterStrayPetMessages(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            return Ok(await _queryDispatcher.QueryAsync(new GetStrayPetMessages
            {
                ShelterId = id,
                UserId = userId
            }));
        }

        /// <summary>
        /// Creates a report stray pet message
        /// </summary>
        [HttpPost("stray")]
        public async Task<IActionResult> ReportStrayPet([FromForm] ReportStrayPetRequest request)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (Guid.Empty == userId)
            {
                return Unauthorized();
            }

            Guid messageId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new CreateStrayPetMessage(messageId, userId,
                request.Location.AsValueObject(), request.Photos.CreatePhotoFiles(), request.Description,
                request.ReporterName, request.ReporterPhoneNumber));

            return Created($"api/message/{messageId}/stray", null);
        }
        
        /// <summary>
        /// Deletes a stray pet message
        /// </summary>
        [HttpDelete("{id:guid}/stray")]
        public async Task<IActionResult> DeleteStrayPetMessage(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (Guid.Empty == userId)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new DeleteStrayPetMessage(id, userId));

            return NoContent();
        }
    }
}