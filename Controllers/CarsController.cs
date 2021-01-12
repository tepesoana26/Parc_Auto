using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Parc_Auto.Data;
using Parc_Auto.Models;

namespace Parc_Auto.Controllers
{
    public class CarsController : Controller
    {
        private readonly ParcContext _context;

        public CarsController(ParcContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
             string searchString,
             int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["MarkSortParm"] = String.IsNullOrEmpty(sortOrder) ? "mark_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var cars = from b in _context.Cars
                        select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                cars = cars.Where(s => s.Mark.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "mark_desc":
                    cars = cars.OrderByDescending(b => b.Mark);
                    break;
                case "Price":
                    cars = cars.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    cars = cars.OrderByDescending(b => b.Price);
                    break;
                default:
                    cars = cars.OrderBy(b => b.Mark);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Car>.CreateAsync(cars.AsNoTracking(), pageNumber ?? 1, pageSize)); 

        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.CarID == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Mark,Combustible,Price")] Car car)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(car);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {

                ModelState.AddModelError("", "Unable to save changes. " +  "Try again, and if the problem persists ");
            }
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var studentToUpdate = await _context.Cars.FirstOrDefaultAsync(s => s.CarID == id);
            if (await TryUpdateModelAsync<Car>(
            studentToUpdate,
            "",
            s => s.Mark, s => s.Combustible, s => s.Price))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +  "Try again, and if the problem persists");
                }
            }
            return View(studentToUpdate);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }
            var car = await _context.Cars
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.CarID == id);
            if (car == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again";
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {

                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}
