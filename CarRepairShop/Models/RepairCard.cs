using CarRepairShop.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace CarRepairShop.Models
{
    [Table("Repair Cards")]
    public class RepairCard
    {
        [Key]
        public int RepairCardId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int CarId { get; set; }
        public Car Car { get; set; }
        public TypeOfRepair TypeOfRepair { get; set; }
        public int MechanicId { get; set; }
        public Mechanic Mechanic { get; set; }
        public ICollection<Part> Parts { get; set; }

    }
}
