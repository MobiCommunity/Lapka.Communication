using Lapka.Communication.Core.Exceptions.Abstract;

namespace Lapka.Communication.Core.Exceptions
{
    public class TooShortDescriptionException : DomainException
    {
        public string Description { get; }
        public int MinimumLength { get; }

        public TooShortDescriptionException(string description, int minimumLength) : base(
            $"Too short description: {description}. Minimum length is {minimumLength}")
        {
            Description = description;
            MinimumLength = minimumLength;
        }

        public override string Code => "too_short_description_value";
    }
}