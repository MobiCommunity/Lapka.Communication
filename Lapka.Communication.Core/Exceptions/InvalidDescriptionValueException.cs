using Lapka.Communication.Core.Exceptions.Abstract;

namespace Lapka.Communication.Core.Exceptions
{
    public class InvalidDescriptionValueException : DomainException
    {
        public string Description { get; }
        public InvalidDescriptionValueException(string description) : base($"Invalid description: {description}")
        {
            Description = description;
        }
        public override string Code => "invalid_full_name_value";
    }
}