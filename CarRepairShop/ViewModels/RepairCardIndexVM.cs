using CarRepairShop.Common;
using CarRepairShop.Models;

namespace CarRepairShop.ViewModels
{
    public class RepairCardIndexVM
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int SelectedCarId { get; set; }
        public string CarRegistration { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public TypeOfRepairs TypeOfRepair { get; set; }
        public int SelectedPartId { get; set; }
        public IEnumerable<Part> Parts { get; set; }
        public string SelectedMechanicId { get; set; }
        public string MechanicName { get; set; }
    }
}
