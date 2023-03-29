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
            var parts = _context.Parts.Include(p => p.RepairCards).ToList();
            return parts;
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

        public decimal CalculatePrice(List<Part> selectedParts)
        {
            decimal price = 0;
            foreach (var part in selectedParts)
            {
                price += part.WorkingHours * workHourPrice + part.Price;
            }
            return price;
        }

        public List<Part> GetAllParts(RepairCard repairCard)
        {
            var SearchedParts = repairCard.Parts.Select(p => p.PartId).ToList();
            
            var Parts =_context.Parts.Where(p => SearchedParts.Contains(p.PartId)).ToList();
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
            };

            foreach (Part part in selectedParts)
            {
                part.Quantity--;
            }

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

        public void EditRepairCard(int repairCardId,
                                    DateTime startDate,
                                    DateTime? endDate,
                                    Car selectedCar,
                                    string descpription,
                                    TypeOfRepairs typeOfRepair,
                                    List<Part> selectedParts,
                                    Mechanic selectedMechanic)
        {
            RepairCard repairCardToEdit = new()
            {
                RepairCardId = repairCardId,
                StartDate = startDate,
                EndDate = endDate,
                Car = selectedCar,
                Description = descpription,
                TypeOfRepair = typeOfRepair,
                Price = CalculatePrice(selectedParts),
                Mechanic = selectedMechanic,
                MechanicId = selectedMechanic.Id,
            };

            

            foreach (Part part in GetAllParts(repairCardToEdit))
            {
                part.Quantity++;
            }

            //repairCardToEdit.Parts.Clear();

            ////foreach (var part in selectedParts)
            ////{
            ////    var partToEnter = _context.Parts.FirstOrDefault(p => p.PartId == part.PartId);
            ////    repairCardToEdit.Parts.Add(new RepairCardPart()
            ////    {
            ////        PartId = partToEnter.PartId,
            ////        RepairCardId = repairCardToEdit.RepairCardId
            ////    });
            ////}

            //foreach (Part part in selectedParts)
            //{
            //    part.Quantity--;
            //}

            _context.Update(repairCardToEdit);
            _context.SaveChanges();

        }

    }
}
