using CarRepairShop.Common;
using System.ComponentModel.DataAnnotations;

namespace CarRepairShop.ViewModels
{
    public class CreateEditCarVM
    {
        [Required]
        public int CarId { get; set; }

        [Required]
        public string CarRegistration { get; set; }

        [Required]
        public CarBrands Brand { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public YearsOfManifacture YearOfManifacture { get; set; }

        [Required]
        public string EngineNum { get; set; }

        [Required]
        public string FrameNum { get; set; }

        [Required]
        public Colors Color { get; set; }

        [Required]
        public double WorkingVolume { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string OwnerName { get; set; }

        [Required]
        public string OwnerPhoneNum { get; set; }
    }
}
