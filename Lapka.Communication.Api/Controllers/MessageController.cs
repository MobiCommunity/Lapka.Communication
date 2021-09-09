using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Communication.Api.Models.Request;
using Lapka.Communication.Application.Commands;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Application.Queries;
using Lapka.Communication.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Communication.Api.Controllers
{
    [ApiController]
    [Route("api/message")]
    public class MessageController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public MessageController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
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
        
        [HttpGet("shelter/{id:guid}")]
        public async Task<IActionResult> GetShelterMessages(Guid id)
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


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateAdoptPetMessageRequest message)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            Guid id = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new CreateAdoptPetMessage(id, userId, message.ShelterId, message.PetId,
                message.Description, message.FullName, message.PhoneNumber));

            return Created($"api/message/{id}", null);
        }
    }
}