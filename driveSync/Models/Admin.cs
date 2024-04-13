using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using driveSync.Models;

namespace driveSync.Models
{
    public class Admin
    {
        //what describes an admin
        [Key]
        public int AdminId { get; set; }
        public string firstName { get; set; }

        public string username { get; set; }
        public string password { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }

        



    }

    public class AdminDTO
    {
        public int AdminId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }

    }
}