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

        [Required]
        public string CarRegistration { get; set; }

        [Required]
        public CarBrands CarBrand { get; set; }

        [Required]
        public string CarModel { get; set; }

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

        [MaxLength(256)]
        public string? Description { get; set; }

        [Required]
        public string Owner { get; set; }

        [Required]
        [MaxLength(11)]
        [RegularExpression("/^\\+?[1-9][0-9]{10,13}$/")]
        public string OwnerPhoneNum { get; set; }

        public ICollection<RepairCard>? RepairCards { get; set; } = new List<RepairCard>();
    }
}
