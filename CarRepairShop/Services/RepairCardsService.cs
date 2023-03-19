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
            var parts = _context.Parts.Include(p => p.RepairCard).ToList();
            return parts;
        }

        public decimal CalculatePrice(ICollection<PartVM> selectedParts)
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
                                    ICollection<PartVM> selectedParts,
                                    Mechanic selectedMechanic)
        {

            RepairCard newRepairCard = new()
            {
                RepairCardId = repairCardId,
                StartDate = startDate,
                EndDate = endDate,
                Car = selectedCar,
                Description = descpription,
                TypeOfRepair = typeOfRepair,
                Price = CalculatePrice(selectedParts),
                Mechanic = selectedMechanic,
            };
            foreach (var part in selectedParts)
            {
                part.RepairCard = newRepairCard;
            }

            _context.Add(newRepairCard);
            _context.SaveChanges();
        }

       
    }
}
