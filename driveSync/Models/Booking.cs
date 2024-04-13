using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using driveSync.Models;

namespace driveSync.Models
{
    public class Booking
    {

        //what describes a booking
        [Key]
        public int BookingId { get; set; }


        //a booking has a passenger ID
        //a passenger has many bookings
        [ForeignKey("Passenger")]
        public int PassengerId { get; set; }
        public virtual Passenger Passenger { get; set; }

        //a booking has a ride ID
        //a ride has many bookings

        [ForeignKey("Ride")]
        public int RideId { get; set; }
        public virtual Ride Ride { get; set; }

        // a booking can have multiple inventories
        public virtual ICollection<Inventory> Inventories { get; set; }

    }

    public class BookingDTO
    {
        public int BookingId { get; set; }
        public int PassengerId { get; set; }
        public int RideId { get; set; }
        public int DriverId { get; set; }
        public string passengerFirstName { get; set; }
        public string passengerLastName { get; set; }
        public string passengerEmail { get; set; }
        public string driverFirstName { get; set; }
        public string driverLastName { get; set; }
        public string driverEmail { get; set; }
        public string startLocation { get; set; }
        public string endLocation { get; set; }
        public string dayOftheweek { get; set; }

        public string ItemName { get; set; }
        public string LuggageQuantity { get; set; }
        public string LuggageWeight { get; set; }
        public string LuggageSize { get; set; }

        public string BagQuantity { get; set; }
        public string BagWeight { get; set; }
        public string BagSize { get; set; }

        public DateTime Time { get; set; }

        public string price { get; set; }
        public string CarType { get; set; }


    }
}
