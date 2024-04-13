using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace driveSync.Models
{
    public class AvailableInventoriesDTO
    {
        public int RideId { get; set; }
        public string ItemName { get; set; }
        public string Weight { get; set; }
        public string Size { get; set; }
        public string can_carry { get; set; }

    }
}