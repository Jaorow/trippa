using Microsoft.AspNetCore.Mvc;
using Data;
using Models;
using Dtos;


namespace Controllers
{
    // TODO: beautify endpoints, eg error handling, better return values
    // localHost:8080/api
    [Route("api")]
    [ApiController]
    public class DonsController : Controller
    {
        private readonly IRepo _repo;
        public DonsController(IRepo repo)
        {
            _repo = repo;
        }

        //* General Endpoints

        // GET api/GetVersion
        [HttpGet("GetVersion")]
        public ActionResult<string> GetVersion()
        {
            return Ok("0.0.2 (TESTING PHASE)");
        }



        //* User Endpoints

        // POST api/AddUser
        [HttpPost("AddUser")]
        public IActionResult AddUser(UserDto user)
        {
            if (user.Username == null)
            {
                return BadRequest("Username is required");
            }
            if (user.Password == null)
            {
                return BadRequest("Password is required");
            }
            if (_repo.GetUserByUsername(user.Username) != null)
            {
                return BadRequest("Username already exists");
            }
            string username = _repo.AddUser(new User
            {
                Username = user.Username,
                Password = user.Password,
                Phone = user.Phone
            });
            return new CreatedResult($"/api/GetUser/{username}", user);
        }

        // GET api/GetUser
        [HttpGet("GetUser/{id}")]
        public ActionResult<User> GetUser(long id)
        {
            if (_repo.GetUser(id) == null)
            {
                return BadRequest($"User with id {id} does not exist");
            }
            return Ok(_repo.GetUser(id));
        }

        // GET api/GetAllUsers
        [HttpGet("GetAllUsers")]
        public ActionResult<User[]> GetAllUsers()
        {
            if (_repo.GetAllUsers().Length == 0)
            {
                return BadRequest("No users exist");
            }
            return Ok(_repo.GetAllUsers());
        }

        //GET api/GetUserByUsername
        [HttpGet("GetUserByUsername/{username}")]
        public ActionResult<UserOutDto> GetUserByUsername(string username)
        {
            if (_repo.GetUserByUsername(username) == null)
            {
                return NotFound($"User with username {username} does not exist");
            }
            User user = _repo.GetUserByUsername(username);
            return Ok(new UserOutDto { Username = user.Username, password = user.Password, Phone = user.Phone, Id = user.Id });
        }

        //GET api/GetIdByUsername
        [HttpGet("GetIdByUsername/{username}")]
        public ActionResult<long> GetIdByUsername(string username)
        {
            if (_repo.GetUserByUsername(username) == null)
            {
                return NotFound($"User with username {username} does not exist");
            }
            User user = _repo.GetUserByUsername(username);
            return Ok(user.Id);
        }

