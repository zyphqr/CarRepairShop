using System.ComponentModel.DataAnnotations;

namespace CarRepairShop.Common
{
    public enum Criteria
    {
        [Display(Name = "All")]
        All = 1,
        [Display(Name = "Finished")]
        Finished = 2,
        [Display(Name = "Unfinished repair")]
        Unfinished = 3,
    }
}
