using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Communication.Application.Commands.ShelterMessages;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Application.Services.Grpc;
using Lapka.Communication.Application.Services.Repositories;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.ValueObjects;
using Lapka.Communication.Core.ValueObjects.Locations;

namespace Lapka.Communication.Application.Commands.Handlers.ShelterMessages
{
    public class CreateStrayPetMessageHandler : ICommandHandler<CreateStrayPetMessage>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _photoService;
        private readonly IShelterMessageRepository _repository;
        private readonly IShelterMessageFactory _messageFactory;
        private readonly IShelterRepository _shelterRepository;

        public CreateStrayPetMessageHandler(IEventProcessor eventProcessor, IGrpcPhotoService photoService,
            IShelterMessageRepository repository, IShelterMessageFactory messageFactory,
            IShelterRepository shelterRepository)
        {
            _eventProcessor = eventProcessor;
            _photoService = photoService;
            _repository = repository;
            _messageFactory = messageFactory;
            _shelterRepository = shelterRepository;
        }

        public async Task HandleAsync(CreateStrayPetMessage command)
        {
            Guid shelterId = await GetClosestShelterIdAsync(command);

            IEnumerable<string> paths = await AddPhotosToMinioAsync(command);
            
            ShelterMessage message = await _messageFactory.CreateFromStrayPetMessageAsync(command, shelterId, paths);

            await _repository.AddAsync(message);
            await _eventProcessor.ProcessAsync(message.Events);
        }

        private async Task<Guid> GetClosestShelterIdAsync(CreateStrayPetMessage command)
        {
            IEnumerable<Shelter> shelters = await _shelterRepository.GetAllAsync();

            Shelter shelter = shelters.ToList().OrderBy(x =>
                x.Location.CalculateDistance(new Location(command.Location.Latitude.Value,
                    command.Location.Longitude.Value))).FirstOrDefault();

            if (shelter is null)
            {
                throw new ErrorDuringFindingClosestShelterException();
            }

            return shelter.Id.Value;
        }

        private async Task<IEnumerable<string>> AddPhotosToMinioAsync(CreateStrayPetMessage command)
        {
            Collection<string> paths = new Collection<string>();

            try
            {
                foreach (File photo in command.Photos)
                {
                    paths.Add(await _photoService.AddAsync(photo.Name, command.UserId, true, photo.Content,
                        BucketName.PetPhotos));
                }
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }

            return paths;
        }
    }
}