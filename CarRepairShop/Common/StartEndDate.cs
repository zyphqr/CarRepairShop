using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CarRepairShop.Common
{
    public enum StartEndDate
    {
        [Display(Name = "Start date")]
        StartDate = 1,
        [Display(Name = "End date")]
        EndDate = 2,
    }
}
