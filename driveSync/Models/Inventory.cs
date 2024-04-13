using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace driveSync.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }
        public string ItemName { get; set; }
        public string Quantity { get; set; }
        public string Weight { get; set; }
        public string Size { get; set; }

        // Navigation property
        public virtual Booking Booking { get; set; }

    }

    public class InventoryDTO
    {
        public int InventoryId { get; set; }
        public string ItemName { get; set; }
        public string Quantity { get; set; }
        public string Weight { get; set; }
        public string Size { get; set; }

    }
}
