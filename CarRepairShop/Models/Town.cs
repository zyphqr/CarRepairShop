using System.ComponentModel.DataAnnotations;

namespace CarRepairShop.Models
{
    public class Town
    {
        [Key]
        public int TownId { get; set; }
        public string TownCode { get; set; }
    }
}
