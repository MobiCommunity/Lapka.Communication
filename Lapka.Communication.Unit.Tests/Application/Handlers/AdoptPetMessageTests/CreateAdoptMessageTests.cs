using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Communication.Application.Commands;
using Lapka.Communication.Application.Commands.Handlers;
using Lapka.Communication.Application.Commands.Handlers.ShelterMessages;
using Lapka.Communication.Application.Commands.ShelterMessages;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Application.Services.Grpc;
using Lapka.Communication.Application.Services.Repositories;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Events.Abstract;
using Lapka.Communication.Core.Events.Concrete;
using Lapka.Communication.Core.Exceptions;
using Lapka.Communication.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Lapka.Communication.Unit.Tests.Application.Handlers.AdoptPetMessageTests
{
    public class CreateAdoptMessageTests
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPetService _grpcPetService;
        private readonly CreateAdoptPetMessageHandler _handler;
        private readonly IShelterMessageRepository _repository;
        private readonly IShelterMessageFactory _shelterMessageFactory;

        public CreateAdoptMessageTests()
        {
            _repository = Substitute.For<IShelterMessageRepository>();
            _shelterMessageFactory = Substitute.For<IShelterMessageFactory>();
            _grpcPetService = Substitute.For<IGrpcPetService>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _handler = new CreateAdoptPetMessageHandler(_eventProcessor, _shelterMessageFactory, _repository,
                _grpcPetService);
        }

        private Task Act(CreateAdoptPetMessage command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async void given_valid_adopt_message_should_be_created()
        {
            Guid petId = Guid.NewGuid();
            Guid shelterId = Guid.NewGuid();

            ShelterMessage message = Extensions.ArrangeShelterMessage(shelterId: shelterId);

            CreateAdoptPetMessage command = new CreateAdoptPetMessage(message.Id.Value, message.UserId, petId,
                message.Description.Value, message.FullName.Value, message.PhoneNumber.Value);

            _grpcPetService.GetShelterIdAsync(petId).Returns(shelterId);
            _shelterMessageFactory.CreateFromAdoptPetMessageAsync(command, shelterId).Returns(message);

            await Act(command);

            await _repository.Received()
                .AddAsync(Arg.Is<ShelterMessage>(m =>
                    m.Id.Value == message.Id.Value && m.ShelterId == shelterId && m.UserId == message.UserId &&
                    m.FullName.Value == message.FullName.Value && m.PhoneNumber.Value == message.PhoneNumber.Value));

            await _eventProcessor.Received().ProcessAsync(Arg.Is<IEnumerable<IDomainEvent>>(e
                => e.FirstOrDefault().GetType() == typeof(ShelterMessageCreated)));
        }

        [Fact]
        public async void pet_service_return_empty_guid_should_throw_an_exception()
        {
            Guid petId = Guid.NewGuid();

            ShelterMessage message = Extensions.ArrangeShelterMessage();
            
            CreateAdoptPetMessage command = new CreateAdoptPetMessage(message.Id.Value, message.UserId, petId,
                message.Description.Value, message.FullName.Value, message.PhoneNumber.Value);

            _grpcPetService.GetShelterIdAsync(petId).Returns(Guid.Empty);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<PetDoesNotExistsException>();
        }

        [Fact]
        public async void given_invalid_adopt_userId_should_throw_an_exception()
        {
            Guid petId = Guid.Empty;
            ShelterMessage message = Extensions.ArrangeShelterMessage();
                
            CreateAdoptPetMessage command = new CreateAdoptPetMessage(message.Id.Value, message.UserId, petId,
                message.Description.Value, message.FullName.Value, message.PhoneNumber.Value);
            
            Exception exception = await Record.ExceptionAsync(async () => await Act(command));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<PetDoesNotExistsException>();
        }

        [Fact]
        public void given_invalid_adopt_shelterId_should_throw_an_exception()
        {
            Guid userId = Guid.Empty;

            Exception exception = Record.Exception(() => Extensions.ArrangeShelterMessage(userId: userId));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidUserIdValueException>();
        }
    }
}