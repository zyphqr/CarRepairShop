using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRepairShop.Models
{
    [Table("Cars")]
    public class Car
    {
        [Key]
        public int CarId { get; set; }
        public string CarRegistration { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public DateTime YearOfManifacture { get; set; }
        public int EngineNum { get; set; }
        public int FrameNum { get; set; }
        public string Color { get; set; }
        public string WorkingVolume { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public string OwnerPhoneNum { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public ICollection<RepairCard> RepairCards { get; set; }
    }
}
