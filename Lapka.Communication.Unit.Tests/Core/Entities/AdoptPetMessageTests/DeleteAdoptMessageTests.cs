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
    public class DeleteAdoptMessageTests
    {
        [Fact]
        public void given_valid_adopt_message_should_be_created()
        {
            AdoptPetMessage message = Extensions.ArrangeAdoptPetMessage();

            message.ShouldNotBeNull();
            message.Delete();
            
            message.Events.Count().ShouldBe(1);
            IDomainEvent @event = message.Events.Single();
            @event.ShouldBeOfType<AdoptPetMessageDeleted>();
        }
    }
}