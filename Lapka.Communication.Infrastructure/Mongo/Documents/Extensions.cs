using System;
using System.Linq;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Exceptions;
using Lapka.Communication.Core.ValueObjects;
using Lapka.Communication.Core.ValueObjects.Locations;

namespace Lapka.Communication.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static ShelterMessage AsBusiness(this ShelterMessageDocument message)
        {
            return new ShelterMessage(message.Id, message.UserId, message.ShelterId, message.IsRead, message.Title,
                new MessageDescription(message.Description), new FullName(message.FullName),
                new PhoneNumber(message.PhoneNumber), message.CreatedAt, message.PhotoPaths);
        }

        public static ShelterDocument AsDocument(this Shelter shelter)
        {
            return new ShelterDocument
            {
                Id = shelter.Id.Value,
                Location = shelter.Location.AsDocument(),
                Owners = shelter.Owners
            };
        }

        public static LocationDocument AsDocument(this Location location)
        {
            return new LocationDocument
            {
                Longitude = location.Longitude.AsDouble(),
                Latitude = location.Latitude.AsDouble()
            };
        }

        public static Location AsBusiness(this LocationDocument location)
        {
            return new Location(location.Latitude.ToString(), location.Longitude.ToString());
        }

        public static Shelter AsBusiness(this ShelterDocument shelter)
        {
            return new Shelter(shelter.Id, shelter.Location.AsBusiness(), shelter.Owners);
        }

        public static ShelterPet AsBusiness(this ShelterPetDocument shelter)
        {
            return new ShelterPet(shelter.Id, shelter.PetName, shelter.Race, shelter.BirthDate, shelter.PhotoPaths);
        }
        
        public static ShelterPetDocument AsDocument(this ShelterPet shelter)
        {
            return new ShelterPetDocument
            {
                Id = shelter.Id,
                PetName = shelter.PetName,
                Race = shelter.Race,
                BirthDate = shelter.BirthDate,
                PhotoPaths = shelter.PhotoPaths
            };
        }

        public static ShelterMessageDocument AsDocument(this ShelterMessage message)
        {
            return new ShelterMessageDocument
            {
                Id = message.Id.Value,
                UserId = message.UserId,
                ShelterId = message.ShelterId,
                IsRead = message.IsRead,
                Title = message.Title,
                Description = message.Description.Value,
                FullName = message.FullName.Value,
                PhoneNumber = message.PhoneNumber.Value,
                CreatedAt = message.CreatedAt,
                PhotoPaths = message.Photos
            };
        }

        public static ShelterMessageDto AsDto(this ShelterMessageDocument message)
        {
            return new ShelterMessageDto
            {
                Id = message.Id,
                UserId = message.UserId,
                ShelterId = message.ShelterId,
                Description = message.Description,
                FullName = message.FullName,
                PhoneNumber = message.PhoneNumber,
                CreatedAt = message.CreatedAt,
                IsRead = message.IsRead,
                PhotoPaths = message.PhotoPaths
            };
        }

        public static UserDetailedConversationDto AsDetailDto(this UserConversationDocument message, Guid userId)
        {
            return new UserDetailedConversationDto
            {
                Id = message.Id,
                Messages = message.Messages.Select(x => x.AsDto(userId)).ToList()
            };
        }

        public static UserConversation AsBusiness(this UserConversationDocument conversation)
        {
            return new UserConversation(conversation.Id, conversation.Members,
                conversation.Messages.Select(x => x.AsBusiness()).ToList());
        }

        public static UserConversationDocument AsDocument(this UserConversation conversation)
        {
            return new UserConversationDocument
            {
                Id = conversation.Id.Value,
                Members = conversation.Members,
                Messages = conversation.Messages.Select(x => x.AsDocument()).ToList()
            };
        }

        public static UserMessage AsBusiness(this UserMessageDocument message)
        {
            return new UserMessage(message.SenderUserId, message.Message, message.IsReadByReceiver, message.CreatedAt);
        }

        public static UserMessageDocument AsDocument(this UserMessage message)
        {
            return new UserMessageDocument
            {
                SenderUserId = message.SenderUserId,
                Message = message.Message,
                IsReadByReceiver = message.IsReadByReceiver,
                CreatedAt = message.CreatedAt,
            };
        }

        public static UserMessageDto AsDto(this UserMessageDocument message, Guid userId)
        {
            return new UserMessageDto
            {
                Message = message.Message,
                CreatedAt = message.CreatedAt,
                IsUserSender = message.SenderUserId == userId
            };
        }

        public static UserBasicConversationDto AsBasicDto(this UserConversationDocument conversation, Guid userId)
        {
            UserMessageDocument lastMessage =
                conversation.Messages.OrderByDescending(x => x.CreatedAt).FirstOrDefault();

            return new UserBasicConversationDto
            {
                ConversationId = conversation.Id,
                LastMessage = lastMessage.Message,
                IsLastMeesageReadByReceiver = lastMessage.IsReadByReceiver,
                IsUserReceiverOfLastMessage = lastMessage.SenderUserId != userId,
                LastMessageCreation = lastMessage.CreatedAt
            };
        }

        public static UploadPhotoRequest.Types.Bucket AsGrpcUpload(this BucketName bucket)
        {
            return bucket switch
            {
                BucketName.PetPhotos => UploadPhotoRequest.Types.Bucket.PetPhotos,
                BucketName.ShelterPhotos => UploadPhotoRequest.Types.Bucket.ShelterPhotos,
                BucketName.UserPhotos => UploadPhotoRequest.Types.Bucket.UserPhotos,
                _ => throw new InvalidBucketNameException(bucket.ToString())
            };
        }

        public static DeletePhotoRequest.Types.Bucket AsGrpcDelete(this BucketName bucket)
        {
            return bucket switch
            {
                BucketName.PetPhotos => DeletePhotoRequest.Types.Bucket.PetPhotos,
                BucketName.ShelterPhotos => DeletePhotoRequest.Types.Bucket.ShelterPhotos,
                BucketName.UserPhotos => DeletePhotoRequest.Types.Bucket.UserPhotos,
                _ => throw new InvalidBucketNameException(bucket.ToString())
            };
        }
    }
}