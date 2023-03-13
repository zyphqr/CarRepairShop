using System.ComponentModel.DataAnnotations;

namespace CarRepairShop.Common
{
    public enum Criteria
    {
        [Display(Name = "All repair cards")]
        All = 1,
        [Display(Name = "Finished")]
        Finished = 2,
        [Display(Name = "Unfinished repair")]
        Unfinished = 3,
    }
}
