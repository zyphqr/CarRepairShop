using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRepairShop.Models
{
    [Table("Type Of Repairs")]
    public class TypeOfRepair
    {
        [Key]
        public int RepairId { get; set; }
        public string RepairName { get; set; }

        public int RepairCardId { get; set; }
        public RepairCard RepairCard { get; set; }
    }
}
