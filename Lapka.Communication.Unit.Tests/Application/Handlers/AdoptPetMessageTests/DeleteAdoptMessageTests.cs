using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Communication.Application.Commands;
using Lapka.Communication.Application.Commands.Handlers;
using Lapka.Communication.Application.Exceptions;
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
    public class DeleteAdoptMessageTests
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcIdentityService _grpcIdentityService;
        private readonly DeleteAdoptPetMessageHandler _messageHandler;
        private readonly IAdoptPetMessageRepository _adoptPetRepository;

        public DeleteAdoptMessageTests()
        {
            _adoptPetRepository = Substitute.For<IAdoptPetMessageRepository>();
            _grpcIdentityService = Substitute.For<IGrpcIdentityService>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _messageHandler = new DeleteAdoptPetMessageHandler(_eventProcessor, _grpcIdentityService, _adoptPetRepository);
        }

        private Task Act(DeleteAdoptPetMessage command)
        {
            return _messageHandler.HandleAsync(command);
        }

        [Fact]
        public async void given_valid_adopt_message_as_shelter_should_be_deleted()
        {
            Guid petId = Guid.NewGuid();
            AdoptPetMessage message = Extensions.ArrangeAdoptPetMessage(petId: petId);
            
            _adoptPetRepository.GetAsync(message.Id.Value).Returns(message);
            _grpcIdentityService.IsUserOwnerOfShelterAsync(message.ShelterId, message.UserId).Returns(true);
            
            DeleteAdoptPetMessage command = new DeleteAdoptPetMessage(message.Id.Value, message.UserId);

            await Act(command);

            await _adoptPetRepository.Received().DeleteAsync(message);
            await _eventProcessor.Received().ProcessAsync(message.Events);
        }

        [Fact]
        public async void given_valid_adopt_message_as_user_should_be_deleted()
        {
            Guid petId = Guid.NewGuid();
            AdoptPetMessage message = Extensions.ArrangeAdoptPetMessage(petId: petId);
            
            _adoptPetRepository.GetAsync(message.Id.Value).Returns(message);
            _grpcIdentityService.IsUserOwnerOfShelterAsync(message.ShelterId, message.UserId).Returns(false);
            
            DeleteAdoptPetMessage command = new DeleteAdoptPetMessage(message.Id.Value, message.UserId);
            
            await Act(command);

            await _adoptPetRepository.Received().DeleteAsync(message);
            await _eventProcessor.Received().ProcessAsync(message.Events);
        }

        [Fact]
        public async void given_invalid_adopt_message_as_user_should_thrown_exception()
        {
            Guid petId = Guid.NewGuid();
            AdoptPetMessage message = Extensions.ArrangeAdoptPetMessage(petId: petId);
            
            _adoptPetRepository.GetAsync(message.Id.Value).Returns(message);
            _grpcIdentityService.IsUserOwnerOfShelterAsync(message.ShelterId, message.UserId).Returns(false);
            
            DeleteAdoptPetMessage command = new DeleteAdoptPetMessage(message.Id.Value, Guid.NewGuid());
            
            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<UserDoesNotOwnMessageException>();
        }
    }
}