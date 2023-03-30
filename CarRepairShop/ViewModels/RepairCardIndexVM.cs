using CarRepairShop.Common;
using CarRepairShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRepairShop.ViewModels
{
    public class RepairCardIndexVM
    {
        public int RepairCardId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CarRegistration { get; set; }
        public IEnumerable<SelectListItem> CarRegistrations { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public TypeOfRepairs TypeOfRepair { get; set; }
        public List<Part> Parts { get; set; }
        public string MechanicName { get; set; }
        
    }
}
