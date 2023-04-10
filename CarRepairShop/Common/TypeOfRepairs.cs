using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CarRepairShop.Common
{
    public enum TypeOfRepairs
    {
        [Display(Name = "All repairs")]
        All = 0,
        [Display(Name = "Engine repair")]
        EngineRepair = 1,
        [Display(Name = "Cooling system repair")]
        CoolingSystemRepair = 2,
        [Display(Name = "Exhaust system repair")]
        ExhaustSystemRepair = 3,
        [Display(Name = "Fuel system repair")]
        FuelSystemRepair = 4,
        [Display(Name = "Electric system repair")]
        ElectricSystemRepair = 5,
        [Display(Name = "Braking system repair")]
        BrakingSystemRepair = 6,
    }
}
