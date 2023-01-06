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
    public class KadromuzController : Controller
    {
        private readonly DatabaseContext _context;

        public KadromuzController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Kadromuz
        public async Task<IActionResult> Index()
        {
              return _context.AnaBilimDals != null ? 
                          View(await _context.AnaBilimDals.ToListAsync()) :
                          Problem("Entity set 'DatabaseContext.AnaBilimDals'  is null.");
        }

        // GET: Kadromuz/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AnaBilimDals == null)
            {
                return NotFound();
            }

            var anaBilimDali = await _context.AnaBilimDals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (anaBilimDali == null)
            {
                return NotFound();
            }

            return View(anaBilimDali);
        }

        // GET: Kadromuz/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kadromuz/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] AnaBilimDali anaBilimDali)
        {
            if (ModelState.IsValid)
            {
                _context.Add(anaBilimDali);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(anaBilimDali);
        }

        // GET: Kadromuz/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AnaBilimDals == null)
            {
                return NotFound();
            }

            var anaBilimDali = await _context.AnaBilimDals.FindAsync(id);
            if (anaBilimDali == null)
            {
                return NotFound();
            }
            return View(anaBilimDali);
        }

        // POST: Kadromuz/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] AnaBilimDali anaBilimDali)
        {
            if (id != anaBilimDali.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(anaBilimDali);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnaBilimDaliExists(anaBilimDali.Id))
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
            return View(anaBilimDali);
        }

        // GET: Kadromuz/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AnaBilimDals == null)
            {
                return NotFound();
            }

            var anaBilimDali = await _context.AnaBilimDals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (anaBilimDali == null)
            {
                return NotFound();
            }

            return View(anaBilimDali);
        }

        // POST: Kadromuz/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AnaBilimDals == null)
            {
                return Problem("Entity set 'DatabaseContext.AnaBilimDals'  is null.");
            }
            var anaBilimDali = await _context.AnaBilimDals.FindAsync(id);
            if (anaBilimDali != null)
            {
                _context.AnaBilimDals.Remove(anaBilimDali);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnaBilimDaliExists(int id)
        {
          return (_context.AnaBilimDals?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
