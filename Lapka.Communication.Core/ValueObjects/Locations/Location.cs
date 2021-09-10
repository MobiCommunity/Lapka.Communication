namespace Lapka.Communication.Core.ValueObjects.Locations
{
    public class Location
    {
        public Latitude Latitude { get; }
        public Longitude Longitude { get; }

        public Location(string latitude, string longitude)
        {
            Latitude = new Latitude(latitude);
            Longitude = new Longitude(longitude);
        }
    }
}