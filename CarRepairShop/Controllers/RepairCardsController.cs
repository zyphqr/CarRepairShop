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
    public class RepairCardsController : Controller
    {
        private readonly MEchanicDataContext _context;
        private readonly RepairCardsService _repairCardsService;

        public RepairCardsController(MEchanicDataContext context, RepairCardsService repairCardsService)
        {
            _context = context;
            _repairCardsService = repairCardsService;
        }

        // GET: RepairCards
        [Authorize]
        public IActionResult Index()
        {
            var repairCards = _repairCardsService.GetAllRepairCards();
            var parts = _repairCardsService.GetParts();

            var cars = _repairCardsService.GetCars();
            var selectListCars = cars
                .Select(cars => new SelectListItem(
                    cars.CarRegistration,
                    cars.CarId.ToString()));

            var indexVM = new IndexVMs
            {
                RepairCardsVM = repairCards.Select(repairCard => new RepairCardIndexVM
                {
                    RepairCardId = repairCard.RepairCardId,
                    StartDate = repairCard.StartDate,
                    EndDate = repairCard.EndDate,
                    CarRegistration = repairCard.Car.CarRegistration,
                    Description = repairCard.Description,
                    Price = repairCard.Price,
                    TypeOfRepair = repairCard.TypeOfRepair,
                    MechanicName = repairCard.Mechanic.FirstName + " " + repairCard.Mechanic.LastName,
                    Parts = _repairCardsService.SearchedParts(_context.RepairCards.Include(x => x.Parts).FirstOrDefault(rc => rc.RepairCardId == repairCard.RepairCardId)),
                }),
                CarRegistrations = selectListCars,
                SelectedCarId = cars.ToList()[0].CarId,
            };

            return View(indexVM);
        }


        [Authorize]
        [HttpGet]
        public IActionResult SelectRepair()
        {
            return View("Views/RepairCards/TypeOfRepairForm.cshtml", new RepairCardVM());
        }

        [Authorize]
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
                    parts.PartName + "-" + parts.Price + "lv",
                    parts.PartId.ToString()));

            var mechanics = _repairCardsService.GetMechanics();
            var selectListMechanics = mechanics
                .Select(mechanics => new SelectListItem(
                    mechanics.FirstName + " " + mechanics.LastName,
                    mechanics.Id.ToString()));

            return View("Views/RepairCards/Create.cshtml", new RepairCardVM
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                CarRegistrations = selectListCars,
                SelectedCarId = cars.ToList()[0].CarId,
                TypeOfRepair = vm.TypeOfRepair,
                Parts = selectListParts,
                Mechanics = selectListMechanics,
                SelectedMechanicId = mechanics.ToList()[0].Id
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateFromHome(RepairCardVM vm)
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
                    parts.PartName + "-" + parts.Price + "lv",
                    parts.PartId.ToString()));

            var mechanics = _repairCardsService.GetMechanics();
            var selectListMechanics = mechanics
                .Select(mechanics => new SelectListItem(
                    mechanics.FirstName + " " + mechanics.LastName,
                    mechanics.Id.ToString()));

            return View("Views/RepairCards/Create.cshtml", new RepairCardVM
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                CarRegistrations = selectListCars,
                SelectedCarId = cars.ToList()[0].CarId,
                TypeOfRepair = vm.TypeOfRepair,
                Parts = selectListParts,
                Mechanics = selectListMechanics,
                SelectedMechanicId = mechanics.ToList()[0].Id
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RepairCardVM createRepairCard)
        {
            Car selectedCarReg = _repairCardsService.GetCars().Single(c => c.CarId == createRepairCard.SelectedCarId);

            List<Part> SelectedParts = new();

            foreach (var partId in createRepairCard.SelectedPartIds)
            {
                SelectedParts.Add(_repairCardsService.GetParts().Single(c => c.PartId == partId));
            }

            Mechanic selectedMechanicId = _repairCardsService.GetMechanics().Single(m => m.Id == createRepairCard.SelectedMechanicId);

            createRepairCard.TypeOfRepair = SelectedParts[0].TypeOfRepair;//TODO: FIx  is scuffed

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

        [Authorize]
        public IActionResult References(IndexVMs vm)
        {
            var repairCards = _repairCardsService.GetAllRepairCards();

            var cars = _repairCardsService.GetCars();
            var selectListCars = cars
                .Select(cars => new SelectListItem(
                    cars.CarRegistration,
                    cars.CarId.ToString()));

            var newVm = new IndexVMs
            {
                Criteria = vm.Criteria,
                StartEndDate = vm.StartEndDate,
                SelectedCarId = vm.SelectedCarId,
                CarRegistrations = selectListCars,
                Date = vm.Date,
                RepairCardsVM = repairCards.Select(repairCard => new RepairCardIndexVM
                {
                    RepairCardId = repairCard.RepairCardId,
                    StartDate = repairCard.StartDate,
                    EndDate = repairCard.EndDate,
                    CarRegistration = repairCard.Car.CarRegistration,
                    Description = repairCard.Description,
                    Price = repairCard.Price,
                    TypeOfRepair = repairCard.TypeOfRepair,
                    MechanicName = repairCard.Mechanic.FirstName + " " + repairCard.Mechanic.LastName,
                    Parts = _repairCardsService.SearchedParts(_context.RepairCards.FirstOrDefault(rc => rc.RepairCardId == repairCard.RepairCardId))
                }),

            };

            if (newVm.Criteria != null)
            {
                switch (newVm.Criteria)
                {
                    case Criteria.All:
                        newVm.RepairCardsVM = repairCards.Select(repairCard => new RepairCardIndexVM
                        {
                            RepairCardId = repairCard.RepairCardId,
                            StartDate = repairCard.StartDate,
                            EndDate = repairCard.EndDate,
                            CarRegistration = repairCard.Car.CarRegistration,
                            Description = repairCard.Description,
                            Price = repairCard.Price,
                            TypeOfRepair = repairCard.TypeOfRepair,
                            MechanicName = repairCard.Mechanic.FirstName + " " + repairCard.Mechanic.LastName,
                            Parts = _repairCardsService.SearchedParts(_context.RepairCards.FirstOrDefault(rc => rc.RepairCardId == repairCard.RepairCardId))
                        }); break;
                    case Criteria.Finished:
                        newVm.RepairCardsVM = newVm.RepairCardsVM.Where(rc => rc.EndDate != null); break;
                    case Criteria.Unfinished:
                        newVm.RepairCardsVM = newVm.RepairCardsVM.Where(rc => rc.EndDate == null); break;

                }
            }
            if (newVm.StartEndDate != null)
            {
                if (newVm.StartEndDate == StartEndDate.StartDate)
                {
                    newVm.RepairCardsVM = newVm.RepairCardsVM.Where(rc => rc.StartDate >= newVm.Date);
                }
                else
                {
                    newVm.RepairCardsVM = newVm.RepairCardsVM.Where(rc => rc.EndDate <= newVm.Date);
                }
            }
            if (newVm.SelectedCarId != null)
            {
                Car CarToSearch = _context.Cars.FirstOrDefault(car => car.CarId == vm.SelectedCarId);
                newVm.RepairCardsVM = newVm.RepairCardsVM.Where(rc => rc.CarRegistration == CarToSearch.CarRegistration);
            }

            return View("Views/RepairCards/References.cshtml", newVm);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.RepairCards == null)
            {
                return NotFound();
            }

            var repairCard = _context.RepairCards.FirstOrDefault(rc=> rc.RepairCardId == id);
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

            List<Part> parts = _context.Parts.Where(p => p.TypeOfRepair == toBeEdited.TypeOfRepair).ToList();
            parts = parts.Where(part => part.Quantity > 0).ToList();

            var selectListParts = parts
                .Select(parts => new SelectListItem(
                    parts.PartName + "-" + parts.Price + "lv",
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
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RepairCardVM editRepairCard)
        {
            Car selectedCarReg = _repairCardsService.GetCars().Single(c => c.CarId == editRepairCard.SelectedCarId);

            List<Part> SelectedParts = new();

            Mechanic selectedMechanicId = _repairCardsService.GetMechanics().Single(m => m.Id == editRepairCard.SelectedMechanicId);

            foreach (var id in editRepairCard.SelectedPartIds)
            {
                SelectedParts.Add(_context.Parts.FirstOrDefault(p => p.PartId == id));
            }

            editRepairCard.TypeOfRepair = SelectedParts[0].TypeOfRepair;//TODO: FIx  is scuffed

            _repairCardsService.EditRepairCard(editRepairCard.RepairCardId,
                                                editRepairCard.StartDate,
                                                editRepairCard.EndDate,
                                                selectedCarReg,
                                                editRepairCard.Description,
                                                editRepairCard.TypeOfRepair,
                                                SelectedParts,
                                                selectedMechanicId);

            return RedirectToAction(nameof(Index));
        }


        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RepairCards == null)
            {
                return NotFound();
            }

            var repairCard = await _context.RepairCards
                .Include(r => r.Car)
                .Include(r => r.Mechanic)
                .FirstOrDefaultAsync(m => m.RepairCardId == id);
            if (repairCard == null)
            {
                return NotFound();
            }

            return View(repairCard);
        }


        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RepairCards == null)
            {
                return Problem("Entity set 'MEchanicDataContext.RepairCards'  is null.");
            }
            var repairCard = await _context.RepairCards.FindAsync(id);
            if (repairCard != null)
            {
                _context.RepairCards.Remove(repairCard);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
