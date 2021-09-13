using System;
using Lapka.Communication.Core.Exceptions;
using Lapka.Communication.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Lapka.Communication.Unit.Tests.Core.Entities
{
    public class FullNameTests
    {
        private FullName Act(string fullName) => new FullName(fullName);

        [Fact]
        public void given_valid_message_description_should_be_created()
        {
            string validFullName = "Hi Ha";
            
            FullName fullName = Act(validFullName);

            fullName.ShouldNotBeNull();
            fullName.Value.ShouldBe(validFullName);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void given_invalid_full_name_should_throw_an_exception(string fullName)
        {
            Exception exception = Record.Exception(() => Act(fullName));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidFullNameValueException>();
        }
        
        [Fact]
        public void given_too_short_full_name_should_throw_an_exception()
        {
            string invalidFullName = "Mik";
            
            Exception exception = Record.Exception(() => Act(invalidFullName));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TooShortFullNameException>();
        }
    }
}