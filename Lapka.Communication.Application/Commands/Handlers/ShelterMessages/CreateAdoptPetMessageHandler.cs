﻿using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Communication.Application.Commands.ShelterMessages;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Application.Services.Grpc;
using Lapka.Communication.Application.Services.Repositories;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Commands.Handlers.ShelterMessages
{
    public class CreateAdoptPetMessageHandler : ICommandHandler<CreateAdoptPetMessage>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterMessageFactory _messageFactory;
        private readonly IShelterMessageRepository _repository;
        private readonly IGrpcPetService _grpcPetService;

        public CreateAdoptPetMessageHandler(IEventProcessor eventProcessor, IShelterMessageFactory messageFactory,
            IShelterMessageRepository repository, IGrpcPetService grpcPetService)
        {
            _eventProcessor = eventProcessor;
            _messageFactory = messageFactory;
            _repository = repository;
            _grpcPetService = grpcPetService;
        }

        public async Task HandleAsync(CreateAdoptPetMessage command)
        {
            Guid shelterId = await GetShelterIdAsync(command);

            ShelterMessage message = _messageFactory.CreateFromAdoptPetMessage(command, shelterId);

            await _repository.AddAsync(message);
            await _eventProcessor.ProcessAsync(message.Events);
        }

        private async Task<Guid> GetShelterIdAsync(CreateAdoptPetMessage command)
        {
            Guid shelterId = await _grpcPetService.GetShelterIdAsync(command.PetId);
            if (shelterId == Guid.Empty)
            {
                throw new PetDoesNotExistsException(command.Id);
            }

            return shelterId;
        }
    }
}