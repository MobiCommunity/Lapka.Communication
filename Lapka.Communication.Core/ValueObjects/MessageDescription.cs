using Lapka.Communication.Core.Exceptions;

namespace Lapka.Communication.Core.ValueObjects
{
    public class MessageDescription
    {
        public string Value { get; }

        public MessageDescription(string description)
        {
            ValidateDescription(description);
            
            Value = description;
        }

        private void ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new InvalidDescriptionValueException(description);
            }

            if (description.Length < MinimumDescriptionLength)
            {
                throw new TooShortDescriptionException(description, MinimumDescriptionLength);
            }
        }

        private const int MinimumDescriptionLength = 30;
    }
}