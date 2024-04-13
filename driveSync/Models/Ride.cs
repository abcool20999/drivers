using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using driveSync.Models;

namespace driveSync.Models
{
    public class Ride
    {
        internal object StartLocation;
        internal object EndLocation;
        internal object Price;
        internal object DayOfTheWeek;
        internal string ItemName;
        internal string Weight;
        internal string Size;
        internal string Quantity;

        //what describes a ride
        [Key]
        public int RideId { get; set; }
        public string startLocation { get; set; }
        public string endLocation { get; set; }
        public string price { get; set; }
        public DateTime Time { get; set; }
        public string dayOftheweek { get; set; }

        public string BagQuantity { get; set; }
        public string BagWeight { get; set; }
        public string BagSize { get; set; }

        public string LuggageQuantity { get; set; }
        public string LuggageWeight { get; set; }
        public string LuggageSize { get; set; }

        //a ride has a driver ID
        //a driver has many rides
        [ForeignKey("Driver")]
        public int DriverId { get; set; }
        public virtual Driver Driver { get; set; }
        public object Bookings { get; internal set; }
    }

    public class RideDTO
    {
        public int RideId { get; set; }

        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public string Price { get; set; }

        public int DriverId { get; set; }
        public DateTime Time { get; set; }

        public string DayOftheweek { get; set; }

        //public string ItemName { get; set; }
        public string BagQuantity { get; set; }
        public string BagWeight { get; set; }
        public string BagSize { get; set; }

        public string LuggageQuantity { get; set; }
        public string LuggageWeight { get; set; }
        public string LuggageSize { get; set; }


    }
}