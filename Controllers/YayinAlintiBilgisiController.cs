using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaramaMVC.Models;

namespace TaramaMVC.Controllers
{
    public class YayinAlintiBilgisiController : Controller
    {
        private readonly DatabaseContext _context;

        public YayinAlintiBilgisiController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: YayinAlintiBilgisi ialtin@ogu.edu.tr 
        public async Task<IActionResult> Index()
        {
            
            return View(await _context.YayinAlintiBilgisis.Include(t => t.personelYayinBilgileri).Include(t=>t.personelYayinBilgileri.Personel).ToListAsync());
        }

        // GET: YayinAlintiBilgisi/Details/5 ialtin@ogu.edu.tr
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.YayinAlintiBilgisis == null)
            {
                return NotFound();
            }

            var yayinAlintiBilgisi = await _context.YayinAlintiBilgisis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yayinAlintiBilgisi == null)
            {
                return NotFound();
            }

            return View(yayinAlintiBilgisi);
        }

        // GET: YayinAlintiBilgisi/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: YayinAlintiBilgisi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,YayinId,Tip,Ad")] YayinAlintiBilgisi yayinAlintiBilgisi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(yayinAlintiBilgisi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(yayinAlintiBilgisi);
        }

        // GET: YayinAlintiBilgisi/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.YayinAlintiBilgisis == null)
            {
                return NotFound();
            }

            var yayinAlintiBilgisi = await _context.YayinAlintiBilgisis.FindAsync(id);
            if (yayinAlintiBilgisi == null)
            {
                return NotFound();
            }
            return View(yayinAlintiBilgisi);
        }

        // POST: YayinAlintiBilgisi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,YayinId,Tip,Ad")] YayinAlintiBilgisi yayinAlintiBilgisi)
        {
            if (id != yayinAlintiBilgisi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(yayinAlintiBilgisi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YayinAlintiBilgisiExists(yayinAlintiBilgisi.Id))
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
            return View(yayinAlintiBilgisi);
        }

        // GET: YayinAlintiBilgisi/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.YayinAlintiBilgisis == null)
            {
                return NotFound();
            }

            var yayinAlintiBilgisi = await _context.YayinAlintiBilgisis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yayinAlintiBilgisi == null)
            {
                return NotFound();
            }

            return View(yayinAlintiBilgisi);
        }

        // POST: YayinAlintiBilgisi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.YayinAlintiBilgisis == null)
            {
                return Problem("Entity set 'DatabaseContext.YayinAlintiBilgisis'  is null.");
            }
            var yayinAlintiBilgisi = await _context.YayinAlintiBilgisis.FindAsync(id);
            if (yayinAlintiBilgisi != null)
            {
                _context.YayinAlintiBilgisis.Remove(yayinAlintiBilgisi);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool YayinAlintiBilgisiExists(int id)
        {
          return _context.YayinAlintiBilgisis.Any(e => e.Id == id);
        }
    }
}
