using CarRepairShop.Common;
using System.ComponentModel.DataAnnotations;

namespace CarRepairShop.ViewModels
{
    public class CreateEditPartVM
    {
        [Required]
        public int PartId { get; set; }

        [Required]
        public string PartName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int WorkingHours { get; set; }

        [Required]
        public TypeOfRepairs TypeOfRepair { get; set; }
    }
}
