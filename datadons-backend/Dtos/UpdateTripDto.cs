namespace Dtos
{
    public class UpdateTripDto
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
    }

}