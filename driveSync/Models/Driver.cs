using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using driveSync.Models;

namespace driveSync.Models
{
    public class Driver
    {
        //what describes a driver
        [Key]
        public int DriverId { get; set; }

        public string username { get; set; }

        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public int Age { get; set; }
        public string CarType { get; set; }

        //a driver has many rides
        public ICollection<Ride> Rides { get; set; }


    }

    public class DriverDTO
    {
        public string username { get; set; }
        public int DriverId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public int Age { get; set; }
        public string CarType { get; set; }


    }
}
