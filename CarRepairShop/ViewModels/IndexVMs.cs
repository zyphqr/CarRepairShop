using CarRepairShop.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRepairShop.ViewModels
{
    public class IndexVMs
    {
        public IEnumerable<RepairCardIndexVM> RepairCardsVM { get; set; }

        public TypeOfRepairs TypeOfRepair { get; set; }

        public Criteria? Criteria { get; set; }

        public StartEndDate? StartEndDate { get; set; }

        public DateTime? Date { get; set; }

        public int? SelectedCarId { get; set; }
        public IEnumerable<SelectListItem> CarRegistrations { get; set; }
    }
}
