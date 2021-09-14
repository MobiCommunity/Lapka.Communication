using System;
using System.Collections.Generic;
using System.Linq;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Events.Abstract;
using Lapka.Communication.Core.Events.Concrete;
using Lapka.Communication.Core.Exceptions;
using Lapka.Communication.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Lapka.Communication.Unit.Tests.Core.Entities.AdoptPetMessageTests
{
    public class CreateShelterTests
    {
        private ShelterMessage Act(AggregateId id, Guid userId, Guid shelterId, string title, string description,
            string fullName, string phoneNumber) => ShelterMessage.Create(id.Value, userId, shelterId, title,
            description, fullName, phoneNumber, DateTime.Now);

        [Fact]
        public void given_valid_adopt_message_should_be_created()
        {
            ShelterMessage arrangeShelter = Extensions.ArrangeShelterMessage();

            ShelterMessage shelter = Act(arrangeShelter.Id, arrangeShelter.UserId, arrangeShelter.ShelterId,
                arrangeShelter.Title, arrangeShelter.Description.Value, arrangeShelter.FullName.Value,
                arrangeShelter.PhoneNumber.Value);

            shelter.ShouldNotBeNull();
            shelter.Id.ShouldBe(arrangeShelter.Id);
            shelter.UserId.ShouldBe(arrangeShelter.UserId);
            shelter.ShelterId.ShouldBe(arrangeShelter.ShelterId);
            shelter.Title.ShouldBe(arrangeShelter.Title);
            shelter.FullName.Value.ShouldBe(arrangeShelter.FullName.Value);
            shelter.PhoneNumber.Value.ShouldBe(arrangeShelter.PhoneNumber.Value);
            shelter.Events.Count().ShouldBe(1);
            IDomainEvent @event = shelter.Events.Single();
            @event.ShouldBeOfType<ShelterMessageCreated>();
        }

        [Fact]
        public void given_invalid_adopt_userId_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                Extensions.ArrangeShelterMessage(userId: Guid.Empty));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidUserIdValueException>();
        }
        
        [Fact]
        public void given_invalid_adopt_shelterId_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                Extensions.ArrangeShelterMessage(shelterId: Guid.Empty));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidShelterIdValueException>();
        }
    }
}