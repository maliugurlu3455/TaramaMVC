using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics.Metrics;
using System.Net;
using System.Reflection;
using System.Security.Policy;
using System.Text;
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

       
        public async Task<IActionResult> AGuncelle(int? id) {
            YayinAlintiBilgisi yAB = null;
            var Pyb = await _context.PersonelYayinBilgileris.Where(r => r.Alinti > 0 && r.PersonelId==id).ToListAsync();
            if (Pyb != null && Pyb.Count > 0)
            {
                foreach (var pyb in Pyb)
                {

                    var liste = Helperim.AlintiGetir(pyb.BaslikCites);
                    if (liste != null && liste.Count > 0)
                    {
                        foreach (var item in liste)
                        {
                            yAB = null;
                            yAB = new YayinAlintiBilgisi();
                            yAB.YayinId = pyb.Id;
                            yAB.Ad = item;
                            await _context.YayinAlintiBilgisis.AddAsync(yAB);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // return Content(doc.Text, System.Net.Mime.MediaTypeNames.Text.Html, Encoding.GetEncoding("iso-8859-9"));
                    }

                }

            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Personeller/Delete/5 AGuncelle 
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
                var  silinecekler2 = await _context.YayinAlintiBilgisis.ToListAsync();

                _context.YayinAlintiBilgisis.RemoveRange(silinecekler2);

                _context.Personels.ExecuteUpdate(p => p.SetProperty(x => x.Alintilanma, x =>0 ));
                 
                await _context.SaveChangesAsync();
                mesaj = "Veriler Silindi.  ";
                mesaj += "<a href='/Personeller/Index' class='form-control btn btn-secondary'><svg class='bi d-block mx-auto mb-1' width='16' height='16'><use xlink:href='#back1' /></svg>Geri</a>";
            }
            catch (Exception ex)
            {

                mesaj = "Hata : "+ ex.Message;
            }
            
            return Content( mesaj, System.Net.Mime.MediaTypeNames.Text.Html);
        }
            public async Task<IActionResult> VeriGuncelle()
        {
            List<PersonelYayinBilgileri> UserList = new List<PersonelYayinBilgileri>();
            //var databaseContext = _context.Personels.Where(r=>r.Name.Contains("slam")).ToList();
            var databaseContext = _context.Personels.ToList();
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
        [HttpGet]
        public async Task<IActionResult> YayinAlinti()
        {
          

                string url = "https://scholar.google.com/scholar?oi=bibs&hl=tr&cites=4986682506114850678";
                            //https://scholar.google.com/scholar?oi=bibs&hl=tr&cites=1186450095630501573
            System.Threading.Thread.Sleep(Random.Shared.Next(100, 1100));
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = null;

            //web.PreRequest = delegate (HttpWebRequest webRequest)
            //{
            //    webRequest.CookieContainer = new CookieContainer();
            //    webRequest.CookieContainer.Add(new Cookie("SNID", "AC6x132FepUTnasiuMO3MNMv2o_x8sznbvdnESjNk8qd2P3XTG2_SliRpvegQnkZ2gCKQa2aaLedThyQmGpXV2mv0JRSEQ", "/", url));
            //    webRequest.CookieContainer.Add(new Cookie("NID", "511=f8lKQmPeUR9QMua8iL63YYL7dfIXn_Qb3KArSubhSmfpK1xmdYOzUemXHGZE12UeSNUu6bzvL4hlmPv7wwfgmYQY71AVC_WBDp0BXaKt6-3i6Yg8XZwWEje0IZLvRGDiHPbelNTyiareFBMn8tyF-F923exLE77iEVdu4KRXlJg", "/",url));
            //    webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.1587.41 Safari/537.36 Edg/110.0.864.52"; 
            //    webRequest.ContentType = "text/html; charset=UTF-8";
            //    return true;
            //};
            web.UseCookies = true;
            web.UserAgent= "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.1587.41 Safari/537.36 Edg/110.0.864.52";
           

            //doc = web.Load(url,"GET");
            doc = TaramaMVC.Helper.Helperim.GetPage(url);
            return Content(doc.Text, System.Net.Mime.MediaTypeNames.Text.Html, Encoding.GetEncoding("iso-8859-9"));
            }
            /*yayın guncelle*/
            public async Task<IActionResult> YayinAlintiGuncelle()
        {
            YayinAlintiBilgisi yAB = null;
            var Pyb = await _context.PersonelYayinBilgileris.Where(r => r.Alinti > 0).ToListAsync();
            if (Pyb != null && Pyb.Count > 0)
            {
                foreach (var pyb in Pyb)
                {

                    var liste = Helperim.AlintiGetir(pyb.BaslikCites);
                    if (liste != null && liste.Count > 0)
                    {
                        foreach (var item in liste)
                        {
                            yAB = null;
                            yAB = new YayinAlintiBilgisi();
                            yAB.YayinId = pyb.Id;
                            yAB.Ad = item;
                            await _context.YayinAlintiBilgisis.AddAsync(yAB);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                       // return Content(doc.Text, System.Net.Mime.MediaTypeNames.Text.Html, Encoding.GetEncoding("iso-8859-9"));
                    }

                }

            }
            return RedirectToAction("Index");
        }
        public IWebDriver driver= null;
        public IActionResult Test()
        {
            var options = new ChromeOptions();
            options.AddArgument("--no-sandbox");
            options.AddArgument("headless");
            options.AddArgument("ignore-certificate-errors");
            options.Proxy = new Proxy()
            {
                Kind = ProxyKind.Direct
            };
            WebRequest.DefaultWebProxy = null;
            HttpClient.DefaultProxy = new WebProxy()
            {
                BypassProxyOnLocal = true,
            };
            driver = new ChromeDriver(options);

            string url = "https://scholar.google.com/scholar?q=info:61wsb8aJxj4J:scholar.google.com/&output=cite&scirp=0&hl=tr"; //"https://scholar.google.com/scholar?oi=bibs&hl=tr&cites=4986682506114850678";
            //"https://serpapi.com/search.json?engine=google_scholar_cite&q=FDc6HiktlqEJ";
            driver.Navigate().GoToUrl(url);
            System.Threading.Thread.Sleep(15000);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(driver.PageSource);

            var asd = driver.FindElements(By.CssSelector("gs_or_cit gs_or_btn gs_nph"));
            foreach (var item in asd)
            {
                item.Click();
            }
            //var liste = Helperim.AlintiGetirSelenium();
            return RedirectToAction("Index");


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
           

        }

    }
}
