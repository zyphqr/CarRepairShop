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
        public string PartName { get; set; }
        public int Quantity { get; set; } 

        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; } 
        public int WorkingHours { get; set; } 
        public TypeOfRepairs TypeOfRepair { get; set; } 
        
        public ICollection<RepairCardPart>? RepairCardParts { get; set; }
    }
}
