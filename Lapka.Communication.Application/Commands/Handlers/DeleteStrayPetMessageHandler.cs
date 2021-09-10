using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Communication.Application.Commands.Handlers
{
    public class DeleteStrayPetMessageHandler : ICommandHandler<DeleteStrayPetMessage>
    {
        private readonly ILogger<DeleteStrayPetMessageHandler> _logger;
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcIdentityService _grpcIdentityService;
        private readonly IGrpcPhotoService _photoService;
        private readonly IStrayPetMessageRepository _repository;


        public DeleteStrayPetMessageHandler(ILogger<DeleteStrayPetMessageHandler> logger,
            IEventProcessor eventProcessor, IGrpcIdentityService grpcIdentityService, IGrpcPhotoService photoService,
            IStrayPetMessageRepository repository)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
            _grpcIdentityService = grpcIdentityService;
            _photoService = photoService;
            _repository = repository;
        }

        public async Task HandleAsync(DeleteStrayPetMessage command)
        {
            StrayPetMessage message = await _repository.GetAsync(command.Id);
            if (message is null)
            {
                throw new MessageNotFoundException(command.Id);
            }

            await ValidIfUserCanDeleteMessageAsync(command, message);

            message.Delete();
            await DeletePhotosAsync(message);
            
            await _repository.DeleteAsync(message);
            await _eventProcessor.ProcessAsync(message.Events);
        }

        private async Task ValidIfUserCanDeleteMessageAsync(DeleteStrayPetMessage command, StrayPetMessage message)
        {
            bool isUserOwner = await _grpcIdentityService.IsUserOwnerOfShelterAsync(message.ShelterId, command.UserId);
            if (!isUserOwner && message.UserId != command.UserId)
            {
                throw new UserDoesNotOwnMessageException(command.UserId, message.Id.Value);
            }
        }
        
        private async Task DeletePhotosAsync(StrayPetMessage message)
        {
            try
            {
                foreach (Guid photoId in message.PhotoIds)
                {
                    await _photoService.DeleteAsync(photoId, BucketName.PetPhotos);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}