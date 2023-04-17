using CarRepairShop.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRepairShop.ViewModels
{
    public class FilteringVM
    {
        public IEnumerable<RepairCardIndexVM> RepairCardIndexVM { get; set; }

        public Criteria Criteria { get; set; }
        public TypeOfRepairs TypeOfRepair { get; set; } = 0;

        public StartEndDate StartEndDate { get; set; } 

        public DateTime? Date { get; set; }
        public int SelectedCarId { get; set; } = 0;
        public IEnumerable<SelectListItem> CarRegistrations { get; set; }

        public string SelectedMechanicId { get; set; } = "0";
        public IEnumerable<SelectListItem> Mechanics { get; set; }
    }
}
