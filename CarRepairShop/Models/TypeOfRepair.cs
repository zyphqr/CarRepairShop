using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRepairShop.Models
{
    [Table("Type_Of_Repairs")]
    public class TypeOfRepair
    {
        [Key]
        public int RepairId { get; set; }
        public string RepairName { get; set; }

        public RepairCard RepairCard { get; set; }
        public ICollection<Part> Parts { get; set; }
    }
}
