using CarRepairShop.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRepairShop.Models
{
    [Table("Parts")]
    public class Part
    {
        [Key]
        public int PartId { get; set; }

        [Required]
        public string PartName { get; set; }

        [Required]
        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }

        [Required]
        public int WorkingHours { get; set; }

        [Required]
        public TypeOfRepairs TypeOfRepair { get; set; }

        public ICollection<RepairCard>? RepairCards { get; set; } = new List<RepairCard>();
    }
}
