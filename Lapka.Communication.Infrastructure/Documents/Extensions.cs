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
        public static ShelterMessage AsBusiness(this ShelterMessageDocument message)
        {
            return new ShelterMessage(message.Id, message.UserId, message.ShelterId, message.IsRead, message.Title,
                new MessageDescription(message.Description), new FullName(message.FullName),
                new PhoneNumber(message.PhoneNumber), message.CreatedAt);
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
                CreatedAt = message.CreatedAt
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
                CreatedAt = message.CreatedAt
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
            return new UserMessage(message.SenderUserId,  message.Message, message.IsReadByReceiver, message.CreatedAt);
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