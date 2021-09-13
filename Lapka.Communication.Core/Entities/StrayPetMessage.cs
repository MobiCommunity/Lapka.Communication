using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Lapka.Communication.Core.Events.Concrete;
using Lapka.Communication.Core.Exceptions;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Core.Entities
{
    public class StrayPetMessage : AggregateShelterMessage
    {
        public List<Guid> PhotoIds { get; }

        public StrayPetMessage(Guid id, Guid userId, Guid shelterId, List<Guid> photoIds, MessageDescription description, FullName fullName,
            PhoneNumber phoneNumber, DateTime createdAt) : base(id, userId, shelterId, description,
            fullName, phoneNumber, createdAt)
        {
            PhotoIds = photoIds;
        }

        public static StrayPetMessage Create(Guid id, Guid userId, Guid shelterId, List<Guid> photoIds,
            string description, string fullName, string phoneNumber)
        {
            StrayPetMessage message = new StrayPetMessage(id, userId, shelterId, photoIds,
                new MessageDescription(description), new FullName(fullName), new PhoneNumber(phoneNumber),
                DateTime.Now);

            message.AddEvent(new StrayPetMessageCreated(message));
            return message;
        }

        public override void Delete()
        {
            AddEvent(new StrayPetMessageDeleted(this));
        }
    }
}