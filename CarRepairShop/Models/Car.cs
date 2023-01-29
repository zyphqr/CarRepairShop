using CarRepairShop.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRepairShop.Models
{
    [Table("Cars")]
    public class Car
    {
        [Key]
        public int CarId { get; set; }

        [MaxLength(16)]
        public string CarRegistration { get; set; }
        public CarBrands CarBrand { get; set; }

        [MaxLength(20)]
        public string CarModel { get; set; }
        public YearsOfManifacture YearOfManifacture { get; set; }

        //TODO: [RegularExpression("/^\\+?[1-9][0-9]{7,14}$/")]
        //public int YearOfManifacture { get; set; } 

        [MaxLength(10)]
        public string EngineNum { get; set; }

        [MaxLength(17)]
        public string FrameNum { get; set; }
        public Colors Color { get; set; }
        public double WorkingVolume { get; set; }

        [MaxLength(256)]
        public string? Description { get; set; }
        public string Owner { get; set; }

        [MaxLength(10)]
        [RegularExpression("/^\\+?[1-9][0-9]{7,14}$/")]
        public string OwnerPhoneNum { get; set; }

        public ICollection<RepairCard> RepairCards { get; set; }
    }
}
