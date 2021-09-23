using System;
using System.Collections.Generic;
using GeoCoordinatePortable;
using Lapka.Communication.Core.ValueObjects;
using Lapka.Communication.Core.ValueObjects.Locations;

namespace Lapka.Communication.Application.Dto
{
    public static class Extensions
    {

        public static double CalculateDistance(this Location startingLocation, Location destinationLocation)
        {
            GeoCoordinate pin1 = new GeoCoordinate(startingLocation.Latitude.AsDouble(),
                startingLocation.Longitude.AsDouble());
            GeoCoordinate pin2 = new GeoCoordinate(destinationLocation.Latitude.AsDouble(), destinationLocation.Longitude.AsDouble());
            return pin1.GetDistanceTo(pin2);
        }
    }
}