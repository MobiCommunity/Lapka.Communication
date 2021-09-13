using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Commands.Handlers
{
    public class CreateAdoptPetMessageHandler : ICommandHandler<CreateAdoptPetMessage>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IAdoptPetMessageRepository _repository;
        private readonly IGrpcPetService _grpcPetService;

        public CreateAdoptPetMessageHandler(IEventProcessor eventProcessor, IAdoptPetMessageRepository repository,
            IGrpcPetService grpcPetService)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _grpcPetService = grpcPetService;
        }

        public async Task HandleAsync(CreateAdoptPetMessage command)
        {
            Guid shelterId = await _grpcPetService.GetShelterId(command.PetId);

            AdoptPetMessage message = AdoptPetMessage.Create(command.Id, command.UserId, shelterId,
                command.PetId, command.Description, command.FullName, command.PhoneNumber);

            await _repository.AddAsync(message);
            await _eventProcessor.ProcessAsync(message.Events);
        }
    }
}