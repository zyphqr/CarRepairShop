using CarRepairShop.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRepairShop.ViewModels
{
    public class FilteringVM
    {
        public IEnumerable<RepairCardIndexVM> RepairCardIndexVM { get; set; }

        public Criteria Criteria { get; set; } = Criteria.All;
        public TypeOfRepairs TypeOfRepair { get; set; } = TypeOfRepairs.All;

        public StartEndDate StartEndDate { get; set; } = StartEndDate.All;

        public DateTime? Date { get; set; }
        public int SelectedCarId { get; set; } = 0;
        public IEnumerable<SelectListItem> CarRegistrations { get; set; }

        public string SelectedMechanicId { get; set; } = "0";
        public IEnumerable<SelectListItem> Mechanics { get; set; }
    }
}
