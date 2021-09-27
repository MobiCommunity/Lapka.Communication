using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Communication.Api.Models;
using Lapka.Communication.Api.Models.Request;
using Lapka.Communication.Application.Commands;
using Lapka.Communication.Application.Commands.ShelterMessages;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Application.Queries;
using Lapka.Communication.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Communication.Api.Controllers
{
    [ApiController]
    [Route("api/communication/message")]
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
        /// Gets shelter message. Can be obtained by user who sent the message, and by the owner of the shelter, to which
        /// was message sent. User has to be logged.
        /// </summary>
        [ProducesResponseType(typeof(ShelterMessageDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
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
        /// Gets all shelter messages. Can be obtained only by owner of shelter to which messages messages was sent.
        /// User has to be logged.
        /// </summary>
        [ProducesResponseType(typeof(IEnumerable<ShelterMessageDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
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
        /// Creates a help message to the shelter. By help means he offer for example walk with a pet.
        /// User has to be logged.
        /// </summary>
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
        [HttpPost("help")]
        public async Task<IActionResult> CreateHelpShelterMessage(CreateHelpShelterMessageRequest message)
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
        /// Creates a stray message to the shelter. This type of message is for reporting stray pets.
        /// User has to be logged. 
        /// </summary>
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
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
        /// Creates message for adoption to the shelter. User has to be logged. 
        /// </summary>
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [HttpPost("adopt")]
        public async Task<IActionResult> CreateAdoptPetMessage(CreateAdoptPetMessageRequest message)
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
        
        /// <summary>
        /// Marks message as read. User has to be logged. 
        /// </summary>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkAsReadMessage(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            await _commandDispatcher.SendAsync(new MarkShelterMessageAsRead(userId, id));

            return NoContent();
        }
        
        /// <summary>
        /// Gets shelter unread messages count. User has to be logged and owner of shelter.
        /// </summary>
        [ProducesResponseType(typeof(IEnumerable<long>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [HttpGet("count/shelter/{id:guid}")]
        public async Task<ActionResult<long>> GetUnreadMessagesCount(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            return Ok(await _queryDispatcher.QueryAsync(new GetUnreadMessagesCount
            {
                ShelterId = id
            }));
            
        }
    }
}