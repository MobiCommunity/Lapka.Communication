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
        private AdoptPetMessage Act(AggregateId id, Guid userId, Guid shelterId, Guid petId, string description,
            string fullName, string phoneNumber) => AdoptPetMessage.Create(id.Value, userId, shelterId, petId,
            description, fullName, phoneNumber);

        [Fact]
        public void given_valid_adopt_message_should_be_created()
        {
            AdoptPetMessage arrangeShelter = Extensions.ArrangeAdoptPetMessage();

            AdoptPetMessage shelter = Act(arrangeShelter.Id, arrangeShelter.UserId, arrangeShelter.ShelterId,
                arrangeShelter.PetId, arrangeShelter.Description.Value, arrangeShelter.FullName.Value,
                arrangeShelter.PhoneNumber.Value);

            shelter.ShouldNotBeNull();
            shelter.Id.ShouldBe(arrangeShelter.Id);
            shelter.UserId.ShouldBe(arrangeShelter.UserId);
            shelter.ShelterId.ShouldBe(arrangeShelter.ShelterId);
            shelter.PetId.ShouldBe(arrangeShelter.PetId);
            shelter.Description.Value.ShouldBe(arrangeShelter.Description.Value);
            shelter.FullName.Value.ShouldBe(arrangeShelter.FullName.Value);
            shelter.PhoneNumber.Value.ShouldBe(arrangeShelter.PhoneNumber.Value);
            shelter.Events.Count().ShouldBe(1);
            IDomainEvent @event = shelter.Events.Single();
            @event.ShouldBeOfType<AdoptPetMessageCreated>();
        }
        
        [Fact]
        public void given_invalid_adopt_petId_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                Extensions.ArrangeAdoptPetMessage(petId: Guid.Empty));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidPetIdValueException>();
        }
        
        [Fact]
        public void given_invalid_adopt_userId_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                Extensions.ArrangeAdoptPetMessage(userId: Guid.Empty));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidUserIdValueException>();
        }
        
        [Fact]
        public void given_invalid_adopt_shelterId_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                Extensions.ArrangeAdoptPetMessage(shelterId: Guid.Empty));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidShelterIdValueException>();
        }
    }
}