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
    public class ParamController : Controller
    {
        private readonly DatabaseContext _context;

        public ParamController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Param
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Parametrelers.Include(p => p.Grup);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Param/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Parametrelers == null)
            {
                return NotFound();
            }

            var parametreler = await _context.Parametrelers
                .Include(p => p.Grup)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parametreler == null)
            {
                return NotFound();
            }

            return View(parametreler);
        }

        // GET: Param/Create
        public IActionResult Create()
        {
            ViewData["GrupId"] = new SelectList(_context.Grups, "Id", "Name");
            return View();
        }

        // POST: Param/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Value,GrupId")] Parametreler parametreler)
        {
            if (parametreler.GrupId>0)
            {
                var grub = _context.Grups.FirstOrDefault(r => r.Id == parametreler.GrupId);
                if (grub!=null)
                {
                    parametreler.Grup = grub;
                    
                }
                

            }
            if (ModelState.IsValid)
            {
                _context.Parametrelers.Add(parametreler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GrupId"] = new SelectList(_context.Grups, "Id", "Name", parametreler.GrupId);
            return View(parametreler);
        }

        // GET: Param/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Parametrelers == null)
            {
                return NotFound();
            }

            var parametreler = await _context.Parametrelers.FindAsync(id);
            if (parametreler == null)
            {
                return NotFound();
            }
            ViewData["GrupId"] = new SelectList(_context.Grups, "Id", "Name", parametreler.GrupId);
            return View(parametreler);
        }

        // POST: Param/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Value,GrupId")] Parametreler parametreler)
        {
            if (id != parametreler.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parametreler);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParametrelerExists(parametreler.Id))
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
            ViewData["GrupId"] = new SelectList(_context.Grups, "Id", "Id", parametreler.GrupId);
            return View(parametreler);
        }

        // GET: Param/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Parametrelers == null)
            {
                return NotFound();
            }

            var parametreler = await _context.Parametrelers
                .Include(p => p.Grup)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parametreler == null)
            {
                return NotFound();
            }

            return View(parametreler);
        }

        // POST: Param/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Parametrelers == null)
            {
                return Problem("Entity set 'DatabaseContext.Parametrelers'  is null.");
            }
            var parametreler = await _context.Parametrelers.FindAsync(id);
            if (parametreler != null)
            {
                _context.Parametrelers.Remove(parametreler);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParametrelerExists(int id)
        {
          return (_context.Parametrelers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
