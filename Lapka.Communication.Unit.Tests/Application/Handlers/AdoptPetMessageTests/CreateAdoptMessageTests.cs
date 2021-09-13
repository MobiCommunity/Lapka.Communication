using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Communication.Application.Commands;
using Lapka.Communication.Application.Commands.Handlers;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Events.Abstract;
using Lapka.Communication.Core.Events.Concrete;
using Lapka.Communication.Core.Exceptions;
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
        private readonly IAdoptPetMessageRepository _adoptPetRepository;

        public CreateAdoptMessageTests()
        {
            _adoptPetRepository = Substitute.For<IAdoptPetMessageRepository>();
            _grpcPetService = Substitute.For<IGrpcPetService>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _handler = new CreateAdoptPetMessageHandler(_eventProcessor, _adoptPetRepository, _grpcPetService);
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

            AdoptPetMessage message =
                Extensions.ArrangeAdoptPetMessage(petId: petId);

            CreateAdoptPetMessage command = new CreateAdoptPetMessage(message.Id.Value, message.UserId, message.PetId,
                message.Description.Value, message.FullName.Value, message.PhoneNumber.Value);

            _grpcPetService.GetShelterId(petId).Returns(shelterId);

            await Act(command);

            await _adoptPetRepository.Received()
                .AddAsync(Arg.Is<AdoptPetMessage>(m =>
                    m.Id.Value == message.Id.Value && m.ShelterId == shelterId && m.UserId == message.UserId &&
                    m.PetId == message.PetId && m.Description.Value == message.Description.Value &&
                    m.FullName.Value == message.FullName.Value && m.PhoneNumber.Value == message.PhoneNumber.Value));

            await _eventProcessor.Received().ProcessAsync(Arg.Is<IEnumerable<IDomainEvent>>(e
                => e.FirstOrDefault().GetType() == typeof(AdoptPetMessageCreated)));
        }

        [Fact]
        public async void pet_service_return_empty_guid_should_throw_an_exception()
        {
            Guid petId = Guid.NewGuid();

            AdoptPetMessage message =
                Extensions.ArrangeAdoptPetMessage(petId: petId);

            CreateAdoptPetMessage command = new CreateAdoptPetMessage(message.Id.Value, message.UserId, message.PetId,
                message.Description.Value, message.FullName.Value, message.PhoneNumber.Value);

            _grpcPetService.GetShelterId(petId).Returns(Guid.Empty);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidShelterIdValueException>();
        }

        [Fact]
        public async void given_invalid_adopt_userId_should_throw_an_exception()
        {
            Guid petId = Guid.Empty;

            Exception exception =
                await Record.ExceptionAsync(async () => Extensions.ArrangeAdoptPetMessage(petId: petId));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidPetIdValueException>();
        }

        [Fact]
        public async void given_invalid_adopt_shelterId_should_throw_an_exception()
        {
            Guid userId = Guid.Empty;

            Exception exception =
                await Record.ExceptionAsync(async () => Extensions.ArrangeAdoptPetMessage(userId: userId));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidUserIdValueException>();
        }
    }
}