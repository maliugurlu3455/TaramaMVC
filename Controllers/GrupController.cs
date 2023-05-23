using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TaramaMVC.Models;

namespace TaramaMVC.Controllers
{
    public class GrupController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;
        private readonly DatabaseContext _context;

        public GrupController(DatabaseContext context, RoleManager<IdentityRole> roleMgr, UserManager<AppUser> userMrg)
        {
            roleManager = roleMgr;
            userManager = userMrg;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Grups;

            return View(await databaseContext.ToListAsync());
        }


        // GET: ApiKey/Create
        ////[Authorize]
        public IActionResult Create()
        {

            return View();
        }

        // POST: ApiKey/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        ////[Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name")] Grup grups)
        {
            if (ModelState.IsValid)
            {
                //personel.Name = personel.Name;

                _context.Add(grups);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(grups);
        }

        ////[Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Grups == null)
            {
                return NotFound();
            }

            var grups = await _context.Grups.FindAsync(id);
            if (grups == null)
            {
                return NotFound();
            }

            return View(grups);
        }


        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        ////[Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Grup grups)
        {
            if (id != grups.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grups);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GrupsExists(grups.Id))
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

            return View(grups);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Grups == null)
            {
                return NotFound();
            }

            var grups = await _context.Grups.FirstOrDefaultAsync(m => m.Id == id);
            if (grups == null)
            {
                return NotFound();

            }
            return View(grups);


        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deletem(int? id)
        {
            if (id == null || _context.Grups == null)
            {
                return NotFound();
            }

            var grups = await _context.Grups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grups != null)
            {
                _context.Grups.Remove(grups);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index));


        }

        private bool GrupsExists(int id)
        {
            return _context.Grups.Any(e => e.Id == id);
        }
    }
}
