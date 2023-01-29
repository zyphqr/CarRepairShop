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
using System.Net;

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
                                    CarBrands brand,
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
            return View("Views/Cars/Create.cshtml", new CreateEditCarVM
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
        public IActionResult Create(CreateEditCarVM createCar)
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

        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }
            var car = _context.Cars.Find(id);
            CreateEditCarVM editCar = new()
            {
                CarId = car.CarId,
                CarRegistration = car.CarRegistration,
                Brand = car.CarBrand,
                Model = car.CarModel,
                YearOfManifacture = car.YearOfManifacture,
                EngineNum = car.EngineNum,
                FrameNum = car.FrameNum,
                Color = car.Color,
                WorkingVolume = car.WorkingVolume,
                Description = car.Description,
                OwnerName = car.Owner,
                OwnerPhoneNum = car.OwnerPhoneNum,
            };
            return View(editCar);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(CreateEditCarVM editCar)
        {
            if (ModelState.IsValid)
            {
                _shopService.EditCarCheck(editCar.CarId,
                    editCar.CarRegistration,
                    editCar.Brand,
                    editCar.Model,
                    editCar.YearOfManifacture,
                    editCar.EngineNum,
                    editCar.FrameNum,
                    editCar.Color,
                    editCar.WorkingVolume,
                    editCar.Description,
                    editCar.OwnerName,
                    editCar.OwnerPhoneNum
                    );
                
                return RedirectToAction(nameof(Index));
            }
            return View("Views/Cars/Edit.cshtml", editCar);
        }

        [Authorize]
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

        [Authorize]
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
    }
}
