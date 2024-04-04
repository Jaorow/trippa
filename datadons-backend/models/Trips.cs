using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models;

namespace Models
{
    public class Trip
    {
        [Key]
        public long TripID { get; set; }

        [Required]
        public long DriverID { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public long MaxRiders { get; set; }

        public List<User> CurrentRiders { get; set; } = new List<User>();

        public bool IsFull => CurrentRiders?.Count >= MaxRiders;

        [Required]
        public double Price { get; set; }

        [Required]
        public GPS? StartPoint { get; set; }

        [Required]
        public GPS? EndPoint { get; set; }
        public double DetourRange { get; set; }
        public ICollection<Preference> Preferences { get; set; }

        public string? StartLocation { get; set; }
        public string? EndLocation { get; set; }
    }

    public class GPS
    {
        [Key]
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
