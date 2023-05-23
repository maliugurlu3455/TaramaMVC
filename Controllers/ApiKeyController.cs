using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaramaMVC.Models;

namespace TaramaMVC.Controllers
{
    public class ApiKeyController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;
        private readonly DatabaseContext _context;

        public ApiKeyController(DatabaseContext context,RoleManager<IdentityRole> roleMgr, UserManager<AppUser> userMrg)
        {
            roleManager = roleMgr;
            userManager = userMrg;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.ApiKey;

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
        public async Task<IActionResult> Create([Bind("Id,Key")] ApiKeys apiKeys)
        {
            if (ModelState.IsValid)
            {
                //personel.Name = personel.Name;

                _context.Add(apiKeys);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(apiKeys);
        }
       
        ////[Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ApiKey == null)
            {
                return NotFound();
            }

            var apiKey = await _context.ApiKey.FindAsync(id);
            if (apiKey == null)
            {
                return NotFound();
            }
           
            return View(apiKey);
        }

       
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        ////[Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Key,Sayi,IsTamam")] ApiKeys apiKey)
        {
            if (id != apiKey.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apiKey);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApiKeyExists(apiKey.Id))
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
            
            return View(apiKey);
        }
        [HttpGet]
         public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ApiKey == null)
            {
                return NotFound();
            }

            var apiKey = await _context.ApiKey.FirstOrDefaultAsync(m => m.Id == id);
            if (apiKey == null)
            {
                return NotFound();

            }
            return View(apiKey);
      

        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>  Deletem(int? id)
        {
            if (id == null || _context.ApiKey == null)
            {
                return NotFound();
            }

            var apiKey = await _context.ApiKey
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apiKey != null)
            {
                _context.ApiKey.Remove(apiKey);
                await _context.SaveChangesAsync();
               
            }
            return RedirectToAction(nameof(Index));


        }

        private bool ApiKeyExists(int id)
        {
            return _context.ApiKey.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Sifirla()
        {


            var apiKey = await _context.ApiKey.ToListAsync();
            if (apiKey == null)
            {
                return NotFound();
            }
            try
            {
                for (int i = 0; i < apiKey.Count; i++)
                {
                    var api = apiKey[i];
                    api.Sayi = 100;
                    api.IsTamam = false;
                    _context.ApiKey.Update(api);
                }
                _context.SaveChangesAsync().Wait();
            }
            catch (Exception ex)
            {

                return Content(ex.Message);
            }

            return RedirectToAction(nameof(Index));
            // return View(apiKey);
        }
        // To protect from overposting attacks, enable the specific properties you want to bind to. Sifirla
    }
}
