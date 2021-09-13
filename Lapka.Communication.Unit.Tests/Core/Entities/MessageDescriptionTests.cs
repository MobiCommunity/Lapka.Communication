using System;
using Lapka.Communication.Core.Exceptions;
using Lapka.Communication.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Lapka.Communication.Unit.Tests.Core.Entities
{
    public class MessageDescriptionTests
    {
        private MessageDescription Act(string description) => new MessageDescription(description);

        [Fact]
        public void given_valid_message_description_should_be_created()
        {
            string validValue = "This is valid description. Its valid because its long enough";
            
            MessageDescription message = Act(validValue);

            message.ShouldNotBeNull();
            message.Value.ShouldBe(validValue);
        }
        
        [Fact]
        public void given_invalid_adopt_message_description_should_throw_an_exception()
        {
            string invalidValue = "";

            Exception exception = Record.Exception(() => Act(invalidValue));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidDescriptionValueException>();
        }

        [Fact]
        public void given_too_short_adopt_message_description_should_throw_an_exception()
        {
            string invalidValue = "Too short description.";

            Exception exception = Record.Exception(() => Act(invalidValue));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TooShortDescriptionException>();
        }
    }
}