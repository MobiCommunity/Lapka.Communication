using Lapka.Communication.Core.Exceptions.Abstract;

namespace Lapka.Communication.Core.Exceptions.Location
{
    public class LongitudeTooBigException : DomainException
    {
        public string Longitude { get; }
        public LongitudeTooBigException(string longitude) : base($"Longitude is too big: {longitude}")
        {
            Longitude = longitude;
        }

        public override string Code => "longitude_too_big";
    }
}