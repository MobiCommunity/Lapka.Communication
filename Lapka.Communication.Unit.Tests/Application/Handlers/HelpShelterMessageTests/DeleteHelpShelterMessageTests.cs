using System;
using System.Threading.Tasks;
using Lapka.Communication.Application.Commands;
using Lapka.Communication.Application.Commands.Handlers;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Core.Entities;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Lapka.Communication.Unit.Tests.Application.Handlers.HelpShelterMessageTests
{
    public class DeleteHelpShelterMessageTests
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcIdentityService _grpcIdentityService;
        private readonly DeleteHelpShelterMessageHandler _messageHandler;
        private readonly IHelpShelterMessageRepository _helpShelterMessageRepository;

        public DeleteHelpShelterMessageTests()
        {
            _helpShelterMessageRepository = Substitute.For<IHelpShelterMessageRepository>();
            _grpcIdentityService = Substitute.For<IGrpcIdentityService>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _messageHandler =
                new DeleteHelpShelterMessageHandler(_eventProcessor, _grpcIdentityService,
                    _helpShelterMessageRepository);
        }

        private Task Act(DeleteHelpShelterMessage command)
        {
            return _messageHandler.HandleAsync(command);
        }

        [Fact]
        public async void given_valid_help_shelter_message_as_shelter_should_be_deleted()
        {
            HelpShelterMessage message = Extensions.ArrangeHelpShelterMessage();

            _helpShelterMessageRepository.GetAsync(message.Id.Value).Returns(message);
            _grpcIdentityService.IsUserOwnerOfShelterAsync(message.ShelterId, message.UserId).Returns(true);

            DeleteHelpShelterMessage command = new DeleteHelpShelterMessage(message.Id.Value, message.UserId);

            await Act(command);

            await _helpShelterMessageRepository.Received().DeleteAsync(message);
            await _eventProcessor.Received().ProcessAsync(message.Events);
        }

        [Fact]
        public async void given_valid_help_shelter_message_as_user_should_be_deleted()
        {
            HelpShelterMessage message = Extensions.ArrangeHelpShelterMessage();

            _helpShelterMessageRepository.GetAsync(message.Id.Value).Returns(message);
            _grpcIdentityService.IsUserOwnerOfShelterAsync(message.ShelterId, message.UserId).Returns(false);

            DeleteHelpShelterMessage command = new DeleteHelpShelterMessage(message.Id.Value, message.UserId);

            await Act(command);

            await _helpShelterMessageRepository.Received().DeleteAsync(message);
            await _eventProcessor.Received().ProcessAsync(message.Events);
        }

        [Fact]
        public async void given_invalid_help_shelter_message_as_user_should_thrown_exception()
        {
            HelpShelterMessage message = Extensions.ArrangeHelpShelterMessage();

            _helpShelterMessageRepository.GetAsync(message.Id.Value).Returns(message);
            _grpcIdentityService.IsUserOwnerOfShelterAsync(message.ShelterId, message.UserId).Returns(false);

            DeleteHelpShelterMessage command = new DeleteHelpShelterMessage(message.Id.Value, Guid.NewGuid());

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<UserDoesNotOwnMessageException>();
        }
    }
}