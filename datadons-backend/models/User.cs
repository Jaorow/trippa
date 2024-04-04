using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
        public string? Email { get; set; }

        // TODO: phone cant start with leading zero, fix this later... can store as string coz not a soul cares
        [Required]
        public long? Phone { get; set; }
        public string? Address { get; set; }

        // TODO: Add avatar functionally
        // public string? Avatar { get; set; }
        // public Preference[]? Preferences { get; set; }

        public List<Review>? OutgoingReviews { get; set; }
        public List<Review>? IncomingReviews { get; set; }

        public Driver? Driver { get; set; }
        public List<Trip> RiddenTrips { get; set; } = new List<Trip>();
        public List<Trip> DrivenTrips { get; set; } = new List<Trip>();

    }

    public class Review
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public long ReviewerId { get; set; }
        public User? Reviewer { get; set; }


        public string? ReviewText { get; set; }
        public int? Rating { get; set; }
    }

    public class Driver
    {

        [Key]
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        public User? User { get; set; }
        [Required]
        public required string LicenseNumber { get; set; }
        public Car? Car { get; set; }
    }

    public class Car
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long DriverId { get; set; }
        public Driver? Driver { get; set; }

        [Required]
        public required string LicensePlate { get; set; }



        // TODO: review what we need for cars... 
        // will have to change in the DTO too
        [Required]
        public required string Make { get; set; }
        [Required]
        public required string Type { get; set; }
        [Required]
        public required string Model { get; set; }
        [Required]
        public required string Color { get; set; }
    }

    public class Preference
    {
        public int Id { get; set; }
        public bool NoPets { get; set; }
        public bool NoLuggage { get; set; }
        public bool NoFood { get; set; }
        public bool NoDrinks { get; set; }
        public bool NoSmoking { get; set; }

        public long TripId { get; set; }  // ForeignKey
        public Trip Trip { get; set; }   // Navigation Property
    }



    public enum CarType
    {
        sedan,
        coupe,
        hatchback,
        convertible,
        suv,
        truck,
        van,
        wagon,
        other
    }

    public enum Make
    {
        mazda,
        toyota,
        honda,
        ford,
        chevrolet,
        nissan,
        hyundai,
        kia,
        dodge,
        jeep,
        subaru,
        bmw,
        mercedes,
        audi,
        volkswagen,
        lexus,
        other
    }

    public enum Category
    {
        // TODO: add more categories
        [Display(Name = "Smoking")]
        smoking,
        [Display(Name = "Pets")]
        pets,
        [Display(Name = "Music")]
        music,
        [Display(Name = "Food")]
        food,
        [Display(Name = "Drinks")]
        drinks,
        [Display(Name = "Chatter")]
        chatter,
        [Display(Name = "Luggage")]
        luggage,
        [Display(Name = "Air Conditioner")]
        airConditioner,
        [Display(Name = "Charger")]
        charger,
        [Display(Name = "Other")]
        other
    }
}