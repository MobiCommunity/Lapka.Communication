using Lapka.Communication.Core.Exceptions.Abstract;

namespace Lapka.Communication.Core.Exceptions
{
    public class TooShortFullNameException : DomainException
    {
        public string FullName { get; }
        public int MinimumLength { get; }

        public TooShortFullNameException(string fullName, int minimumLength) : base(
            $"Too short full name: {fullName}. Minimum length is {minimumLength}")
        {
            FullName = fullName;
            MinimumLength = minimumLength;
        }

        public override string Code => "too_short_full_name_value";
    }
}