using System.ComponentModel.DataAnnotations;

namespace CarRepairShop.Common
{
    public enum Criteria
    {
        [Display(Name = "Start date")]
        Startdate = 1,
        [Display(Name = "End date")]
        Enddate = 2,
        [Display(Name = "Unfinished repair")]
        Unfinished = 3,
    }
}
