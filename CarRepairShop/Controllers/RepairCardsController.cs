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

            List<string> selectedParts = new();
            foreach (int id in _context.RepairCardParts.Select(p => p.PartId).ToList())
            {
                selectedParts.Add(_context.Parts.FirstOrDefault(p => p.PartId == id).PartName);
            }

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
                    PartNames = selectedParts,
                    MechanicName = repairCard.Mechanic.FirstName + " " + repairCard.Mechanic.LastName,
                    Parts = _repairCardsService.GetAllParts(_context.RepairCards.FirstOrDefault(rc => rc.RepairCardId == repairCard.RepairCardId)),
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

            var selectListParts = parts
                .Select(parts => new SelectListItem(
                    parts.PartName,
                    parts.PartId.ToString()));

            var mechanics = _repairCardsService.GetMechanics();
            var selectListMechanics = mechanics
                .Select(mechanics => new SelectListItem(
                    mechanics.FirstName + " " + mechanics.LastName,
                    mechanics.Id.ToString()));


            //List<PartVM> partVMs = new(); //TODO: Might cause issues due typing missmatching

            //foreach(Part part in parts)
            //{
            //    PartVM newvm = new()
            //    {
            //        PartName = part.PartName,
            //        Quantity = part.Quantity,
            //        Price = part.Price,
            //        WorkingHours = part.WorkingHours,
            //        PartId = part.PartId,
            //        TypeOfRepair = part.TypeOfRepair,
            //    };

            //    partVMs.Add(newvm);
            //}

            return View("Views/RepairCards/Create.cshtml", new RepairCardVM
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                CarRegistrations = selectListCars,
                SelectedCarId = cars.ToList()[0].CarId,
                TypeOfRepair = vm.TypeOfRepair,
                //partVMs = partVMs,
                Parts = selectListParts,
                Mechanics = selectListMechanics,
                SelectedMechanicId = mechanics.ToList()[0].Id
            });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            //var cars = _shopService.GetCars();
            //var selectListCars = cars
            //    .Select(cars => new SelectListItem(
            //        cars.CarRegistration,
            //        cars.CarId.ToString()));

            //var parts = _shopService.GetParts();
            //var selectListParts = parts
            //    .Select(parts => new SelectListItem(
            //        parts.PartName,
            //        parts.PartId.ToString()));

            //var mechanics = _shopService.GetMechanics();
            //var selectListMechanics = mechanics
            //    .Select(mechanics => new SelectListItem(
            //        mechanics.FirstName + " " + mechanics.LastName,
            //        mechanics.Id.ToString()));

            return View("Views/RepairCards/Create.cshtml", new RepairCardVM
            {
                //StartDate = DateTime.Now,
                //EndDate = DateTime.Now,
                //CarRegistrations = selectListCars,
                //SelectedCarId = cars.ToList()[0].CarId,
                //Parts = selectListParts,
                //SelectedPartId = parts.ToList()[0].PartId,
                //Mechanics = selectListMechanics,
                //SelectedMechanicId = mechanics.ToList()[0].Id
            });
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RepairCardVM createRepairCard)
        {

            Car selectedCarReg = _repairCardsService.GetCars().Single(c => c.CarId == createRepairCard.SelectedCarId);
            //List<PartVM> selectedParts = createRepairCard.partVMs.Where(part=>part.IsSelected == true).ToList();
            List<Part> selectedParts = new();
            foreach (var partId in createRepairCard.SelectedPartIds)
            {
                selectedParts.Add(_repairCardsService.GetParts().Single(c => c.PartId == partId));
            }
            Mechanic selectedMechanicId = _repairCardsService.GetMechanics().Single(m => m.Id == createRepairCard.SelectedMechanicId);

            createRepairCard.TypeOfRepair = selectedParts[0].TypeOfRepair;//TODO: FIx  is scuffed

            _repairCardsService.CreateRepairCard(
                createRepairCard.StartDate,
                createRepairCard.EndDate,
                selectedCarReg,
                createRepairCard.Description,
                createRepairCard.TypeOfRepair,
                selectedParts,
                selectedMechanicId
                );
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public IActionResult References(IndexVMs vm)
        {
            var repairCards = _repairCardsService.GetAllRepairCards();

            List<string> selectedParts = new();
            foreach (int id in _context.RepairCardParts.Select(p => p.PartId).ToList())
            {
                selectedParts.Add(_context.Parts.FirstOrDefault(p => p.PartId == id).PartName);
            }

            var newVm = new IndexVMs
            {
                Criteria = vm.Criteria,
                StartEndDate = vm.StartEndDate,
                SelectedCarId = vm.SelectedCarId,
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
                    PartNames = selectedParts,
                    MechanicName = repairCard.Mechanic.FirstName + " " + repairCard.Mechanic.LastName
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
                            PartNames = selectedParts,
                            MechanicName = repairCard.Mechanic.FirstName + " " + repairCard.Mechanic.LastName
                        }); break;
                    case Criteria.Finished:
                        newVm.RepairCardsVM = newVm.RepairCardsVM.Where(rc => rc.EndDate <= DateTime.Now); break;
                    case Criteria.Unfinished:
                        newVm.RepairCardsVM = newVm.RepairCardsVM.Where(rc => rc.EndDate > DateTime.Now); break;

                }
            }
            if (newVm.StartEndDate != null)
            {
                if (newVm.StartEndDate == StartEndDate.StartDate)
                {
                    newVm.RepairCardsVM = newVm.RepairCardsVM.Where(rc => rc.StartDate == newVm.Date);
                }
                else
                {
                    newVm.RepairCardsVM = newVm.RepairCardsVM.Where(rc => rc.EndDate == newVm.Date);
                }
            }
            if (newVm.SelectedCarId != null)
            {
                Car CarToSearch = _context.Cars.FirstOrDefault(car => car.CarId == vm.SelectedCarId);
                newVm.RepairCardsVM = newVm.RepairCardsVM.Where(rc => rc.CarRegistration == CarToSearch.CarRegistration);
            }

            return View("Views/RepairCards/Index.cshtml", newVm);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RepairCards == null)
            {
                return NotFound();
            }

            var repairCard = await _context.RepairCards.FindAsync(id);
            if (repairCard == null)
            {
                return NotFound();
            }

            var cars = _repairCardsService.GetCars();
            var selectListCars = cars
                .Select(cars => new SelectListItem(
                    cars.CarRegistration,
                    cars.CarId.ToString()));

            var parts = _repairCardsService.GetParts();
            parts = parts.Where(part => part.TypeOfRepair == repairCard.TypeOfRepair).ToList();

            var selectListParts = parts
                .Select(parts => new SelectListItem(
                    parts.PartName,
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
                SelectedPartIds = repairCard.Parts.Select(rc => rc.PartId).ToList(),
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
            if (ModelState.IsValid)
            {
                List<Part> SelectedParts = new();
                foreach(var id in editRepairCard.SelectedPartIds)
                {
                    SelectedParts.Add(_context.Parts.FirstOrDefault(p => p.PartId == id));
                }
                _repairCardsService.EditRepairCard(editRepairCard.RepairCardId,
                                                    editRepairCard.StartDate,
                                                    editRepairCard.EndDate,
                                                    editRepairCard.Car,
                                                    editRepairCard.Description,
                                                    editRepairCard.TypeOfRepair,
                                                    SelectedParts,
                                                    editRepairCard.Mechanic);

                return RedirectToAction(nameof(Index));
            }
            return View(editRepairCard);
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
