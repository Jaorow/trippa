using Models;
using System;
using System.Collections.Generic;
using System.Linq;

public static class TripService
{
    public static List<Trip> GetTripsWithinRange(List<Trip> trips, GPS currentLocation, double range)
    {
        return trips.Where(trip => CalculateDistance(currentLocation, trip.StartPoint) <= trip.DetourRange + range).ToList();
    }

    //TODO - this is just a basic implementation, we need to use the Haversine formula to calculate the distance between two GPS points
    private static double CalculateDistance(GPS point1, GPS point2)
    {
        return Math.Sqrt(Math.Pow(point2.Latitude - point1.Latitude, 2) + Math.Pow(point2.Longitude - point1.Longitude, 2));
    }
}
