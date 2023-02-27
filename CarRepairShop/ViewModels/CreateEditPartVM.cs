using CarRepairShop.Common;
using System.ComponentModel.DataAnnotations;

namespace CarRepairShop.ViewModels
{
    public class CreateEditPartVM
    {
        public int PartId { get; set; }

        public string PartName { get; set; }

        public bool IsSelected { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public int WorkingHours { get; set; }

        public TypeOfRepairs TypeOfRepair { get; set; }
    }
}
