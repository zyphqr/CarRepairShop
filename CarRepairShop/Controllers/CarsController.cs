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
using CarRepairShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using CarRepairShop.Services;

namespace CarRepairShop.Controllers
{
    public class CarsController : Controller
    {
        private readonly MEchanicDataContext _context;
        private readonly ShopService _shopService;

        public CarsController(MEchanicDataContext context,
                              ShopService shopService)
        {
            _context = context;
            _shopService = shopService;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cars.ToListAsync());
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create(int carId,
                                    string carRegistation,
                                    string brand,
                                    string model,
                                    YearsOfManifacture yearOfManifacture,
                                    string engineNum,
                                    string frameNum,
                                    Colors color,
                                    double workingVolume,
                                    string description,
                                    string ownerName,
                                    string ownerPhoneNum)
        {
            return View("Views/Cars/Create.cshtml", new CreateCarVM
            {
                CarId = carId,
                CarRegistration = carRegistation,
                Brand = brand,
                Model = model,
                YearOfManifacture = yearOfManifacture,
                EngineNum = engineNum,
                FrameNum = frameNum,
                Color = color,
                WorkingVolume = workingVolume,
                Description = description,
                OwnerName = ownerName,
                OwnerPhoneNum = ownerPhoneNum,
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CreateCarVM createCar)
        {
            if (ModelState.IsValid)
            {
                _shopService.CreateCarCheck(
                    createCar.CarId,
                    createCar.CarRegistration,
                    createCar.Brand,
                    createCar.Model,
                    createCar.YearOfManifacture,
                    createCar.EngineNum,
                    createCar.FrameNum,
                    createCar.Color,
                    createCar.WorkingVolume,
                    createCar.Description,
                    createCar.OwnerName,
                    createCar.OwnerPhoneNum
                 );
                return RedirectToAction(nameof(Index));
            }
            return View("Views/Cars/Create.cshtml", createCar);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarId,CarRegistration,CarBrand,CarModel,YearOfManifacture,EngineNum,FrameNum,Color,WorkingVolume,Description,Owner,OwnerPhoneNum")] Car car)
        {
            if (id != car.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.CarId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cars == null)
            {
                return Problem("Entity set 'MEchanicDataContext.Cars'  is null.");
            }
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }
    }
}
