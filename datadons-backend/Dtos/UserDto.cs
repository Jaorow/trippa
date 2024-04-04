namespace Dtos{
    public class UserDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required long Phone { get; set; }
    }

    public class UserOutDto
    {
        public required string Username { get; set; }
        public required string password { get; set; }
        public long? Phone { get; set; }
        public long Id { get; set; }
    }
    public class DriverDto{
        public long UserId { get; set; } 
        public required string LicenseNumber { get; set; }
        public required string CarModel { get; set; }
        public required string CarColor { get; set; }
        public required string CarMake { get; set; }
        public required string CarType { get; set; }
        public required string PlateNumber { get; set; }

    }

    public class ReviewDto{
        public long ReviewerId { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
    }
    public class OutReviewDto{
        public required string ReviewerName { get; set; }
        public required string ReviewText { get; set; }
        public required int Rating { get; set; }
    }
}