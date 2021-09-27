using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Communication.Api.Models.Request;
using Lapka.Communication.Application.Commands;
using Lapka.Communication.Application.Commands.Conversations;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Application.Queries;
using Lapka.Communication.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Communication.Api.Controllers
{
    [ApiController]
    [Route("api/communication/message/conversation")]
    public class UserMessageController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public UserMessageController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        /// <summary>
        /// Gets all user messages from conversation. User has to be logged. 
        /// </summary>
        [ProducesResponseType(typeof(UserDetailedConversationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [HttpGet("{id:guid}/user")]
        public async Task<IActionResult> GetUserConversationMessages(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            return Ok(await _queryDispatcher.QueryAsync(new GetUserConversation
            {
                Id = id,
                UserId = userId
            }));
        }

        /// <summary>
        /// Gets all user conversations. User has to be logged. 
        /// </summary>
        [ProducesResponseType(typeof(IEnumerable<UserBasicConversationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [HttpGet("user")]
        public async Task<IActionResult> GetUserConversations()
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            return Ok(await _queryDispatcher.QueryAsync(new GetUserConversations
            {
                UserId = userId
            }));
        }


        /// <summary>
        /// Sends message to other user. User has to be logged. 
        /// </summary>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [HttpPost("user/{id:guid}")]
        public async Task<IActionResult> SendMessage(Guid id, CreateMessageRequest message)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            DateTime createdAt = DateTime.UtcNow;

            await _commandDispatcher.SendAsync(new CreateUserMessage(userId, id, message.Description,
                createdAt));

            return NoContent();
        }
        
        /// <summary>
        /// Marks message as read. User has to be logged. 
        /// </summary>
        [ProducesResponseType(typeof(object), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> MarkAsReadMessages(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            await _commandDispatcher.SendAsync(new MarkUserMessageAsRead(userId, id));

            return NoContent();
        }

        /// <summary>
        /// Deletes conversation between users (with all messages). User has to be logged. 
        /// </summary>
        [ProducesResponseType(typeof(object), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> SendMessage(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new DeleteUserConversation(userId, id));

            return NoContent();
        }
    }
}