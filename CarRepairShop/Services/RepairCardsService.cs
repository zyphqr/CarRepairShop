using CarRepairShop.Areas.Identity.Data;
using CarRepairShop.Common;
using CarRepairShop.Models;
using CarRepairShop.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CarRepairShop.Services
{
    public class RepairCardsService
    {
        private const int workHourPrice = 2;

        private readonly MEchanicDataContext _context;
        public RepairCardsService(MEchanicDataContext context)
        {
            _context = context;
        }
        public List<Mechanic> GetMechanics()
        {
            return _context.Mechanics.ToList();
        }

        public List<Part> GetParts()
        {
            return _context.Parts.ToList();
        }

        public List<Car> GetCars()
        {
            return _context.Cars.ToList();
        }

        public List<Town> GetTowns()
        {
            return _context.Towns.ToList();
        }

        public List<RepairCard> GetAllRepairCards()
        {
            var repairCards = _context.RepairCards
                            .Include(r => r.Car)
                            .Include(r => r.Mechanic)
                            .Include(r => r.Parts).ToList();
            return repairCards;
        }

        public List<Part> GetAllParts()
        {
            var parts = _context.Parts.Include(p => p.RepairCards).ToList();
            return parts;
        }

        public decimal CalculatePrice(List<Part> selectedParts)
        {
            decimal price = 0;
            foreach (var part in selectedParts)
            {
                price += part.WorkingHours * workHourPrice + part.Price;
            }

            return price;
        }

        public void CreateRepairCard(int repairCardId,
                                    DateTime startDate,
                                    DateTime? endDate,
                                    Car selectedCar,
                                    string descpription,
                                    TypeOfRepairs typeOfRepair,
                                    List<Part> selectedParts,
                                    Mechanic selectedMechanic)
        {

            RepairCard newRepairCard = new()
            {
                StartDate = startDate,
                EndDate = endDate,
                Car = selectedCar,
                Description = descpription,
                TypeOfRepair = typeOfRepair,
                Price = CalculatePrice(selectedParts),
                Mechanic = selectedMechanic,
            };
            
           

            _context.Add(newRepairCard);
            _context.SaveChanges();

            foreach (var part in selectedParts)
            {
                var parttoenter = _context.Parts.FirstOrDefault(p => p.PartId == part.PartId);
                newRepairCard.Parts.Add(new RepairCardPart()
                {
                    PartId = parttoenter.PartId,
                    RepairCardId = newRepairCard.RepairCardId
                });
            }
            _context.SaveChanges();
        }

       
    }
}
