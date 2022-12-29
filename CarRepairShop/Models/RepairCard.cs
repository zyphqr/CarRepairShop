using CarRepairShop.Areas.Identity.Data;
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
        public DateTime EndDate { get; set; }
        public int CarRegistration { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

        public string Parts { get; set; }
        public Car Car { get; set; }
        public int RepairId { get; set; }
        public TypeOfRepair TypeOfRepair { get; set; }
        public string MechanicId { get; set; }
        public Mechanic Mechanic { get; set; }

    }
}
