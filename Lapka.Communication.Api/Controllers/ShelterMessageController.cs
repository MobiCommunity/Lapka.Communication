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
    public class ShelterMessageController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public ShelterMessageController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        /// <summary>
        /// Gets help message 
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetMessage(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            return Ok(await _queryDispatcher.QueryAsync(new GetShelterMessage
            {
                MessageId = id,
                UserId = userId
            }));
        }

        /// <summary>
        /// Get all help shelter messages
        /// </summary>
        [HttpGet("shelter/{id:guid}")]
        public async Task<IActionResult> GetShelterMessages(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            return Ok(await _queryDispatcher.QueryAsync(new GetShelterMessages
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

            return Created($"api/message/{id}", null);
        }
        
        /// <summary>
        /// Creates a report stray pet message
        /// </summary>
        [HttpPost("stray")]
        public async Task<IActionResult> CreateStrayPetMessage([FromForm] ReportStrayPetRequest request)
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

            return Created($"api/message/{messageId}", null);
        }
        
        /// <summary>
        /// Creates message for adoption
        /// </summary>
        [HttpPost("adopt")]
        public async Task<IActionResult> CreateAdoptPetMessage([FromBody] CreateAdoptPetMessageRequest message)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            Guid id = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new CreateAdoptPetMessage(id, userId, message.PetId, message.Description,
                message.FullName, message.PhoneNumber));

            return Created($"api/message/{id}", null);
        }
    }
}