using Lapka.Communication.Core.Exceptions.Abstract;

namespace Lapka.Communication.Core.Exceptions.Location
{
    public class LongitudeIncorrectDataTypeException : DomainException
    {
        public string Longitude { get; }
        public LongitudeIncorrectDataTypeException(string longitude) : base($"Invalid data type of longitude: {longitude}")
        {
            Longitude = longitude;
        }

        public override string Code => "invalid_longitude_data_type";
    }
}