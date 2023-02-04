using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using TaramaMVC.Helper;
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
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "Name");
            return View();
        }

        // POST: Personeller/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,SurName,AnaBilimDallariId,ScholarName,User,Alintilanma")] Personel personel)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SurName,AnaBilimDallariId,ScholarName,User,Alintilanma")] Personel personel)
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
        public async Task<IActionResult> VerileriSil()
        {
            string mesaj = "";
            try
            {
                var silinecekler = await _context.PersonelYayinBilgileris.ToListAsync();
                _context.PersonelYayinBilgileris.RemoveRange(silinecekler);

                _context.Personels.ExecuteUpdate(p => p.SetProperty(x => x.Alintilanma, x =>0 ));
                 
                await _context.SaveChangesAsync();
                mesaj = "Veriler Silindi.";
            }
            catch (Exception ex)
            {

                mesaj = "Hata : "+ ex.Message;
            }
            
            return Content(mesaj);
        }
            public async Task<IActionResult> VeriGuncelle()
        {
            List<PersonelYayinBilgileri> UserList = new List<PersonelYayinBilgileri>();
            var databaseContext = _context.Personels.Where(r=>r.Name.Contains("slam")).ToList();
            foreach (var item in databaseContext)
            {
               //web servisten kullanıcı bilgilerini al
                UserList = Helperim.KullaniciBilgileri(item.User);
                var person = await _context.Personels.FirstOrDefaultAsync(i => i.Id == item.Id);
                if (UserList!=null && UserList.Count>0)
                {
                    foreach (PersonelYayinBilgileri py in UserList)
                    {
                        person.Alintilanma = py.Personel.Alintilanma;
                        py.Personel = person;
                        py.PersonelId = person.Id;
                        py.UpdateDate = DateTime.Today;

                        await _context.PersonelYayinBilgileris.AddAsync(py);
                        _context.Personels.Update(person);
                    }
                    await _context.SaveChangesAsync();
                }
             
               
            }
            return RedirectToAction("Index");
        }
        public IActionResult Test() {
            string deger = Helperim.AlintiGetir("https://scholar.google.com/scholar?oi=bibs&hl=tr&cites=4986682506114850678");
            return Content(deger);
        }
            public IActionResult Getir()
        {
            //string UserId = "";VeriGuncelle
            string[] UserId = new string[3] { string.Empty, string.Empty, string.Empty };
            var databaseContext = _context.Personels.ToList();
            foreach (var item in databaseContext)
            {
                //UserId = Helperim.GetUserId(item.ScholarName);
                UserId = Helperim.GetUserId(item.User,true);
                var person = _context.Personels.FirstOrDefault(i => i.Id == item.Id);
                person.User = UserId[0];
                person.Alintilanma=Convert.ToInt32(!string.IsNullOrEmpty(UserId[1])? UserId[1]:0);
                _context.Update(person);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
            //var deger=doc.DocumentNode.SelectSingleNode("//div[@id='gsc_prf_in']");gs_ai_t

            ////çalışıyor
            // string value = doc.DocumentNode.SelectSingleNode("//span[@class='gs_hlt']").InnerText;


            //var query = $"//div[@id='gsc_prf_in']";

            //string tag = doc.GetElementbyId("gsc_prf_in").Name;
            //string href = doc.GetElementbyId("gsc_prf_in").GetAttributeValue("href", "");

            //gs_ai_t
            //string value = doc.DocumentNode.SelectSingleNode("//h3[@class = 'gs_ai_name']").InnerHtml;

            //foreach (var item in nodesWithARef)
            //{
            //   string value2= item.GetAttributeValue("href", "");
            //}

            //var link = nod.SelectNodes("//link[@href]").FirstOrDefault();
            //// you can also check if link is not null
            //var href = link.Attributes["href"].Value; // "anotherstyle7.css"

        }

    }
}
