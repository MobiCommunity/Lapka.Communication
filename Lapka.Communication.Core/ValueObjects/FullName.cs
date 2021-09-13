using Lapka.Communication.Core.Exceptions;

namespace Lapka.Communication.Core.ValueObjects
{
    public class FullName
    {
        public string Value { get; }

        public FullName(string fullName)
        {
            ValidateFullName(fullName);
            
            Value = fullName;
        }

        private void ValidateFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new InvalidFullNameValueException(fullName);
            }

            if (fullName.Length < MinimumFullNameLength)
            {
                throw new TooShortFullNameException(fullName, MinimumFullNameLength);
            }
        }

        private const int MinimumFullNameLength = 4;
    }
}