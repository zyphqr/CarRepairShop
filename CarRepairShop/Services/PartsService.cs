using CarRepairShop.Areas.Identity.Data;
using CarRepairShop.Common;
using CarRepairShop.Models;

namespace CarRepairShop.Services
{
    public class PartsService
    {
        private readonly ShopDataContext _context;
        public PartsService(ShopDataContext context)
        {
            _context = context;
        }

        public void CreatePart(int partId, 
                               string partName, 
                               int quantity, 
                               decimal price, 
                               int workingHours, 
                               TypeOfRepairs typeOfRepair)
        {

            Part newPart = new()
            {
                PartId = partId,
                PartName = partName,
                Quantity = quantity,
                Price = price,
                WorkingHours = workingHours,
                TypeOfRepair = typeOfRepair
            };

            _context.Add(newPart);
            _context.SaveChanges();
        }

        public void EditPart(int partId, 
                             string partName, 
                             int quantity, 
                             decimal price, 
                             int workingHours, 
                             TypeOfRepairs typeOfRepair)
        {
            var partToBeUpdated = _context.Parts
                                  .FirstOrDefault(c => c.PartId == partId);

            partToBeUpdated.PartId = partId;
            partToBeUpdated.PartName = partName;
            partToBeUpdated.Quantity = quantity;
            partToBeUpdated.Price = price;
            partToBeUpdated.WorkingHours = workingHours;
            partToBeUpdated.TypeOfRepair = typeOfRepair;

            _context.Update(partToBeUpdated);
            _context.SaveChanges();
        }
    }
}
