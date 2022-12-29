﻿using CarRepairShop.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRepairShop.Models
{
    [Table("Cars")]
    public class Car
    {
        [Key]
        public int CarId { get; set; }
        public string CarRegistration { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public YearsOfManifacture YearOfManifacture { get; set; }
        public string EngineNum { get; set; }
        public string FrameNum { get; set; }
        public Colors Color { get; set; }
        public double WorkingVolume { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public string OwnerPhoneNum { get; set; }

        public ICollection<RepairCard> RepairCards { get; set; }
    }
}
