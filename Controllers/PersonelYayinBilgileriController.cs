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
    public class PersonelYayinBilgileriController : Controller
    {
        private readonly DatabaseContext _context;

        public PersonelYayinBilgileriController(DatabaseContext context)
        {
           
            _context = context;
        }

        // GET: PersonelYayinBilgileri
        public async Task<IActionResult> Index()
        {
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "ScholarName");
            return View(await _context.PersonelYayinBilgileris.ToListAsync());
        }

        // GET: PersonelYayinBilgileri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PersonelYayinBilgileris == null)
            {
                return NotFound();
            }

            var personelYayinBilgileri = await _context.PersonelYayinBilgileris
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personelYayinBilgileri == null)
            {
                return NotFound();
            }

            return View(personelYayinBilgileri);
        }

        // GET: PersonelYayinBilgileri/Create
        public IActionResult Create()
        {

            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "ScholarName");
            return View();
        }

        // POST: PersonelYayinBilgileri/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Baslik,Alinti,Yil,UpdateDate,PersonelId")] PersonelYayinBilgileri personelYayinBilgileri)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personelYayinBilgileri);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(personelYayinBilgileri);
        }

        // GET: PersonelYayinBilgileri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PersonelYayinBilgileris == null)
            {
                return NotFound();
            }

            var personelYayinBilgileri = await _context.PersonelYayinBilgileris.FindAsync(id);
            if (personelYayinBilgileri == null)
            {
                return NotFound();
            }
            return View(personelYayinBilgileri);
        }

        // POST: PersonelYayinBilgileri/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Baslik,Alinti,Yil,UpdateDate,PersonelId")] PersonelYayinBilgileri personelYayinBilgileri)
        {
            if (id != personelYayinBilgileri.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personelYayinBilgileri);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonelYayinBilgileriExists(personelYayinBilgileri.Id))
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
            return View(personelYayinBilgileri);
        }

        // GET: PersonelYayinBilgileri/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PersonelYayinBilgileris == null)
            {
                return NotFound();
            }

            var personelYayinBilgileri = await _context.PersonelYayinBilgileris
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personelYayinBilgileri == null)
            {
                return NotFound();
            }

            return View(personelYayinBilgileri);
        }

        // POST: PersonelYayinBilgileri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PersonelYayinBilgileris == null)
            {
                return Problem("Entity set 'DatabaseContext.PersonelYayinBilgileris'  is null.");
            }
            var personelYayinBilgileri = await _context.PersonelYayinBilgileris.FindAsync(id);
            if (personelYayinBilgileri != null)
            {
                _context.PersonelYayinBilgileris.Remove(personelYayinBilgileri);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonelYayinBilgileriExists(int id)
        {
          return _context.PersonelYayinBilgileris.Any(e => e.Id == id);
        }
    }
}
