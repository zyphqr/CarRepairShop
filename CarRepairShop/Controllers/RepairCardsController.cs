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
        public async Task<IActionResult> Index()
        {
            var repairCards = _context.RepairCards.Include(r => r.Car).Include(r => r.Mechanic);
            return View(await repairCards.ToListAsync());
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create(int repairCardId,
                                    DateTime startDate,
                                    DateTime endDate,
                                    string description,
                                    TypeOfRepairs typeOfRepair)
        {
            var cars = _shopService.GetCars();
            var selectListCars = cars
                .Select(cars => new SelectListItem(
                    cars.CarRegistration,
                    cars.CarId.ToString()));

            var parts = _shopService.GetParts();
            var selectListParts = parts
                .Select(parts => new SelectListItem(
                    parts.PartName,
                    parts.PartId.ToString()));

            var mechanics = _shopService.GetMechanics();
            var selectListMechanics = mechanics
                .Select(mechanics => new SelectListItem(
                    mechanics.FirstName + " " + mechanics.LastName,
                    mechanics.Id.ToString())); 

            return View("Views/RepairCards/Create.cshtml", new CreateEditRepairCardVM
            {
                RepairCardId = repairCardId,
                StartDate = startDate,
                EndDate = endDate,
                CarRegistrations = selectListCars,
                SelectedCarId = cars.ToList()[0].CarId,
                Description = description,
                TypeOfRepair = typeOfRepair,
                Parts = selectListParts,
                SelectedPartId = parts.ToList()[0].PartId, 
                Mechanics = selectListMechanics,
                SelectedMechanicId = mechanics.ToList()[0].Id
            });
        }

        // POST: RepairCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateEditRepairCardVM createRepairCard)
        {
            
                Car selectedCarReg = _shopService.GetCars().Single(c => c.CarId == createRepairCard.SelectedCarId);
                Part selectedPartId = _shopService.GetParts().Single(p => p.PartId == createRepairCard.SelectedPartId);
                Mechanic selectedMechanicId = _shopService.GetMechanics().Single(m => m.Id == createRepairCard.SelectedMechanicId);

                _shopService.CreateRepairCard(
                    createRepairCard.RepairCardId,
                    createRepairCard.StartDate,
                    createRepairCard.EndDate,
                    selectedCarReg,
                    createRepairCard.Description,
                    createRepairCard.TypeOfRepair,
                    selectedPartId,
                    selectedMechanicId
                    );
                return RedirectToAction(nameof(Index));


            //return View("Views/RepairCards/Create.cshtml", createRepairCard);
        }

        //// GET: RepairCards/Edit/5
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
