using CarRepairShop.Common;
using CarRepairShop.Models;
using System.ComponentModel.DataAnnotations;

namespace CarRepairShop.ViewModels
{
    public class PartVM
    {
        public int PartId { get; set; }

        public string PartName { get; set; }

        public decimal Price { get; set; }

        public int WorkingHours { get; set; }

        public TypeOfRepairs TypeOfRepair { get; set; }
    }
}
