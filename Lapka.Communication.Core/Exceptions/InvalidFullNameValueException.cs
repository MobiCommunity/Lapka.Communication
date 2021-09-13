using Lapka.Communication.Core.Exceptions.Abstract;

namespace Lapka.Communication.Core.Exceptions
{
    public class InvalidFullNameValueException : DomainException
    {
        public string FullName { get; }
        public InvalidFullNameValueException(string fullName) : base($"Invalid full name: {fullName}")
        {
            FullName = fullName;
        }
        public override string Code => "invalid_full_name_value";
    }
}