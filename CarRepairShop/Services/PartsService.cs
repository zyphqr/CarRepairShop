using CarRepairShop.Areas.Identity.Data;
using CarRepairShop.Common;
using CarRepairShop.Models;

namespace CarRepairShop.Services
{
    public class PartsService
    {
        private readonly MEchanicDataContext _context;
        public PartsService(MEchanicDataContext context)
        {
            _context = context;
        }

        public void CreatePart(int partId, string partName, decimal price, int workingHours, TypeOfRepairs typeOfRepair)
        {
            foreach (Part part in _context.Parts)
            {
                if (part.PartId == partId || part.PartName == partName)
                {
                    throw new Exception("This part already exists.");
                }
            }

            Part newPart = new()
            {
                PartId = partId,
                PartName = partName,
                Price = price,
                WorkingHours = workingHours,
                TypeOfRepair = typeOfRepair
            };

            _context.Add(newPart);
            _context.SaveChanges();
        }

        public void EditPart(int partId, string partName, decimal price, int workingHours, TypeOfRepairs typeOfRepair)
        {

            Part partToBeUpdated = new()
            {
                PartId = partId,
                PartName = partName,
                Price = price,
                WorkingHours = workingHours,
                TypeOfRepair = typeOfRepair
            };

            _context.Update(partToBeUpdated);
            _context.SaveChanges();
        }
    }
}
