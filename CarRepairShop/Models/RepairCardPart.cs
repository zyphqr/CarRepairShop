namespace CarRepairShop.Models
{
    public class RepairCardPart
    {
        public int RepairCardId { get; set; }
        public RepairCard RepairCard { get; set; }

        public int PartId { get; set; }
        public Part Part { get; set; }
    }
}
