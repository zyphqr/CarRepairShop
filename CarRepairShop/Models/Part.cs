using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRepairShop.Models
{
    [Table("Replacement Parts")]
    public class Part
    {
        [Key]
        public int PartId { get; set; }
        public string PartName { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public int RepairCardId { get; set; } 
        public RepairCard RepairCard { get; set; }
    }
}
