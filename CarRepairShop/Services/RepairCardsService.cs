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

        private readonly ShopDataContext _context;
        public RepairCardsService(ShopDataContext context)
        {
            _context = context;
        }
        public List<Mechanic> GetMechanics()
        {
            return _context.Mechanics.ToList();
        }

        public List<Part> GetParts()
        {
            var parts = _context.Parts.Include(p => p.RepairCards).ToList();
            return parts;
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
            var Parts = repairCard.Parts.ToList();
            return Parts;
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

            foreach (Part part in selectedParts)
            {
                part.Quantity--;
            }

            _context.Add(newRepairCard);
            _context.SaveChanges();
        }

        public void EditRepairCard(int repairCardId,
                                    DateTime startDate,
                                    DateTime? endDate,
                                    Car car,
                                    string descpription,
                                    TypeOfRepairs typeOfRepair,
                                    List<Part> selectedParts,
                                    Mechanic selectedMechanic)
        {
            var repairCardToBeUpdated = _context.RepairCards
                                        .Include(p => p.Parts)
                                        .FirstOrDefault(rc => rc.RepairCardId == repairCardId);

            foreach (Part part in SearchedParts(repairCardToBeUpdated))
            {
                part.Quantity++;
            }

            foreach (Part part in selectedParts)
            {
                part.Quantity--;
            }

            repairCardToBeUpdated.Parts.Clear();

            repairCardToBeUpdated.RepairCardId = repairCardId;
            repairCardToBeUpdated.StartDate = startDate;
            repairCardToBeUpdated.EndDate = endDate;
            repairCardToBeUpdated.Car = car;
            repairCardToBeUpdated.Description = descpription;
            repairCardToBeUpdated.TypeOfRepair = typeOfRepair;
            repairCardToBeUpdated.Price = CalculatePrice(selectedParts);
            repairCardToBeUpdated.Mechanic = selectedMechanic;
            repairCardToBeUpdated.Parts = selectedParts;

            _context.Update(repairCardToBeUpdated);
            _context.SaveChanges();

        }

    }
}
