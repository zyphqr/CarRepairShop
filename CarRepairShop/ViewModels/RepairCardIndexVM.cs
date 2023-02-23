using CarRepairShop.Common;
using CarRepairShop.Models;

namespace CarRepairShop.ViewModels
{
    public class RepairCardIndexVM
    {
        public int RepairCardId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CarRegistration { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public TypeOfRepairs TypeOfRepair { get; set; }
        public ICollection<string> PartNames { get; set; }
        public string MechanicName { get; set; }
    }
}
