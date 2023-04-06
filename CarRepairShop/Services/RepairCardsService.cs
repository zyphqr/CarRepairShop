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
                                    Car selectedCar,
                                    string descpription,
                                    TypeOfRepairs typeOfRepair,
                                    List<Part> selectedParts,
                                    Mechanic selectedMechanic)
        {
            //_context.RepairCards.FirstOrDefault(rc => rc.RepairCardId == repairCardId).Parts.Clear();

            //RepairCard repairCard = _context.RepairCards.Include(rc => rc.Parts)
            //                                            .AsNoTracking()
            //                                            .Single(rc => rc.RepairCardId == repairCardId);
            //_context.Entry(repairCard).State = EntityState.Detached;

            var NewPartIds = selectedParts.Select(p => p.PartId).ToList();

            //foreach (var Part in repairCard.Parts.ToList())
            //{
            //    if(!NewPartIds.Contains(Part.PartId))
            //    {
            //        repairCard.Parts.Remove(Part);
            //    }
            //}
            //foreach(var newid in NewPartIds)
            //{
            //    if (!repairCard.Parts.Any(p => p.PartId == newid))
            //    {
            //        var newPart = new Part
            //        {
            //            PartId = newid,
            //        };
            //        _context.Parts.Attach(newPart);
            //        repairCard.Parts.Add(newPart);
            //    }
            //}

            

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
            };

            var newParts = _context.Parts
                                 .Where(p => NewPartIds.Contains(p.PartId))
                                 .ToList();
            repairCardToEdit.Parts.Clear();

            foreach(Part part in repairCardToEdit.Parts.ToList())
            {
                repairCardToEdit.Parts.Remove(part);
            }
            _context.SaveChanges();

            foreach (var Part in newParts)
            {
                repairCardToEdit.Parts.Add(Part);
            }

            foreach (Part part in SearchedParts(repairCardToEdit))
            {
                part.Quantity++;
            }

            foreach (Part part in selectedParts)
            {
                part.Quantity--;
            }
            //_context.Entry(repairCard).State = EntityState.Modified;
            _context.Update(repairCardToEdit);
            _context.SaveChanges();

        }

    }
}
