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
        private readonly ShopService _shopService;

        public RepairCardsController(MEchanicDataContext context, ShopService shopService)
        {
            _context = context;
            _shopService = shopService;
        }

        // GET: RepairCards
        [Authorize]
        public IActionResult Index()
        {
            var repairCards = _shopService.GetAllRepairCards();
            var parts = _shopService.GetAllParts();

            var cars = _shopService.GetCars();
            var selectListCars = cars
                .Select(cars => new SelectListItem(
                    cars.CarRegistration,
                    cars.CarId.ToString()));

            var indexVM = new IndexVMs
            {
                RepairCards = repairCards.Select(repairCard => new RepairCardIndexVM
                {
                    RepairCardId = repairCard.RepairCardId,
                    StartDate = repairCard.StartDate,
                    EndDate = repairCard.EndDate,
                    CarRegistration = repairCard.Car.CarRegistration,
                    Description = repairCard.Description,
                    Price = repairCard.Price,
                    TypeOfRepair = repairCard.TypeOfRepair,
                    PartNames = repairCard.Parts.Select(p => p.PartName),
                    MechanicName = repairCard.Mechanic.FirstName + " " + repairCard.Mechanic.FirstName
                }),
                CarRegistrations = selectListCars,
                SelectedCarId = cars.ToList()[0].CarId,
            };

            return View(indexVM);
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

            return View("Views/RepairCards/Create.cshtml", new CreateEditRepairCardVM
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
        [HttpGet]
        public IActionResult SelectRepair()
        {
           

            return View("Views/RepairCards/TypeOfRepairForm.cshtml", new CreateEditRepairCardVM
            {
                
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult SelectRepair(CreateEditRepairCardVM vm)
        {
            var cars = _shopService.GetCars();
            var selectListCars = cars
                .Select(cars => new SelectListItem(
                    cars.CarRegistration,
                    cars.CarId.ToString()));

            var parts = _shopService.GetParts();
            parts = parts.Where(part => part.TypeOfRepair == vm.TypeOfRepair).ToList();
            List<CreateEditPartVM> partVMs = new(); //TODO: Might cause issues due typing missmatching

            foreach(Part part in parts)
            {
                CreateEditPartVM newvm = new()
                {
                    PartName = part.PartName,
                    Quantity = part.Quantity,
                    Price = part.Price,
                    WorkingHours = part.WorkingHours,
                    PartId = part.PartId,
                    TypeOfRepair = part.TypeOfRepair,
                };

                partVMs.Add(newvm);
            }
            

            var mechanics = _shopService.GetMechanics();
            var selectListMechanics = mechanics
                .Select(mechanics => new SelectListItem(
                    mechanics.FirstName + " " + mechanics.LastName,
                    mechanics.Id.ToString()));

            return View("Views/RepairCards/Create.cshtml", new CreateEditRepairCardVM 
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                CarRegistrations = selectListCars,
                SelectedCarId = cars.ToList()[0].CarId,
                TypeOfRepair = vm.TypeOfRepair,
                partVMs = partVMs,
                Mechanics = selectListMechanics,
                SelectedMechanicId = mechanics.ToList()[0].Id
            });
        }

        [Authorize]
        public IActionResult References(IndexVMs vm)
        {
            var repairCards = _shopService.GetAllRepairCards();

            var newVm = new IndexVMs
            {
                Criteria = vm.Criteria,
                StartEndDate = vm.StartEndDate,
                SelectedCarId= vm.SelectedCarId,
                Date= vm.Date,
                RepairCards = repairCards.Select(repairCard => new RepairCardIndexVM
                {
                    RepairCardId = repairCard.RepairCardId,
                    StartDate = repairCard.StartDate,
                    EndDate = repairCard.EndDate,
                    CarRegistration = repairCard.Car.CarRegistration,
                    Description = repairCard.Description,
                    Price = repairCard.Price,
                    TypeOfRepair = repairCard.TypeOfRepair,
                    PartNames = repairCard.Parts.Select(p => p.PartName).ToList(),
                    MechanicName = repairCard.Mechanic.FirstName + " " + repairCard.Mechanic.LastName
                }),
                
            };

            if (newVm.Criteria!=null)
            {
                switch (newVm.Criteria)
                {
                    case Criteria.All:
                        newVm.RepairCards = repairCards.Select(repairCard => new RepairCardIndexVM
                        {
                            RepairCardId = repairCard.RepairCardId,
                            StartDate = repairCard.StartDate,
                            EndDate = repairCard.EndDate,
                            CarRegistration = repairCard.Car.CarRegistration,
                            Description = repairCard.Description,
                            Price = repairCard.Price,
                            TypeOfRepair = repairCard.TypeOfRepair,
                            PartNames = repairCard.Parts.Select(p => p.PartName).ToList(),
                            MechanicName = repairCard.Mechanic.FirstName + " " + repairCard.Mechanic.LastName
                        }); break;
                    case Criteria.Finished:
                        newVm.RepairCards = newVm.RepairCards.Where(rc => rc.EndDate <= DateTime.Now); break;
                    case Criteria.Unfinished:
                        newVm.RepairCards = newVm.RepairCards.Where(rc => rc.EndDate > DateTime.Now); break;

                }
            }
            if(newVm.StartEndDate!=null)
            {
                if(newVm.StartEndDate == StartEndDate.StartDate)
                {
                    newVm.RepairCards = newVm.RepairCards.Where(rc => rc.StartDate == newVm.Date);
                }
                else
                {
                    newVm.RepairCards = newVm.RepairCards.Where(rc => rc.EndDate == newVm.Date);
                }
            }
            if(newVm.SelectedCarId!=null)
            {
                Car CarToSearch = _context.Cars.FirstOrDefault(car => car.CarId == vm.SelectedCarId);
                newVm.RepairCards = newVm.RepairCards.Where(rc => rc.CarRegistration == CarToSearch.CarRegistration);
            }
           
            return View("Views/RepairCards/Index.cshtml", newVm);
        }

        // POST: RepairCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateEditRepairCardVM createRepairCard)
        {

            Car selectedCarReg = _shopService.GetCars().Single(c => c.CarId == createRepairCard.SelectedCarId);
            List<CreateEditPartVM> selectedParts = createRepairCard.partVMs.Where(part=>part.IsSelected == true).ToList();
            Mechanic selectedMechanicId = _shopService.GetMechanics().Single(m => m.Id == createRepairCard.SelectedMechanicId);


            _shopService.CreateRepairCard(
                createRepairCard.RepairCardId,
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

        //// GET: RepairCards/Edit/5
        ///[Authorize]
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.RepairCards == null)
        //    {
        //        return NotFound();
        //    }

        //    var repairCard = await _context.RepairCards.FindAsync(id);
        //    if (repairCard == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["RepairCardId"] = new SelectList(_context.Cars, "CarId", "CarId", repairCard.RepairCardId);
        //    ViewData["MechanicId"] = new SelectList(_context.Mechanics, "Id", "Id", repairCard.MechanicId);
        //    return View(repairCard);
        //}

        //// POST: RepairCards/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("RepairCardId,StartDate,EndDate,CarRegistration,Description,Price,TypeOfRepair,MechanicId")] RepairCard repairCard)
        //{
        //    if (id != repairCard.RepairCardId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(repairCard);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!RepairCardExists(repairCard.RepairCardId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["RepairCardId"] = new SelectList(_context.Cars, "CarId", "CarId", repairCard.RepairCardId);
        //    ViewData["MechanicId"] = new SelectList(_context.Mechanics, "Id", "Id", repairCard.MechanicId);
        //    return View(repairCard);
        //}

        //// GET: RepairCards/Delete/5
        ///[Authorize]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.RepairCards == null)
        //    {
        //        return NotFound();
        //    }

        //    var repairCard = await _context.RepairCards
        //        .Include(r => r.Car)
        //        .Include(r => r.Mechanic)
        //        .FirstOrDefaultAsync(m => m.RepairCardId == id);
        //    if (repairCard == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(repairCard);
        //}

        //// POST: RepairCards/Delete/5
        ///[Authorize]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.RepairCards == null)
        //    {
        //        return Problem("Entity set 'MEchanicDataContext.RepairCards'  is null.");
        //    }
        //    var repairCard = await _context.RepairCards.FindAsync(id);
        //    if (repairCard != null)
        //    {
        //        _context.RepairCards.Remove(repairCard);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool RepairCardExists(int id)
        //{
        //    return _context.RepairCards.Any(e => e.RepairCardId == id);
        //}
    }
}
