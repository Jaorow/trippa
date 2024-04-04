namespace Dtos
{
    public class TripDto
    {
        public long TripID { get; set; }

        public long DriverID { get; set; }
        public DateTime DateTime { get; set; }
        public long MaxRiders { get; set; }
        public double Price { get; set; }
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public double EndLatitude { get; set; }
        public double EndLongitude { get; set; }
        public double DetourRange { get; set; }
        public string startLocation { get; set; }
        public string endLocation { get; set; }
    }
    public class TripOutDto
    {
        public long TripID { get; set; }
        public long DriverID { get; set; }
        public DateTime DateTime { get; set; }
        public long MaxRiders { get; set; }
        public double Price { get; set; }
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public double EndLatitude { get; set; }
        public double EndLongitude { get; set; }
        public double DetourRange { get; set; }
        public long CurrentRiders { get; set; }
        public string DriverName { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }

    }
}