using Models;

namespace Dtos
{
    public class PreferenceForTripDTO
    {
        public bool NoPets { get; set; }
        public bool NoLuggage { get; set; }
        public bool NoFood { get; set; }
        public bool NoDrinks { get; set; }
        public bool NoSmoking { get; set; }
        public long TripId { get; set; }
    }

}
