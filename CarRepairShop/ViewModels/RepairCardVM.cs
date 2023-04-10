using CarRepairShop.Areas.Identity.Data;
using CarRepairShop.Common;
using CarRepairShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CarRepairShop.ViewModels
{
    public class RepairCardVM
    {
        [Required]
        public int RepairCardId { get; set; }

        [Required]
        public DateTime StartDate { get; set; } 

        public DateTime? EndDate { get; set; } 

        public int SelectedCarId { get; set; }
        public IEnumerable<SelectListItem> CarRegistrations { get; set; }
        public Car Car { get; set; }
        public string? Description { get; set; }

        public List<int> SelectedPartIds { get; set; }
        public IEnumerable<SelectListItem> Parts { get; set; }
        public List<Part> CurrentParts { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public TypeOfRepairs TypeOfRepair { get; set; }

        public string SelectedMechanicId { get; set; }
        public IEnumerable<SelectListItem> Mechanics { get; set; } 
        public Mechanic Mechanic { get; set; }
    }
}
