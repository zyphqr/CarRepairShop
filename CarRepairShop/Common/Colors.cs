using System.ComponentModel.DataAnnotations;

namespace CarRepairShop.Common
{
    public enum Colors
    {
        [Display(Name = "Red")]
        Red = 1,
        [Display(Name = "Blue")]
        Blue = 2,
        [Display(Name = "Gray")]
        Gray = 3,
        [Display(Name = "Black")]
        Black = 4,
        [Display(Name = "White")]
        White = 5,
    }
}
