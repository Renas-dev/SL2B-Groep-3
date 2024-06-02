using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Dierentuin_App.Models;

namespace Dierentuin_App.Controllers
{
    public class StallsController : Controller
    {
        public static List<Stall> _stalls = new List<Stall>();

        // GET: Stalls/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View(_stalls);
        }

        // GET: Stalls/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stalls/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Stall stall)
        {
            if (ModelState.IsValid)
            {
                _stalls.Add(stall);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Stalls/Edit/id
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var stall = _stalls.FirstOrDefault(s => s.Id == id);
            if (stall == null)
            {
                return NotFound();
            }
            return View(stall);
        }

        // POST: Stalls/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Stall updatedStall)
        {
            if (id != updatedStall.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingStall = _stalls.FirstOrDefault(s => s.Id == id);
                if (existingStall == null)
                {
                    return NotFound();
                }

                existingStall.Name = updatedStall.Name;
                existingStall.Biome = updatedStall.Biome;
                existingStall.Climate = updatedStall.Climate;

                return RedirectToAction(nameof(Index));
            }

            return View(updatedStall);
        }

        // POST: Stalls/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int stallId)
        {
            var stall = _stalls.FirstOrDefault(s => s.Id == stallId);
            if (stall != null)
            {
                _stalls.Remove(stall);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
