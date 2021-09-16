using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Communication.Application.Commands;
using Lapka.Communication.Application.Commands.Handlers;
using Lapka.Communication.Application.Commands.Handlers.ShelterMessages;
using Lapka.Communication.Application.Commands.ShelterMessages;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Application.Services.Grpc;
using Lapka.Communication.Application.Services.Repositories;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Events.Abstract;
using Lapka.Communication.Core.Events.Concrete;
using Lapka.Communication.Core.ValueObjects;
using Lapka.Communication.Core.ValueObjects.Locations;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Lapka.Communication.Unit.Tests.Application.Handlers.StrayPetMessageTests
{
    public class CreateStrayPetMessageTests
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcIdentityService _grpcIdentityService;
        private readonly CreateStrayPetMessageHandler _handler;
        private readonly IShelterMessageRepository _repository;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly IShelterMessageFactory _shelterMessageFactory;

        public CreateStrayPetMessageTests()
        {
            _grpcPhotoService = Substitute.For<IGrpcPhotoService>();
            _grpcIdentityService = Substitute.For<IGrpcIdentityService>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _repository = Substitute.For<IShelterMessageRepository>();
            _shelterMessageFactory = Substitute.For<IShelterMessageFactory>();
            _handler = new CreateStrayPetMessageHandler(_eventProcessor, _grpcIdentityService, _grpcPhotoService,
                _repository, _shelterMessageFactory);
        }

        private Task Act(CreateStrayPetMessage command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async void given_valid_stray_pet_message_should_be_created()
        {
            Location location = Extensions.ArrangeLocation();
            List<PhotoFile> photos = new List<PhotoFile>
            {
                Extensions.ArrangePhotoFile(),
                Extensions.ArrangePhotoFile()
            };
            ShelterMessage message = Extensions.ArrangeShelterMessage();

            CreateStrayPetMessage command = new CreateStrayPetMessage(message.Id.Value, message.UserId,
                location, photos, message.Description.Value, message.FullName.Value, message.PhoneNumber.Value);

            _grpcIdentityService.ClosestShelterAsync(command.Location.Longitude.Value, command.Location.Latitude.Value)
                .Returns(message.ShelterId);
            _shelterMessageFactory.CreateStrayPetMessage(command, message.ShelterId).Returns(message);

            await Act(command);

            await _repository.Received()
                .AddAsync(Arg.Is<ShelterMessage>(m =>
                    m.Id.Value == message.Id.Value && m.ShelterId == message.ShelterId && m.UserId == message.UserId &&
                    m.ShelterId == message.ShelterId && m.FullName.Value == message.FullName.Value &&
                    m.PhoneNumber.Value == message.PhoneNumber.Value));

            await _eventProcessor.Received().ProcessAsync(Arg.Is<IEnumerable<IDomainEvent>>(e
                => e.FirstOrDefault().GetType() == typeof(ShelterMessageCreated)));
        }

        [Fact]
        public async void given_valid_stray_pet_message_but_did_not_found_closest_shelter_should_throw_an_exception()
        {
            Location location = Extensions.ArrangeLocation();
            List<PhotoFile> photos = new List<PhotoFile>
            {
                Extensions.ArrangePhotoFile(),
                Extensions.ArrangePhotoFile()
            };
            ShelterMessage message = Extensions.ArrangeShelterMessage();

            CreateStrayPetMessage command = new CreateStrayPetMessage(message.Id.Value, message.UserId,
                location, photos, message.Description.Value, message.FullName.Value, message.PhoneNumber.Value);

            _grpcIdentityService.ClosestShelterAsync(command.Location.Longitude.Value, command.Location.Latitude.Value)
                .Returns(Guid.Empty);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ErrorDuringFindingClosestShelterException>();
        }
    }
}