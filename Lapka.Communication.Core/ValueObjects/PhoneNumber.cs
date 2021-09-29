using System.Text.RegularExpressions;
using Lapka.Communication.Core.Exceptions;

namespace Lapka.Communication.Core.ValueObjects
{
    public class PhoneNumber
    {
        public string Value { get; }

        public PhoneNumber(string phoneNumber)
        {
            Value = phoneNumber;

            Validate();
        }

        private void Validate()
        {
            if (!PhoneNumberRegex.IsMatch(Value))
            {
                throw new InvalidPhoneNumberException(Value);
            }
        }

        private static readonly Regex PhoneNumberRegex = new Regex(PhoneNumberRegexValue,
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private const string PhoneNumberRegexValue = @"(?<!\w)(\(?(\+|00)?48\)?)?[ -]?\d{3}[ -]?\d{3}[ -]?\d{3}(?!\w)";
    }
}