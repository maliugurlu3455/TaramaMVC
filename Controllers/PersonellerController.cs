using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaramaMVC.Models;

namespace TaramaMVC.Controllers
{
    public class PersonellerController : Controller
    {
        private readonly DatabaseContext _context;

        public PersonellerController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Personeller
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Personels.Include(p => p.AnaBilimDallari);
            
            return View(await databaseContext.ToListAsync());
        }

        // GET: Personeller/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Personels == null)
            {
                return NotFound();
            }

            var personel = await _context.Personels
                .Include(p => p.AnaBilimDallari)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personel == null)
            {
                return NotFound();
            }

            return View(personel);
        }

        // GET: Personeller/Create
        public IActionResult Create()
        {
            ViewData["AnaBilimDallariId"] = new SelectList(_context.AnaBilimDals, "Id", "Name");
            return View();
        }

        // POST: Personeller/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,SurName,AnaBilimDallariId")] Personel personel)
        {
            if (ModelState.IsValid)
            {
                //personel.Name = personel.Name;
                
                _context.Add(personel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnaBilimDallariId"] = new SelectList(_context.AnaBilimDals, "Id", "Name", personel.AnaBilimDallariId);
            return View(personel);
        }

        // GET: Personeller/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Personels == null)
            {
                return NotFound();
            }

            var personel = await _context.Personels.FindAsync(id);
            if (personel == null)
            {
                return NotFound();
            }
            ViewData["AnaBilimDallariId"] = new SelectList(_context.AnaBilimDals, "Id", "Name", personel.AnaBilimDallariId);
            return View(personel);
        }

        // POST: Personeller/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SurName,AnaBilimDallariId")] Personel personel)
        {
            if (id != personel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonelExists(personel.Id))
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
            ViewData["AnaBilimDallariId"] = new SelectList(_context.AnaBilimDals, "Id", "Name", personel.AnaBilimDallariId);
            return View(personel);
        }

        // GET: Personeller/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Personels == null)
            {
                return NotFound();
            }

            var personel = await _context.Personels
                .Include(p => p.AnaBilimDallari)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personel == null)
            {
                return NotFound();
            }

            return View(personel);
        }

        // POST: Personeller/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Personels == null)
            {
                return Problem("Entity set 'DatabaseContext.Personels'  is null.");
            }
            var personel = await _context.Personels.FindAsync(id);
            if (personel != null)
            {
                _context.Personels.Remove(personel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonelExists(int id)
        {
          return _context.Personels.Any(e => e.Id == id);
        }
    }
}
