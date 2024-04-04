using Dtos;
using Models;

namespace Data
{
    public interface IRepo
    {
        //* Users Methods
        string AddUser(User user);
        User GetUser(long id);
        User[] GetAllUsers();

        // Methods to remove and add a driver to a user
        User AddDriverToUser(long userId, Driver driver);
        User RemoveDriverFromUser(long userId);
        Review AddReviewToUser(long userId, Review review);
        List<Review> getIncomingReviewsForUser(long userId);
        public Driver GetDriverByUserId(long UserId);
        public DriverDto ToDriverDto(Driver driver);

        User GetUserByUsername(string username);
        public User GetUserFromDriverId(long driverId);

        //* Trips Methods 
        Trip GetTrip(long id);
        Trip[] GetAllTrips();
        IEnumerable<Trip> GetAllTripsWithGPS();
        double getGpsLon(long id);
        double getGpsLat(long id);
        Trip[] SearchTrips(double? startLatitude, double? startLongitude, double? endLatitude, double? endLongitude, string date, string time, int seats);
        Trip[] GetAllTripsBy(long driverID);
        void DeleteTrip(long id);
        void UpdateTrip(UpdateTripDto trip);
        long AddTrip(TripDto tripDto);
        public Driver GetDriver(long driverId);
        public PreferenceForTripDTO GetPreferenceByTripId(int tripId);
        void SetPreferenceForTrip(int driverId, Preference preference);
        PreferenceForTripDTO AddPreferenceToTrip(long tripId, Preference preference);
        List<Preference> GetPreferencesForTrip(long tripId);
        public PreferenceForTripDTO GetPreferenceByPreferenceId(int preferenceId);
        void RemovePreferenceFromTrip(long preferenceId);
        public List<PreferenceForTripDTO> ConvertToDtoList(List<Preference> preferences);

    }
}