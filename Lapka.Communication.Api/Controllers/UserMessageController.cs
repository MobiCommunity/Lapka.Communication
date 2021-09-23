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
    [Route("api/message/conversation")]
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
        /// <returns>Messages between users</returns>
        /// <response code="200">If successfully got messages</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="404">If conversation is not found</response>
        [ProducesResponseType(typeof(UserDetailedConversationDto), StatusCodes.Status200OK)]
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
        /// <returns>User conversations</returns>
        /// <response code="200">If successfully got messages</response>
        /// <response code="401">If user is not logged</response>
        [ProducesResponseType(typeof(IEnumerable<UserBasicConversationDto>), StatusCodes.Status200OK)]
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
        /// <returns>No content</returns>
        /// <response code="204">If successfully got sent message</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="404">If user is not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
        /// <returns>No content</returns>
        /// <response code="204">If successfully marked message</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="404">If message is not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
        /// <returns>No content</returns>
        /// <response code="204">If successfully deleted conversation</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="404">If conversation is not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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