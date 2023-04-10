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
        private readonly MEchanicDataContext _context;
        private readonly RepairCardsService _repairCardsService;

        public RepairCardsController(MEchanicDataContext context, RepairCardsService repairCardsService)
        {
            _context = context;
            _repairCardsService = repairCardsService;
        }

        public IActionResult Index()
        {
            var repairCards = _repairCardsService.GetAllRepairCards();

            var indexVM = new FilteringVM
            {
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
                    Parts = _repairCardsService.SearchedParts(_context.RepairCards.Include(x => x.Parts).FirstOrDefault(rc => rc.RepairCardId == repairCard.RepairCardId)),
                })
            };
            return View(indexVM);
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
                EndDate = DateTime.Now,
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
                EndDate = DateTime.Now,
                CarRegistrations = selectListCars,
                SelectedCarId = cars.ToList()[0].CarId,
                TypeOfRepair = TypeOfRepair,
                Parts = selectListParts,
                Mechanics = selectListMechanics,
                SelectedMechanicId = mechanics.ToList()[0].Id
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                SelectedCarId = 0,
                CarRegistrations = selectListCars,
                SelectedMechanicId = "All",
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
                    Parts = _repairCardsService.SearchedParts(_context.RepairCards.FirstOrDefault(rc => rc.RepairCardId == repairCard.RepairCardId))
                }),

            };

            if (indexVM.Criteria != null)
            {
                if (indexVM.Criteria == Criteria.All)
                {
                    indexVM.RepairCardIndexVM = repairCards.Select(repairCard => new RepairCardIndexVM
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
                        Parts = _repairCardsService.SearchedParts(_context.RepairCards.FirstOrDefault(rc => rc.RepairCardId == repairCard.RepairCardId))
                    });
                }
                else if (indexVM.Criteria == Criteria.Finished)
                {
                    indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                    .Where(rc => rc.EndDate != null);
                }
                else if (indexVM.Criteria == Criteria.Unfinished)
                {
                    indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                    .Where(rc => rc.EndDate == null);
                }

                //switch (indexVM.Criteria)
                //{
                //    case Criteria.All:
                //        indexVM.RepairCardIndexVM = repairCards.Select(repairCard => new RepairCardIndexVM
                //        {
                //            RepairCardId = repairCard.RepairCardId,
                //            StartDate = repairCard.StartDate,
                //            EndDate = repairCard.EndDate,
                //            CarRegistration = repairCard.Car.CarRegistration,
                //            Description = repairCard.Description,
                //            Price = repairCard.Price,
                //            TypeOfRepair = repairCard.TypeOfRepair,
                //            MechanicFirstName = repairCard.Mechanic.FirstName,
                //            MechanicLastName = repairCard.Mechanic.LastName,
                //            Parts = _repairCardsService.SearchedParts(_context.RepairCards.FirstOrDefault(rc => rc.RepairCardId == repairCard.RepairCardId))
                //        }); break;

                //    case Criteria.Finished:
                //        indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                //                                    .Where(rc => rc.EndDate != null); break;

                //    case Criteria.Unfinished:
                //        indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                //                                    .Where(rc => rc.EndDate == null); break;
                //}
            }
            if (indexVM.TypeOfRepair != null)
            {
                if (indexVM.TypeOfRepair == TypeOfRepairs.All)
                {
                    indexVM.RepairCardIndexVM = repairCards.Select(repairCard => new RepairCardIndexVM
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
                        Parts = _repairCardsService.SearchedParts(_context.RepairCards.FirstOrDefault(rc => rc.RepairCardId == repairCard.RepairCardId))
                    });
                }
                else if (indexVM.TypeOfRepair == TypeOfRepairs.EngineRepair)
                {
                    indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                    .Where(rc => rc.TypeOfRepair == TypeOfRepairs.EngineRepair);
                }
                else if (indexVM.TypeOfRepair == TypeOfRepairs.CoolingSystemRepair)
                {
                    indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                    .Where(rc => rc.TypeOfRepair == TypeOfRepairs.CoolingSystemRepair);
                }
                else if (indexVM.TypeOfRepair == TypeOfRepairs.ExhaustSystemRepair)
                {
                    indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                    .Where(rc => rc.TypeOfRepair == TypeOfRepairs.ExhaustSystemRepair);
                }
                else if (indexVM.TypeOfRepair == TypeOfRepairs.FuelSystemRepair)
                {
                    indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                    .Where(rc => rc.TypeOfRepair == TypeOfRepairs.FuelSystemRepair);
                }
                else if (indexVM.TypeOfRepair == TypeOfRepairs.ElectricSystemRepair)
                {
                    indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                    .Where(rc => rc.TypeOfRepair == TypeOfRepairs.ElectricSystemRepair);
                }
                else if (indexVM.TypeOfRepair == TypeOfRepairs.BrakingSystemRepair)
                {
                    indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                    .Where(rc => rc.TypeOfRepair == TypeOfRepairs.BrakingSystemRepair);
                }
                //switch (indexVM.TypeOfRepair)
                //{
                //    case TypeOfRepairs.All:
                //        indexVM.RepairCardIndexVM = repairCards.Select(repairCard => new RepairCardIndexVM
                //        {
                //            RepairCardId = repairCard.RepairCardId,
                //            StartDate = repairCard.StartDate,
                //            EndDate = repairCard.EndDate,
                //            CarRegistration = repairCard.Car.CarRegistration,
                //            Description = repairCard.Description,
                //            Price = repairCard.Price,
                //            TypeOfRepair = repairCard.TypeOfRepair,
                //            MechanicFirstName = repairCard.Mechanic.FirstName,
                //            MechanicLastName = repairCard.Mechanic.LastName,
                //            Parts = _repairCardsService.SearchedParts(_context.RepairCards.FirstOrDefault(rc => rc.RepairCardId == repairCard.RepairCardId))
                //        }); break;

                //    case TypeOfRepairs.EngineRepair:
                //        indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                //                                    .Where(rc => rc.TypeOfRepair == TypeOfRepairs.EngineRepair); break;

                //    case TypeOfRepairs.CoolingSystemRepair:
                //        indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                //                                    .Where(rc => rc.TypeOfRepair == TypeOfRepairs.CoolingSystemRepair); break;

                //    case TypeOfRepairs.ExhaustSystemRepair:
                //        indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                //                                    .Where(rc => rc.TypeOfRepair == TypeOfRepairs.ExhaustSystemRepair); break;

                //    case TypeOfRepairs.FuelSystemRepair:
                //        indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                //                                    .Where(rc => rc.TypeOfRepair == TypeOfRepairs.FuelSystemRepair); break;

                //    case TypeOfRepairs.ElectricSystemRepair:
                //        indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                //                                    .Where(rc => rc.TypeOfRepair == TypeOfRepairs.ElectricSystemRepair); break;

                //    case TypeOfRepairs.BrakingSystemRepair:
                //        indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                //                                    .Where(rc => rc.TypeOfRepair == TypeOfRepairs.BrakingSystemRepair); break;
                //}
            }
            if(indexVM.StartEndDate != null)
            {
                if(indexVM.StartEndDate == StartEndDate.All)
                {
                    indexVM.RepairCardIndexVM = repairCards.Select(repairCard => new RepairCardIndexVM
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
                        Parts = _repairCardsService.SearchedParts(_context.RepairCards.FirstOrDefault(rc => rc.RepairCardId == repairCard.RepairCardId))
                    });

                }
                else if(indexVM.StartEndDate == StartEndDate.StartDate)
                {
                    indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                .Where(rc => rc.StartDate >= indexVM.Date);
                }
                else
                {
                    indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                .Where(rc => rc.EndDate <= indexVM.Date);
                }
            }
            if (indexVM.SelectedCarId != null)//ToDo ; fucked
            {
                if (indexVM.SelectedCarId == 1)
                {
                    indexVM.RepairCardIndexVM = repairCards.Select(repairCard => new RepairCardIndexVM
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
                        Parts = _repairCardsService.SearchedParts(_context.RepairCards.FirstOrDefault(rc => rc.RepairCardId == repairCard.RepairCardId))
                    });
                }
                else
                {
                    Car CarToSearch = _context.Cars.FirstOrDefault(car => car.CarId == vm.SelectedCarId);

                    indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                .Where(rc => rc.CarRegistration == CarToSearch.CarRegistration);
                }

            }
            if (indexVM.SelectedMechanicId != null)//ToDo also fucked
            {
                if (indexVM.SelectedMechanicId == "All")
                {
                    indexVM.RepairCardIndexVM = repairCards.Select(repairCard => new RepairCardIndexVM
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
                        Parts = _repairCardsService.SearchedParts(_context.RepairCards.FirstOrDefault(rc => rc.RepairCardId == repairCard.RepairCardId))
                    });
                }
                else
                {
                    Mechanic MechanicToSearch = _context.Mechanics.FirstOrDefault(m => m.Id == vm.SelectedMechanicId);

                    indexVM.RepairCardIndexVM = indexVM.RepairCardIndexVM
                                                .Where(rc => rc.MechanicFirstName == MechanicToSearch.FirstName
                                                          && rc.MechanicLastName == MechanicToSearch.LastName);
                }

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
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RepairCardVM editRepairCard)
        {
            Car selectedCarReg = _repairCardsService
                                 .GetCars()
                                 .Single(c => c.CarId == editRepairCard.SelectedCarId);

            List<Part> SelectedParts = new();

            Mechanic selectedMechanicId = _repairCardsService
                                          .GetMechanics()
                                          .Single(m => m.Id == editRepairCard.SelectedMechanicId);

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
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.RepairCards == null)
            {
                return Problem("Entity set 'MEchanicDataContext.RepairCards'  is null.");
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
