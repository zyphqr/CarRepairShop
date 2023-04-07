using CarRepairShop.Areas.Identity.Data;
using CarRepairShop.Common;
using CarRepairShop.Models;
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

        public List<RepairCard> GetAllRepairCards()
        {
            var repairCards = _context.RepairCards
                            .Include(r => r.Car)
                            .Include(r => r.Mechanic)
                            .Include(r => r.Parts).ToList();
            return repairCards;
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

        public List<Part> SearchedParts(RepairCard repairCard)
        {
            return repairCard.Parts.ToList();

        }

        public void CreateRepairCard(DateTime startDate,
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
                Parts = selectedParts
            };

            _context.Add(newRepairCard);
            _context.SaveChanges();
        }

        public void EditRepairCard(int repairCardId,
                                    DateTime startDate,
                                    DateTime? endDate,
                                    Car selectedCar,
                                    string descpription,
                                    TypeOfRepairs typeOfRepair,
                                    List<Part> selectedParts,
                                    Mechanic selectedMechanic)
        {
            var findRepairCard = _context.RepairCards.FirstOrDefault(rc => rc.RepairCardId == repairCardId);

            findRepairCard.Parts.Clear();

            findRepairCard.RepairCardId = repairCardId;
            findRepairCard.StartDate = startDate;
            findRepairCard.EndDate = endDate;
            findRepairCard.Car = selectedCar;
            findRepairCard.Description = descpription;
            findRepairCard.TypeOfRepair = typeOfRepair;
            findRepairCard.Price = CalculatePrice(selectedParts);
            findRepairCard.Mechanic = selectedMechanic;
            findRepairCard.Parts = selectedParts;
            

            _context.Update(findRepairCard);
            _context.SaveChanges();

        }

    }
}