        // POST api/users/AddDriver/{id}
        [HttpPost("users/AddDriver/{userId}")]
        public IActionResult AddDriverToUser(int userId, DriverDto driverDto)
        {
            User u = _repo.GetUser(userId);
            if (u == null)
            {
                return BadRequest("UserId does not exist");
            }

            Driver d = new Driver
            {
                UserId = userId,
                User = u,
                LicenseNumber = driverDto.LicenseNumber,
                Car = new Car
                {
                    Make = driverDto.CarMake,
                    Type = driverDto.CarType,
                    Model = driverDto.CarModel,
                    Color = driverDto.CarColor,
                    LicensePlate = driverDto.PlateNumber
                }
            };

            if (driverDto == null)
            {
                return BadRequest("Driver object is null");
            }

            try
            {
                _repo.AddDriverToUser(userId, d);
                return Ok($"Driver added to user {u.Username}");
            }
            catch (Exception ex)
            {
                // Handle exception and return an error response
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/users/driver/{userId}
        [HttpDelete("users/deleteDriver/{userId}")]
        public IActionResult RemoveDriverFromUser(int userId)
        {
            User u = _repo.GetUser(userId);
            if (u == null)
            {
                return BadRequest("UserId does not exist");
            }
            try
            {
                _repo.RemoveDriverFromUser(userId);
                return Ok($"Driver removed from user {u.Username}");
            }
            catch (Exception ex)
            {
                // Handle exception and return an error response
                return BadRequest(ex.Message);
            }
        }

        // GET api/users/isDriver/{userId}
        [HttpGet("users/isDriver/{userId}")]
        public IActionResult IsDriver(int userId)
        {
            User u = _repo.GetUser(userId);
            if (u == null)
            {
                return BadRequest("UserId does not exist");
            }
            try
            {
                if (_repo.GetDriverByUserId(userId) == null)
                {
                    return Ok(false);
                }
                return Ok(true);
            }
            catch (Exception ex)
            {
                // Handle exception and return an error response
                return BadRequest(ex.Message);
            }
        }

        // POST api/users/review/{id}
        //* add a review to a user (UserId -- the user doing the review, reviewerId[insideReviewDto] -- the user being reviewed)
        [HttpPost("users/review/{userId}")]
        public IActionResult AddReviewToUser(int userId, ReviewDto reviewDto)
        {
            User u = _repo.GetUser(userId);
            if (u == null)
            {
                return BadRequest("UserId does not exist");
            }

            Review r = new Review
            {
                ReviewerId = reviewDto.ReviewerId,
                User = u,
                UserId = userId,
                ReviewText = reviewDto.ReviewText,
                Rating = reviewDto.Rating
            };

            if (reviewDto == null)
            {
                return BadRequest("Review object is null");
            }

            try
            {
                _repo.AddReviewToUser(userId, r);
                return Ok($"Review added to user {u.Username}");
            }
            catch (Exception ex)
            {
                // Handle exception and return an error response
                return BadRequest(ex.Message);
            }
        }

        // GET api/users/review/{id}
        //* get all reviews for a user
        [HttpGet("users/review/{userId}")]
        public ActionResult<List<OutReviewDto>> GetReviewsForUser(int userId)
        {
            User u = _repo.GetUser(userId);
            if (u == null)
            {
                return BadRequest("UserId does not exist");
            }

            try
            {
                List<Review> reviews = _repo.getIncomingReviewsForUser(userId);
                if (reviews == null || reviews.Count == 0)
                {
                    return Ok(new List<OutReviewDto>());
                }

                List<OutReviewDto> OutReviews = new List<OutReviewDto>();
                foreach (Review r in reviews)
                {
                    OutReviewDto outReview = new OutReviewDto
                    {
                        ReviewerName = _repo.GetUser(r.ReviewerId).Username,
                        ReviewText = r.ReviewText,
                        Rating = (int)r.Rating
                    };
                    OutReviews.Add(outReview);
                }
                return Ok(OutReviews);
            }
            catch (Exception ex)
            {
                // Handle exception and return an error response
                return BadRequest(ex.Message);
            }
        }

        // GET api/users/review-avd/{id}
        //* get average rating for a user
        [HttpGet("users/review-avg/{userId}")]
        public IActionResult GetAverageRatingForUser(int userId)
        {
            User u = _repo.GetUser(userId);
            if (u == null)
            {
                return BadRequest("UserId does not exist");
            }

            try
            {
                List<Review> reviews = _repo.getIncomingReviewsForUser(userId);
                if (reviews == null || reviews.Count == 0)
                {
                    return Ok(0);
                }

                return Ok(reviews.Average(r => r.Rating));
            }
            catch (Exception ex)
            {
                // Handle exception and return an error response
                return BadRequest(ex.Message);
            }
        }

        // Driver Endpoints 

        // GET api/GetDriverByUserId - get driver by id
        [HttpGet("GetDriverByUserId/{id}")]
        public ActionResult<DriverDto> GetDriverByUserId(long id)
        {
            Driver driver = _repo.GetDriverByUserId(id);
            if (driver == null)
            {
                return BadRequest($"Driver with user id {id} does not exist");
            }
            DriverDto driverDto = _repo.ToDriverDto(driver);
            return Ok(driverDto);
        }

        // GET api/GetDriverIdByUserId - get driver by id
        [HttpGet("GetDriverIdByUserId/{userId}")]
        public ActionResult<long> GetDriverIdByUserId(long userId)
        {
            // Call the GetDriverByUserId method
            Driver driver = _repo.GetDriverByUserId(userId);

            // Check if the driver exists
            if (driver == null)
            {
                return NotFound($"No driver found for user with ID {userId}");
            }

            // Return the DriverId
            return Ok(driver.Id);
        }

        //* Trip Endpoints

        // GET api/GetTrip - get trip by id
        [HttpGet("GetTrip/{id}")]
        public ActionResult<Trip> GetTrip(long id)
        {
            Trip trip = _repo.GetTrip(id);
            if (trip == null)
            {
                return BadRequest($"Trip with id {id} does not exist");
            }
            return Ok(trip);
        }

        // GET api/GetAllTrips - get all trips
        [HttpGet("GetAllTrips")]
        public ActionResult<IEnumerable<Trip>> GetAllTrips()
        {
            IEnumerable<Trip> trips = _repo.GetAllTrips();
            if (trips == null)
            {
                return Ok("No trips exist");
            }
            return Ok(trips);
        }

        // POST api/AddTrip - create a new trip
        [HttpPost("AddTrip")]
        public ActionResult<long> AddTrip(TripDto trip)
        {
            if (trip.DriverID == 0)
            {
                return BadRequest("DriverID is required");
            }
            if (trip.DateTime == null)
            {
                return BadRequest("DateTime is required");
            }
            if (trip.MaxRiders == 0)
            {
                return BadRequest("MaxRiders is required");
            }
            if (trip.Price == 0)
            {
                return BadRequest("Price is required");
            }
            if (trip.StartLatitude == 0)
            {
                return BadRequest("StartLatitude is required");
            }
            if (trip.StartLongitude == 0)
            {
                return BadRequest("StartLongitude is required");
            }
            if (trip.EndLatitude == 0)
            {
                return BadRequest("EndLatitude is required");
            }
            if (trip.EndLongitude == 0)
            {
                return BadRequest("EndLongitude is required");
            }
            if (trip.DetourRange == 0)
            {
                return BadRequest("DetourRange is required");
            }
            long tripId = _repo.AddTrip(trip);

            return Ok(tripId);
        }

        // PUT api/UpdateTrip - update a trip
        [HttpPut("UpdateTrip")]
        public ActionResult UpdateTrip(UpdateTripDto trip)
        {
            if (trip == null)
            {
                return BadRequest("Trip object is null");
            }
            var tripId = trip.TripID;
            Trip oldtrip = _repo.GetTrip(tripId);
            if (oldtrip == null)
            {
                return BadRequest($"Trip with id {tripId} does not exist");
            }
            var driverId = trip.DriverID;
            Driver driver = _repo.GetDriver(driverId);
            if (driver == null)
            {
                return BadRequest($"Driver with id {driverId} does not exist");
            }
            _repo.UpdateTrip(trip);
            return Ok();
        }

        // DELETE api/DeleteTrip - delete a trip
        // TODO - I think we should also check if the person trying to delete the trip is the driver
        [HttpDelete("DeleteTrip/{id}")]
        public ActionResult DeleteTrip(long id)
        {
            Trip trip = _repo.GetTrip(id);
            if (trip == null)
            {
                return BadRequest($"Trip with id {id} does not exist");
            }
            _repo.DeleteTrip(id);
            return Ok();
        }

        // GET api/GetAllTripsBy/{driverID} - get all trips by user
        [HttpGet("GetAllTripsBy/{driverID}")]
        public ActionResult<IEnumerable<Trip>> GetAllTripsBy(long driverID)
        {
            IEnumerable<Trip> trips = _repo.GetAllTripsBy(driverID);
            if (trips.Count() == 0)
            {
                return BadRequest($"No trips exist for driver with id {driverID}");
            }
            return Ok(trips);
        }

        // GET api/Trips/search - Retrieve trips based on certain criteria like start and end GPS locations, whether the trip is full, date and time range, etc.
        // Not sure how to test this
        [HttpGet("Trips/search")]
        public ActionResult<IEnumerable<Trip>> SearchTrips([FromQuery] double? startLatitude, [FromQuery] double? startLongitude, [FromQuery] double? endLatitude, [FromQuery] double? endLongitude, [FromQuery] string date, [FromQuery] string time, [FromQuery] int seats)
        {
            IEnumerable<Trip> trips = _repo.SearchTrips(startLatitude, startLongitude, endLatitude, endLongitude, date, time, seats);
            if (trips == null)
            {
                return BadRequest($"No trips exist for the given criteria");
            }
            return Ok(trips);
        }


        // GET api/GetAllTrips - get all trips as tripOutDTOs
        [HttpGet("GetAllTripsOut")]
        public ActionResult<IEnumerable<TripOutDto>> GetAllTripsOut()
        {
            IEnumerable<Trip> trips = _repo.GetAllTripsWithGPS();
            if (trips == null)
            {
                return Ok("No trips exist");
            }
            List<TripOutDto> tripOutDtos = new List<TripOutDto>();
            foreach (Trip t in trips)
            {
                Console.WriteLine(trips.Count());
                Console.WriteLine(t.StartPoint.Latitude);
                TripOutDto tripOutDto = new TripOutDto
                {

                    TripID = t.TripID,
                    DriverID = t.DriverID,
                    DateTime = t.DateTime,
                    MaxRiders = t.MaxRiders,
                    Price = t.Price,
                    StartLatitude = t.StartPoint.Latitude,
                    StartLongitude = t.StartPoint.Longitude,
                    EndLatitude = t.EndPoint.Latitude,
                    EndLongitude = t.EndPoint.Longitude,

                    DetourRange = t.DetourRange,
                    CurrentRiders = t.CurrentRiders.Count,

                    DriverName = _repo.GetUserFromDriverId(t.DriverID).Username,

                    StartLocation = t.StartLocation,
                    EndLocation = t.EndLocation,

                };
                tripOutDtos.Add(tripOutDto);
            }
            return Ok(tripOutDtos);
        }

        // Preference Endpoints

        // POST: api/preferences/addToTrip/{tripId}
        [HttpPost("addPrefToTrip")]
        public ActionResult<PreferenceForTripDTO> AddPreferenceToTrip([FromBody] PreferenceForTripDTO preferenceDto)
        {
            var preference = new Preference
            {
                NoPets = preferenceDto.NoPets,
                NoLuggage = preferenceDto.NoLuggage,
                NoFood = preferenceDto.NoFood,
                NoDrinks = preferenceDto.NoDrinks,
                NoSmoking = preferenceDto.NoSmoking,
                TripId = preferenceDto.TripId
            };
            var tripId = preferenceDto.TripId;
            var preferenceFromTripId = _repo.GetPreferenceByTripId((int)tripId);
            if (preferenceFromTripId != null)
            {
                return BadRequest($"Preference already exists for trip with id {tripId}");
            }
            var addedPreference = _repo.AddPreferenceToTrip(preferenceDto.TripId, preference);
            if (addedPreference == null)
            {
                return BadRequest("Failed to add the preference.");
            }
            return Ok(addedPreference);
        }

        // After testing this, I don't think this is actually needed - we can just use the getPrefForTrip endpoint 
        // // GET: api/preferences/getAllForTrip/{tripId}
        // [HttpGet("getAllPrefsForTrip/{tripId}")]
        // public ActionResult<IEnumerable<PreferenceForTripDTO>> GetAllPreferencesForTrip(long tripId)
        // {
        //     var trip = _repo.GetTrip(tripId);
        //     if (trip == null)
        //     {
        //         return BadRequest($"Trip with id {tripId} does not exist");
        //     }
        //     var prefs = _repo.GetPreferencesForTrip(tripId);
        //     if (prefs == null || !prefs.Any())
        //     {
        //         return BadRequest($"No preferences exist for trip with id {tripId}");
        //     }
        //     var prefsDto = _repo.ConvertToDtoList(prefs);
        //     return Ok(prefsDto);
        // }


        // DELETE: api/preferences/removeFromTrip/{preferenceId}
        [HttpDelete("removePrefFromTrip/{preferenceId}")]
        public ActionResult RemovePreferenceFromTrip(long preferenceId)
        {
            var pref = _repo.GetPreferenceByPreferenceId((int)preferenceId);
            if (pref == null)
            {
                return BadRequest($"Preference with id {preferenceId} does not exist");
            }
            var tripId = pref.TripId;
            Console.WriteLine(tripId);
            _repo.RemovePreferenceFromTrip(tripId);
            return Ok();
        }

        // GET: api/preferences/getAllForTrip/{tripId}
        [HttpGet("getPrefForTrip/{tripId}")]
        public ActionResult<PreferenceForTripDTO> GetPreferenceForTrip(long tripId)
        {
            var trip = _repo.GetTrip(tripId);
            if (trip == null)
            {
                return BadRequest($"Trip with id {tripId} does not exist");
            }
            var pref = _repo.GetPreferenceByTripId((int)tripId);
            if (pref == null)
            {
                return BadRequest($"No preferences exist for trip with id {tripId}");
            }
            return Ok(pref);
        }

        // PUT: api/preferences/setForTrip/{tripId}
        [HttpPut("UpdatePrefForTrip")]
        public ActionResult UpdatePreferenceForTrip(PreferenceForTripDTO preferenceForTripDTO)
        {
            var tripId = preferenceForTripDTO.TripId;
            var preference = new Preference
            {
                NoPets = preferenceForTripDTO.NoPets,
                NoLuggage = preferenceForTripDTO.NoLuggage,
                NoFood = preferenceForTripDTO.NoFood,
                NoDrinks = preferenceForTripDTO.NoDrinks,
                NoSmoking = preferenceForTripDTO.NoSmoking,
                TripId = preferenceForTripDTO.TripId
            };
            if (preference == null)
            {
                return BadRequest("Preference object is null");
            }
            var trip = _repo.GetTrip(tripId);
            if (trip == null)
            {
                return BadRequest($"Trip with id {tripId} does not exist");
            }
            _repo.SetPreferenceForTrip((int)tripId, preference);
            return Ok();
        }
    }
}