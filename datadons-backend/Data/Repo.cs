using Models;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Dtos;

namespace Data
{
    public class Repo : IRepo
    {
        private readonly DonsDbContext _repo;

        public Repo(DonsDbContext repo)
        {
            _repo = repo;
        }

        //* Users Methods
        public string AddUser(User user)
        {
            _repo.Users.Add(user);
            _repo.SaveChanges();
            return user.Username;
        }

        public User GetUser(long id)
        {
            return _repo.Users.FirstOrDefault(u => u.Id == id);
        }

        public User[] GetAllUsers()
        {
            User[] c = _repo.Users.ToArray();
            return c;
        }

        public User AddDriverToUser(long userId, Driver driver)
        {
            var user = _repo.Users.Include(u => u.Driver).FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.Driver = driver;
                _repo.SaveChanges();
            }
            return user;
        }

        public DriverDto ToDriverDto(Driver driver)
        {
            return new DriverDto
            {
                UserId = driver.UserId,
                LicenseNumber = driver.LicenseNumber,
                CarModel = driver.Car?.Model,
                CarColor = driver.Car?.Color,
                CarMake = driver.Car?.Make,
                CarType = driver.Car?.Type,
                PlateNumber = driver.Car?.LicensePlate
            };
        }

        public Driver GetDriverByUserId(long userId)
        {
            return _repo.Drivers
                .Include(d => d.Car)
                .FirstOrDefault(d => d.UserId == userId);
        }

        public IEnumerable<Trip> GetAllTripsWithGPS()
        {
            return _repo.Trips.Include(t => t.StartPoint).Include(t => t.EndPoint).ToArray();
        }

        public double getGpsLon(long id)
        {
            return _repo.GPS.FirstOrDefault(g => g.Id == id).Longitude;
        }

        public double getGpsLat(long id)
        {
            return _repo.GPS.FirstOrDefault(g => g.Id == id).Latitude;
        }

        public User GetUserFromDriverId(long driverId)
        {
            return _repo.Users.FirstOrDefault(u => u.Driver.Id == driverId);
        }

