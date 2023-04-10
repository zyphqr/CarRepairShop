using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CarRepairShop.Common
{
    public enum StartEndDate
    {
        [Display(Name = "All dates")]
        All = 1,
        [Display(Name = "Start date")]
        StartDate = 2,
        [Display(Name = "End date")]
        EndDate = 3,
    }
}
