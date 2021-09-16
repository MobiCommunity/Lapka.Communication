﻿using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Communication.Application.Commands.ShelterMessages;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Application.Services.Grpc;
using Lapka.Communication.Application.Services.Repositories;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Application.Commands.Handlers.ShelterMessages
{
    public class CreateStrayPetMessageHandler : ICommandHandler<CreateStrayPetMessage>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcIdentityService _grpcIdentityService;
        private readonly IGrpcPhotoService _photoService;
        private readonly IShelterMessageRepository _repository;
        private readonly IShelterMessageFactory _messageFactory;


        public CreateStrayPetMessageHandler(IEventProcessor eventProcessor, IGrpcIdentityService grpcIdentityService,
            IGrpcPhotoService photoService, IShelterMessageRepository repository , IShelterMessageFactory messageFactory)
        {
            _eventProcessor = eventProcessor;
            _grpcIdentityService = grpcIdentityService;
            _photoService = photoService;
            _repository = repository;
            _messageFactory = messageFactory;
        }

        public async Task HandleAsync(CreateStrayPetMessage command)
        {
            Guid shelterId = await GetClosestShelterIdAsync(command);

            ShelterMessage message = _messageFactory.CreateStrayPetMessage(command, shelterId);

            await AddPhotosAsync(command);
            
            await _repository.AddAsync(message);
            await _eventProcessor.ProcessAsync(message.Events);
        }

        private async Task<Guid> GetClosestShelterIdAsync(CreateStrayPetMessage command)
        {
            Guid shelterId;

            try
            {
                shelterId = await _grpcIdentityService.ClosestShelterAsync(command.Location.Longitude.Value,
                    command.Location.Latitude.Value);
            }
            catch (Exception ex)
            {
                throw new CannotRequestIdentityMicroserviceException(ex);
            }

            if (shelterId == Guid.Empty)
            {
                throw new ErrorDuringFindingClosestShelterException();
            }

            return shelterId;
        }

        private async Task AddPhotosAsync(CreateStrayPetMessage command)
        {
            try
            {
                foreach (PhotoFile photo in command.Photos)
                {
                    await _photoService.AddAsync(photo.Id, photo.Name, photo.Content, BucketName.PetPhotos);
                }
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }
        }
    }
}