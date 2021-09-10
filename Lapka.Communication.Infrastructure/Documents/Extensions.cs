using System;
using System.Linq;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Exceptions;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Infrastructure.Documents
{
    public static class Extensions
    {
        public static AdoptPetMessage AsBusiness(this AdoptPetMessageDocument message)
        {
            return new AdoptPetMessage(message.Id, message.UserId, message.ShelterId, message.PetId,
                message.Description, message.FullName, message.PhoneNumber, message.CreatedAt);
        }

        public static AdoptPetMessageDocument AsDocument(this AdoptPetMessage message)
        {
            return new AdoptPetMessageDocument
            {
                Id = message.Id.Value,
                UserId = message.UserId,
                ShelterId = message.ShelterId,
                PetId = message.PetId,
                Description = message.Description,
                FullName = message.FullName,
                PhoneNumber = message.PhoneNumber,
                CreatedAt = message.CreatedAt
            };
        }

        public static AdoptPetMessageDto AsDto(this AdoptPetMessageDocument message)
        {
            return new AdoptPetMessageDto
            {
                Id = message.Id,
                UserId = message.UserId,
                ShelterId = message.ShelterId,
                PetId = message.PetId,
                Description = message.Description,
                FullName = message.FullName,
                PhoneNumber = message.PhoneNumber,
                CreatedAt = message.CreatedAt
            };
        }
        
        public static StrayPetMessage AsBusiness(this StrayPetMessageDocument message)
        {
            return new StrayPetMessage(message.Id, message.UserId, message.ShelterId, message.PhotoIds,
                message.Description, message.FullName, message.PhoneNumber, message.CreatedAt);
        }

        public static StrayPetMessageDocument AsDocument(this StrayPetMessage message)
        {
            return new StrayPetMessageDocument
            {
                Id = message.Id.Value,
                UserId = message.UserId,
                ShelterId = message.ShelterId,
                PhotoIds = message.PhotoIds,
                Description = message.Description,
                FullName = message.FullName,
                PhoneNumber = message.PhoneNumber,
                CreatedAt = message.CreatedAt
            };
        }

        public static StrayPetMessageDto AsDto(this StrayPetMessageDocument message)
        {
            return new StrayPetMessageDto
            {
                Id = message.Id,
                UserId = message.UserId,
                ShelterId = message.ShelterId,
                PhotoIds = message.PhotoIds,
                Description = message.Description,
                FullName = message.FullName,
                PhoneNumber = message.PhoneNumber,
                CreatedAt = message.CreatedAt
            };
        }

        public static UserDetailedConversationDto AsDto(this UserConversationDocument message, Guid userId)
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
            return new UserMessage(message.SenderUserId, message.Message, message.CreatedAt);
        }

        public static UserMessageDocument AsDocument(this UserMessage message)
        {
            return new UserMessageDocument
            {
                SenderUserId = message.SenderUserId,
                Message = message.Message,
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