        public User RemoveDriverFromUser(long userId)
        {
            var user = _repo.Users.Include(u => u.Driver).FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.Driver = null;
                Driver d = _repo.Drivers.FirstOrDefault(d => d.Id == userId);
                _repo.Drivers.Remove(d);
                _repo.SaveChanges();
            }
            return user;
        }

        public User GetUserByUsername(string username)
        {
            return _repo.Users.FirstOrDefault(u => u.Username == username);
        }

        public Review AddReviewToUser(long userId, Review review)
        {
            var user = _repo.Users.Include(u => u.IncomingReviews).FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.IncomingReviews.Add(review);
                _repo.SaveChanges();
            }
            return review;
        }

        public List<Review> getIncomingReviewsForUser(long userId)
        {
            return _repo.Reviews.Where(r => r.UserId == userId).ToList();
        }








        //* Trips Methods
        public Trip GetTrip(long id)
        {
            return _repo.Trips.FirstOrDefault(t => t.TripID == id);
        }
        public Trip[] GetAllTrips()
        {
            return _repo.Trips.ToArray();
        }

        public Trip[] SearchTrips(double? startLatitude, double? startLongitude, double? endLatitude, double? endLongitude, string dateStr, string timeStr, int seats)
        {
            IQueryable<Trip> query = _repo.Trips.Include(t => t.StartPoint).Include(t => t.EndPoint);

            // Filter for start location
            if (startLatitude.HasValue && startLongitude.HasValue)
            {
                // Assuming you want to find exact matches
                query = query.Where(t => t.StartPoint.Latitude == startLatitude.Value && t.StartPoint.Longitude == startLongitude.Value);
            }

            // Filter for end location
            if (endLatitude.HasValue && endLongitude.HasValue)
            {
                query = query.Where(t => t.EndPoint.Latitude == endLatitude.Value && t.EndPoint.Longitude == endLongitude.Value);
            }

            // Filter for date
            if (DateTime.TryParse(dateStr, out DateTime searchDate))
            {
                query = query.Where(t => t.DateTime.Date == searchDate.Date);
            }

            // Filter for time 
            if (!string.IsNullOrWhiteSpace(timeStr))
            {
                var parts = timeStr.Split(':');
                if (parts.Length == 2 &&
                    int.TryParse(parts[0], out int hours) &&
                    int.TryParse(parts[1], out int minutes))
                {
                    DateTime searchTime = new DateTime(1, 1, 1, hours, minutes, 0); // Using a dummy date here
                    query = query.Where(t => t.DateTime.TimeOfDay == searchTime.TimeOfDay);
                }
            }

            // Filter for the available seats
            query = query.Where(t => t.MaxRiders - t.CurrentRiders.Count >= seats);

            return query.ToArray();
        }

        public Trip[] GetAllTripsBy(long driverId)
        {
            return _repo.Trips.Where(t => t.DriverID == driverId).ToArray();
        }
        public void DeleteTrip(long id)
        {
            var tripToDelete = _repo.Trips.FirstOrDefault(t => t.TripID == id);
            if (tripToDelete != null)
            {
                _repo.Trips.Remove(tripToDelete);
                _repo.SaveChanges();
            }
        }
        public void UpdateTrip(UpdateTripDto trip)
        {
            Trip newTrip = new Trip
            {
                TripID = trip.TripID,
                DriverID = trip.DriverID,
                DateTime = trip.DateTime,
                MaxRiders = trip.MaxRiders,
                Price = trip.Price,
                StartPoint = new GPS
                {
                    Latitude = trip.StartLatitude,
                    Longitude = trip.StartLongitude
                },
                EndPoint = new GPS
                {
                    Latitude = trip.EndLatitude,
                    Longitude = trip.EndLongitude
                },
                DetourRange = trip.DetourRange
            };

            _repo.Trips.Update(newTrip);
            _repo.SaveChanges();
        }
        public long AddTrip(TripDto tripDto)
        {
            Trip newTrip = new Trip
            {
                TripID = tripDto.TripID,
                DriverID = tripDto.DriverID,
                DateTime = tripDto.DateTime,
                MaxRiders = tripDto.MaxRiders,
                Price = tripDto.Price,
                StartPoint = new GPS
                {
                    Latitude = tripDto.StartLatitude,
                    Longitude = tripDto.StartLongitude
                },
                EndPoint = new GPS
                {
                    Latitude = tripDto.EndLatitude,
                    Longitude = tripDto.EndLongitude
                },
                DetourRange = tripDto.DetourRange,
                StartLocation = tripDto.startLocation,
                EndLocation = tripDto.endLocation
            };
            _repo.Trips.Add(newTrip);
            _repo.SaveChanges();
            return newTrip.TripID;
        }
        public Driver GetDriver(long driverId)
        {
            return _repo.Drivers.FirstOrDefault(d => d.Id == driverId);
        }

        public PreferenceForTripDTO AddPreferenceToTrip(long tripId, Preference preference)
        {
            var trip = _repo.Trips.Include(t => t.Preferences).FirstOrDefault(t => t.TripID == tripId);
            if (trip != null)
            {
                trip.Preferences.Add(preference);
                _repo.SaveChanges();
            }
            PreferenceForTripDTO newPrefDto = new PreferenceForTripDTO
            {
                NoPets = preference.NoPets,
                NoLuggage = preference.NoLuggage,
                NoDrinks = preference.NoDrinks,
                NoFood = preference.NoFood,
                NoSmoking = preference.NoSmoking,
                TripId = tripId
            };
            return newPrefDto;
        }

        public List<Preference> GetPreferencesForTrip(long tripId)
        {
            return _repo.Preferences.Where(p => p.TripId == tripId).ToList();
        }

        public List<PreferenceForTripDTO> ConvertToDtoList(List<Preference> preferences)
        {
            return preferences.Select(p => new PreferenceForTripDTO
            {
                NoPets = p.NoPets,
                NoLuggage = p.NoLuggage,
                NoDrinks = p.NoDrinks,
                NoFood = p.NoFood,
                NoSmoking = p.NoSmoking,
                TripId = p.TripId
            }).ToList();
        }

        public void RemovePreferenceFromTrip(long preferenceId)
        {
            var preferenceToDelete = _repo.Preferences.FirstOrDefault(p => p.Id == preferenceId);
            Console.WriteLine(preferenceToDelete);
            if (preferenceToDelete != null)
            {
                _repo.Preferences.Remove(preferenceToDelete);
                _repo.SaveChanges();
                Console.WriteLine("Preference deleted");
            }
        }

        public PreferenceForTripDTO GetPreferenceByTripId(int tripId)
        {
            var preference = _repo.Preferences.FirstOrDefault(p => p.TripId == tripId);
            if (preference != null)
            {
                PreferenceForTripDTO newPrefDto = new PreferenceForTripDTO
                {
                    NoPets = preference.NoPets,
                    NoLuggage = preference.NoLuggage,
                    NoDrinks = preference.NoDrinks,
                    NoFood = preference.NoFood,
                    NoSmoking = preference.NoSmoking,
                    TripId = tripId
                };
                return newPrefDto;
            }
            return null;
        }

        public PreferenceForTripDTO GetPreferenceByPreferenceId(int preferenceId)
        {
            var preference = _repo.Preferences.FirstOrDefault(p => p.Id == preferenceId);
            if (preference != null)
            {
                PreferenceForTripDTO newPrefDto = new PreferenceForTripDTO
                {
                    NoPets = preference.NoPets,
                    NoLuggage = preference.NoLuggage,
                    NoDrinks = preference.NoDrinks,
                    NoFood = preference.NoFood,
                    NoSmoking = preference.NoSmoking,
                    TripId = preference.TripId
                };
                return newPrefDto;
            }
            return null;
        }

        public void SetPreferenceForTrip(int tripId, Preference preference)
        {
            var existingPreference = _repo.Preferences.FirstOrDefault(p => p.TripId == tripId);
            if (existingPreference != null)
            {
                _repo.Preferences.Remove(existingPreference);
                _repo.SaveChanges();
                _repo.Preferences.Add(preference);
                _repo.SaveChanges();
            }
            else
            {
                // If the driver doesn't have a preference, add it
                _repo.Preferences.Add(preference);
                _repo.SaveChanges();
            }
        }

    }
}