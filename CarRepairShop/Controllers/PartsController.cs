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
using CarRepairShop.Services;
using Microsoft.AspNetCore.Authorization;

namespace CarRepairShop.Controllers
{
    public class PartsController : Controller
    {
        private readonly MEchanicDataContext _context;
        private readonly ShopService _shopService;

        public PartsController(MEchanicDataContext context, ShopService shopService)
        {
            _context = context;
            _shopService = shopService;
        }

        public async Task<IActionResult> Index()
        {
              return View(await _context.Parts.ToListAsync());
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create(int partId,
                                    string partName,
                                    int quantity,
                                    decimal price,
                                    TypeOfRepairs typeOfRepair)
        {
            return View("Views/Parts/Create.cshtml", new CreateEditPartVM
            {
                PartId = partId,
                PartName = partName,
                Quantity = quantity,
                Price = price,
                TypeOfRepair = typeOfRepair,
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateEditPartVM createPart)
        {
            if (ModelState.IsValid)
            {
                _shopService.CreatePartCheck(createPart.PartId,
                                             createPart.PartName,
                                             createPart.Quantity,
                                             createPart.Price,
                                             createPart.TypeOfRepair);

                return RedirectToAction(nameof(Index));
            }
            return View("Views/Parts/Create.cshtml", createPart);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Parts == null)
            {
                return NotFound();
            }

            var part = await _context.Parts.FindAsync(id);
            CreateEditPartVM editPart = new()
            {
                PartId = part.PartId,
                PartName = part.PartName,
                Quantity = part.Quantity,
                Price = part.Price,
                WorkingHours = part.WorkingHours,
                TypeOfRepair = part.TypeOfRepair
            };
            return View(editPart);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateEditPartVM editPart)
        {
            if (ModelState.IsValid)
            {
                _shopService.EditPartCheck(editPart.PartId,
                                          editPart.PartName,
                                          editPart.Quantity,
                                          editPart.Price,
                                          editPart.WorkingHours,
                                          editPart.TypeOfRepair
                                          );

                return RedirectToAction(nameof(Index));
            }
            return View("Views/Parts/Edit.cshtml", editPart);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Parts == null)
            {
                return NotFound();
            }

            var part = await _context.Parts
                .FirstOrDefaultAsync(m => m.PartId == id);
            if (part == null)
            {
                return NotFound();
            }

            return View(part);
        }

        
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Parts == null)
            {
                return Problem("Entity set 'MEchanicDataContext.Parts'  is null.");
            }
            var part = await _context.Parts.FindAsync(id);
            if (part != null)
            {
                _context.Parts.Remove(part);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartExists(int id)
        {
          return _context.Parts.Any(e => e.PartId == id);
        }
    }
}
