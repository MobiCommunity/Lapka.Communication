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
using Lapka.Communication.Core.ValueObjects.Locations;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Lapka.Communication.Unit.Tests.Application.Handlers.HelpShelterMessageTests
{
    public class CreateHelpShelterMessageTests
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterRepository _shelterRepository;
        private readonly CreateHelpShelterMessageHandler _handler;
        private readonly IShelterMessageRepository _repository;
        private readonly IShelterMessageFactory _shelterMessageFactory;

        public CreateHelpShelterMessageTests()
        {
            _repository = Substitute.For<IShelterMessageRepository>();
            _shelterMessageFactory = Substitute.For<IShelterMessageFactory>();
            _shelterRepository = Substitute.For<IShelterRepository>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _handler = new CreateHelpShelterMessageHandler(_eventProcessor, _repository, _shelterMessageFactory,
                _shelterRepository);
        }

        private Task Act(CreateHelpShelterMessage command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async void given_valid_help_shelter_message_should_be_created()
        {
            ShelterMessage message = Extensions.ArrangeShelterMessage();
            HelpType helpType = HelpType.Walk;
            Shelter shelter = new Shelter(Guid.NewGuid(), new Location("33", "44"));

            CreateHelpShelterMessage command = new CreateHelpShelterMessage(message.Id.Value, message.UserId,
                message.ShelterId, helpType, message.Description.Value, message.FullName.Value,
                message.PhoneNumber.Value);

            _shelterRepository.GetAsync(message.ShelterId).Returns(shelter);
            _shelterMessageFactory.CreateFromHelpShelterMessage(command).Returns(message);

            await Act(command);

            await _repository.Received()
                .AddAsync(Arg.Is<ShelterMessage>(m =>
                    m.Id.Value == message.Id.Value && m.ShelterId == message.ShelterId && m.UserId == message.UserId &&
                    m.ShelterId == message.ShelterId &&
                    m.FullName.Value == message.FullName.Value && m.PhoneNumber.Value == message.PhoneNumber.Value));

            await _eventProcessor.Received().ProcessAsync(Arg.Is<IEnumerable<IDomainEvent>>(e
                => e.FirstOrDefault().GetType() == typeof(ShelterMessageCreated)));
        }

        [Fact]
        public async void pet_service_return_empty_guid_should_throw_an_exception()
        {
            ShelterMessage message = Extensions.ArrangeShelterMessage();
            HelpType helpType = HelpType.Walk;

            CreateHelpShelterMessage command = new CreateHelpShelterMessage(message.Id.Value, message.UserId,
                message.ShelterId, helpType, message.Description.Value, message.FullName.Value,
                message.PhoneNumber.Value);

            _shelterRepository.GetAsync(message.ShelterId).Returns((Shelter) null);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ShelterDoesNotExistsException>();
        }
    }
}