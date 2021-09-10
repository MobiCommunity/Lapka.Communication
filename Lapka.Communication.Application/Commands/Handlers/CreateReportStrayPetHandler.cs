using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Application.Commands.Handlers
{
    public class CreateReportStrayPetHandler : ICommandHandler<CreateReportStrayPet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcIdentityService _grpcIdentityService;
        private readonly IGrpcPhotoService _photoService;
        private readonly IStrayPetMessageRepository _repository;


        public CreateReportStrayPetHandler(IEventProcessor eventProcessor, IGrpcIdentityService grpcIdentityService,
            IGrpcPhotoService photoService, IStrayPetMessageRepository repository)
        {
            _eventProcessor = eventProcessor;
            _grpcIdentityService = grpcIdentityService;
            _photoService = photoService;
            _repository = repository;
        }

        public async Task HandleAsync(CreateReportStrayPet command)
        {
            Guid shelterId = await GetClosestShelterIdAsync(command);

            StrayPetMessage message = StrayPetMessage.Create(command.Id, command.UserId, shelterId,
                command.Photos.IdsAsGuidList(), command.Description, command.ReporterName, command.ReporterPhoneNumber);

            await AddPhotosAsync(command);
            await _repository.AddAsync(message);
            await _eventProcessor.ProcessAsync(message.Events);
        }

        private async Task<Guid> GetClosestShelterIdAsync(CreateReportStrayPet command)
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

        private async Task AddPhotosAsync(CreateReportStrayPet command)
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