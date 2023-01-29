using CarRepairShop.Areas.Identity.Data;
using CarRepairShop.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace CarRepairShop.Models
{
    [Table("Repair_Cards")]
    public class RepairCard
    {
        [Key]
        public int RepairCardId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [MaxLength(256)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }
        public TypeOfRepairs TypeOfRepair { get; set; }

        public ICollection<RepairCardPart> RepairCardParts { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
        public string MechanicId { get; set; }
        public Mechanic Mechanic { get; set; }

    }
}
