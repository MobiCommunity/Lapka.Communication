using System;
using Lapka.Communication.Core.Exceptions;
using Lapka.Communication.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Lapka.Communication.Unit.Tests.Core.Entities
{
    public class PhoneNumberTests
    {
        private PhoneNumber Act(string phoneNumber) => new PhoneNumber(phoneNumber);

        [Fact]
        public void given_valid_phone_number_should_be_created()
        {
            string validPhoneNumber = "123123123";
            
            PhoneNumber phoneNumber = Act(validPhoneNumber);

            phoneNumber.ShouldNotBeNull();
            phoneNumber.Value.ShouldBe(validPhoneNumber);
        }
        
        [Theory]
        [InlineData("12312312")]
        [InlineData("1231231231")]
        [InlineData("")]
        [InlineData(" ")]
        public void given_invalid_phone_number_should_throw_an_exception(string invalidPhoneNumber)
        {
            Exception exception = Record.Exception(() => Act(invalidPhoneNumber));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidPhoneNumberException>();
        }
    }
}