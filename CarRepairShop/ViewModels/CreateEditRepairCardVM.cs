using CarRepairShop.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CarRepairShop.ViewModels
{
    public class CreateEditRepairCardVM
    {
        [Required]
        public int RepairCardId { get; set; }

        [Required]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required]
        public DateTime EndDate { get; set; } = DateTime.Today;

        public int SelectedCarId { get; set; }
        public IEnumerable<SelectListItem> CarRegistrations { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public TypeOfRepairs TypeOfRepair { get; set; }

        public int SelectedPartId { get; set; }
        public IEnumerable<SelectListItem> Parts { get; set; }

        public string SelectedMechanicId { get; set; }
        public IEnumerable<SelectListItem> Mechanics { get; set; }
    }
}
