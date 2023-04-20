using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarRepairShop.Areas.Identity.Data;
using CarRepairShop.Models;
using CarRepairShop.Common;
using CarRepairShop.Services;
using CarRepairShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace CarRepairShop.Controllers
{
    [Authorize]
    public class RepairCardsController : Controller
    {
        private readonly ShopDataContext _context;
        private readonly RepairCardsService _repairCardsService;

        public RepairCardsController(ShopDataContext context,
                                     RepairCardsService repairCardsService)
        {
            _context = context;
            _repairCardsService = repairCardsService;
        }

        public IActionResult Index()
        {
            var repairCards = _repairCardsService.GetAllRepairCards();

            var repairCardIndexVm = repairCards.Select(repairCard => new RepairCardIndexVM
            {
                RepairCardId = repairCard.RepairCardId,
                StartDate = repairCard.StartDate,
                EndDate = repairCard.EndDate,
                CarRegistration = repairCard.Car.CarRegistration,
                Description = repairCard.Description,
                Price = repairCard.Price,
                TypeOfRepair = repairCard.TypeOfRepair,
                MechanicFirstName = repairCard.Mechanic.FirstName,
                MechanicLastName = repairCard.Mechanic.LastName,
                Parts = _repairCardsService.SearchedParts(_context.RepairCards.Include(x => x.Parts)
                                           .FirstOrDefault(rc => rc.RepairCardId == repairCard.RepairCardId)),
            });
            return View(repairCardIndexVm);
        }

        [HttpGet]
        public IActionResult SelectRepair()
        {
            return View("Views/RepairCards/TypeOfRepairForm.cshtml", new RepairCardVM());
        }

        [HttpPost]
        public IActionResult SelectRepair(RepairCardVM vm)
        {
            var cars = _repairCardsService.GetCars();
            var selectListCars = cars
                .Select(cars => new SelectListItem(
                    cars.CarRegistration,
                    cars.CarId.ToString()));

            var parts = _repairCardsService.GetParts();
            parts = parts.Where(part => part.TypeOfRepair == vm.TypeOfRepair).ToList();
            parts = parts.Where(part => part.Quantity > 0).ToList();

            var selectListParts = parts
                .Select(parts => new SelectListItem(
                    $"{parts.PartName} - {parts.Price}lv, quantity: {parts.Quantity}",
                    parts.PartId.ToString()));

            var mechanics = _repairCardsService.GetMechanics();
            var selectListMechanics = mechanics
                .Select(mechanics => new SelectListItem(
                    mechanics.FirstName + " " + mechanics.LastName,
                    mechanics.Id.ToString()));

            return View("Views/RepairCards/Create.cshtml", new RepairCardVM
            {
                StartDate = DateTime.Now,
                CarRegistrations = selectListCars,
                SelectedCarId = cars.ToList()[0].CarId,
                TypeOfRepair = vm.TypeOfRepair,
                Parts = selectListParts,
                Mechanics = selectListMechanics,
                SelectedMechanicId = mechanics.ToList()[0].Id
            });
        }

        [HttpGet]
        public IActionResult CreateFromHome(TypeOfRepairs TypeOfRepair)
        {

            var cars = _repairCardsService.GetCars();
            var selectListCars = cars
                .Select(cars => new SelectListItem(
                    cars.CarRegistration,
                    cars.CarId.ToString()));

            var parts = _repairCardsService.GetParts();
            parts = parts.Where(part => part.TypeOfRepair == TypeOfRepair).ToList();
            parts = parts.Where(part => part.Quantity > 0).ToList();

            var selectListParts = parts
                .Select(parts => new SelectListItem(
                    $"{parts.PartName} - {parts.Price}lv, quantity: {parts.Quantity}",
                    parts.PartId.ToString()));

            var mechanics = _repairCardsService.GetMechanics();
            var selectListMechanics = mechanics
                .Select(mechanics => new SelectListItem(
                    mechanics.FirstName + " " + mechanics.LastName,
                    mechanics.Id.ToString()));

            return View("Views/RepairCards/Create.cshtml", new RepairCardVM
            {
                StartDate = DateTime.Now,
                CarRegistrations = selectListCars,
                SelectedCarId = cars.ToList()[0].CarId,
                TypeOfRepair = TypeOfRepair,
                Parts = selectListParts,
                Mechanics = selectListMechanics,
                SelectedMechanicId = mechanics.ToList()[0].Id
            });
        }

        [HttpPost]
        public IActionResult Create(RepairCardVM createRepairCard)
        {
            Car selectedCarReg = _repairCardsService
                                    .GetCars()
                                    .Single(c => c.CarId == createRepairCard.SelectedCarId);

            List<Part> SelectedParts = new();

            foreach (var partId in createRepairCard.SelectedPartIds)
            {
                SelectedParts.Add(_repairCardsService.GetParts().Single(c => c.PartId == partId));
            }

            Mechanic selectedMechanicId = _repairCardsService
                                            .GetMechanics()
                                            .Single(m => m.Id == createRepairCard.SelectedMechanicId);

            createRepairCard.TypeOfRepair = SelectedParts[0].TypeOfRepair;

            _repairCardsService.CreateRepairCard(
                                    createRepairCard.StartDate,
                                    createRepairCard.EndDate,
                                    selectedCarReg,
                                    createRepairCard.Description,
                                    createRepairCard.TypeOfRepair,
                                    SelectedParts,
                                    selectedMechanicId
                                    );
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Filtering(FilteringVM vm)
        {
            var repairCards = _repairCardsService.GetAllRepairCards();

            var cars = _repairCardsService.GetCars();
            var selectListCars = cars
                .Select(cars => new SelectListItem(
                    cars.CarRegistration,
                    cars.CarId.ToString()));

            var mechanics = _repairCardsService.GetMechanics();
            var selectListMechanics = mechanics
                .Select(mechanics => new SelectListItem(
                    mechanics.FirstName + " " + mechanics.LastName,
                    mechanics.Id.ToString()));

            var indexVM = new FilteringVM
            {
                Criteria = vm.Criteria,
                TypeOfRepair = vm.TypeOfRepair,
                StartEndDate = vm.StartEndDate,
                SelectedCarId = vm.SelectedCarId,
                CarRegistrations = selectListCars,
                SelectedMechanicId = vm.SelectedMechanicId,
                Mechanics = selectListMechanics,
                Date = vm.Date,
                RepairCardIndexVM = repairCards.Select(repairCard => new RepairCardIndexVM
                {
                    RepairCardId = repairCard.RepairCardId,
                    StartDate = repairCard.StartDate,
                    EndDate = repairCard.EndDate,
                    CarRegistration = repairCard.Car.CarRegistration,
                    Description = repairCard.Description,
                    Price = repairCard.Price,
                    TypeOfRepair = repairCard.TypeOfRepair,
                    MechanicFirstName = repairCard.Mechanic.FirstName,
                    MechanicLastName = repairCard.Mechanic.LastName,
                    Parts = _repairCardsService
                            .SearchedParts(_context.RepairCards
                            .FirstOrDefault(rc => rc.RepairCardId == repairCard.RepairCardId))
                }),

            };

            if (indexVM.Criteria != Criteria.All)
            {
                switch (indexVM.Criteria)
                {
                    case Criteria.Finished:
                        indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                    .Where(rc => rc.EndDate != null); break;

                    case Criteria.Unfinished:
                        indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                    .Where(rc => rc.EndDate == null); break;
                }
            }

            if (indexVM.TypeOfRepair != 0)
            {
                switch (indexVM.TypeOfRepair)
                {
                    case TypeOfRepairs.EngineRepair:
                        indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                    .Where(rc => rc.TypeOfRepair == TypeOfRepairs.EngineRepair); break;

                    case TypeOfRepairs.CoolingSystemRepair:
                        indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                    .Where(rc => rc.TypeOfRepair == TypeOfRepairs.CoolingSystemRepair); break;

                    case TypeOfRepairs.ExhaustSystemRepair:
                        indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                    .Where(rc => rc.TypeOfRepair == TypeOfRepairs.ExhaustSystemRepair); break;

                    case TypeOfRepairs.FuelSystemRepair:
                        indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                    .Where(rc => rc.TypeOfRepair == TypeOfRepairs.FuelSystemRepair); break;

                    case TypeOfRepairs.ElectricSystemRepair:
                        indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                    .Where(rc => rc.TypeOfRepair == TypeOfRepairs.ElectricSystemRepair); break;

                    case TypeOfRepairs.BrakingSystemRepair:
                        indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                    .Where(rc => rc.TypeOfRepair == TypeOfRepairs.BrakingSystemRepair); break;
                }
            }

            if (indexVM.StartEndDate != StartEndDate.All)
            {
                switch (indexVM.StartEndDate)
                {
                    case StartEndDate.StartDate:
                        indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                 .Where(rc => rc.StartDate >= indexVM.Date); break;
                    case StartEndDate.EndDate:
                        indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                 .Where(rc => rc.EndDate <= indexVM.Date); break;
                }
            }

            if (indexVM.SelectedCarId != 0)
            {
                Car CarToSearch = _context.Cars.FirstOrDefault(car => car.CarId == vm.SelectedCarId);

                indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                            .Where(rc => rc.CarRegistration == CarToSearch.CarRegistration);
            }

            if (indexVM.SelectedMechanicId != "0")
            {
                Mechanic MechanicToSearch = _context.Mechanics.FirstOrDefault(m => m.Id == vm.SelectedMechanicId);

                indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                            .Where(rc => rc.MechanicFirstName == MechanicToSearch.FirstName
                                                      && rc.MechanicLastName == MechanicToSearch.LastName);
            }

            return View("Views/RepairCards/Filtering.cshtml", indexVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.RepairCards == null)
            {
                return NotFound();
            }

            var repairCard = _context.RepairCards
                             .FirstOrDefault(rc => rc.RepairCardId == id);

            if (repairCard == null)
            {
                return NotFound();
            }

            var cars = _repairCardsService.GetCars();
            var selectListCars = cars
                .Select(cars => new SelectListItem(
                    cars.CarRegistration,
                    cars.CarId.ToString()));

            RepairCard toBeEdited = _context.RepairCards.Include(p => p.Parts).FirstOrDefault(p => p.RepairCardId == id);

            List<Part> parts = _context.Parts
                               .Where(p => p.TypeOfRepair == toBeEdited.TypeOfRepair)
                               .ToList();

            parts = parts.Where(part => part.Quantity > 0).ToList();

            var selectListParts = parts
                .Select(parts => new SelectListItem(
                    $"{parts.PartName} - {parts.Price}lv, quantity: {parts.Quantity}",
                    parts.PartId.ToString()));

            var mechanics = _repairCardsService.GetMechanics();
            var selectListMechanics = mechanics
                .Select(mechanics => new SelectListItem(
                    mechanics.FirstName + " " + mechanics.LastName,
                    mechanics.Id.ToString()));

            RepairCardVM editRepairCard = new()
            {
                RepairCardId = repairCard.RepairCardId,
                StartDate = repairCard.StartDate,
                EndDate = repairCard.EndDate,
                SelectedCarId = repairCard.CarId,
                CarRegistrations = selectListCars,
                Car = repairCard.Car,
                Description = repairCard.Description,
                SelectedPartIds = repairCard.Parts.Select(p => p.PartId).ToList(),
                Parts = selectListParts,
                Price = repairCard.Price,
                TypeOfRepair = repairCard.TypeOfRepair,
                SelectedMechanicId = repairCard.MechanicId,
                Mechanics = selectListMechanics,
                Mechanic = repairCard.Mechanic
            };
            return View(editRepairCard);
        }


        [HttpPost]
        public IActionResult Edit(RepairCardVM editRepairCard)
        {
            Mechanic selectedMechanicId = _repairCardsService
                                          .GetMechanics()
                                          .Single(m => m.Id == editRepairCard.SelectedMechanicId);

            List<Part> SelectedParts = new();

            foreach (var partId in editRepairCard.SelectedPartIds)
            {
                SelectedParts.Add(_repairCardsService.GetParts().Single(c => c.PartId == partId));
            }

            editRepairCard.TypeOfRepair = SelectedParts[0].TypeOfRepair;

            _repairCardsService.EditRepairCard(editRepairCard.RepairCardId,
                                                editRepairCard.StartDate,
                                                editRepairCard.EndDate,
                                                editRepairCard.Description,
                                                editRepairCard.TypeOfRepair,
                                                SelectedParts,
                                                selectedMechanicId);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || _context.RepairCards == null)
            {
                return NotFound();
            }

            var repairCard = _context.RepairCards
                             .Include(r => r.Car)
                             .Include(r => r.Mechanic)
                             .FirstOrDefault(m => m.RepairCardId == id);
            if (repairCard == null)
            {
                return NotFound();
            }

            return View(repairCard);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.RepairCards == null)
            {
                return NotFound();
            }
            var repairCard = _context.RepairCards.Find(id);
            if (repairCard != null)
            {
                _context.RepairCards.Remove(repairCard);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
