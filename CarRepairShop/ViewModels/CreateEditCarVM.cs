using CarRepairShop.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CarRepairShop.ViewModels
{
    public class CreateEditCarVM
    {
        public int CarId { get; set; }

        //public int SelectedTownId{ get; set; }
        //public IEnumerable<SelectListItem> Towns { get; set; }

        //public string CarRegistrationNumbers{ get; set; }

        //public string CarRegistrationCode { get; set; }

        public string CarRegistration { get; set; }

        public CarBrands Brand { get; set; }

        public string Model { get; set; }

        public YearsOfManifacture YearOfManifacture { get; set; }

        public string EngineNum { get; set; }

        public string FrameNum { get; set; }

        public Colors Color { get; set; }

        public double WorkingVolume { get; set; }

        public string Description { get; set; }

        public string OwnerName { get; set; }

        public string OwnerPhoneNum { get; set; }
    }
}
