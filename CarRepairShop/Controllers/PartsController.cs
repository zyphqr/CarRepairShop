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
    [Authorize]
    public class PartsController : Controller
    {
        private readonly ShopDataContext _context;
        private readonly PartsService _partsService;

        public PartsController(ShopDataContext context, PartsService partsService)
        {
            _context = context;
            _partsService = partsService;
        }

        public IActionResult Index()
        {
              return View(_context.Parts.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Views/Parts/Create.cshtml", new PartVM());
        }

        [HttpPost]
        public IActionResult Create(PartVM createPart)
        {
            if (ModelState.IsValid)
            {
                _partsService.CreatePart(createPart.PartId,
                                             createPart.PartName,
                                             createPart.Quantity,
                                             createPart.Price,
                                             createPart.WorkingHours,
                                             createPart.TypeOfRepair);

                return RedirectToAction(nameof(Index));
            }
            return View("Views/Parts/Create.cshtml", createPart);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Parts == null)
            {
                return NotFound();
            }

            var part = _context.Parts.Find(id);
            PartVM editPart = new()
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
        public IActionResult Edit(PartVM editPart)
        {
            if (ModelState.IsValid)
            {
                _partsService.EditPart(editPart.PartId,
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

        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Parts == null)
            {
                return NotFound();
            }

            var part = _context.Parts
                .FirstOrDefault(m => m.PartId == id);
            if (part == null)
            {
                return NotFound();
            }

            if (part.RepairCards.Count!=0)
            {
                return NotFound();
            }

            return View(part);
        }

        
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Parts == null)
            {
                return NotFound();
            }
            var part =  _context.Parts.Find(id);
            if (part != null)
            {
                _context.Parts.Remove(part);
            }
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
