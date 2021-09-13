using Lapka.Communication.Core.Exceptions.Abstract;

namespace Lapka.Communication.Core.Exceptions
{
    public class InvalidPhoneNumberException : DomainException
    {
        public string PhoneNumber { get; }
        public InvalidPhoneNumberException(string phoneNumber) : base($"Invalid phone number: {phoneNumber}")
        {
            PhoneNumber = phoneNumber;
        }

        public override string Code => "invalid_phone_number";
    }
}