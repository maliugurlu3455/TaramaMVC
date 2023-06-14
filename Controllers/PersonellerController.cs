using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SerpApi;
using System.Collections;
using System.Diagnostics;
using System.Text;
using TaramaMVC.Helper;
using TaramaMVC.Models;

namespace TaramaMVC.Controllers
{
    //[Authorize]
    public class PersonellerController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        //private static IWebDriver _driver=null;
        public PersonellerController(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        // GET: Personeller
        ////[Authorize]
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Personels.OrderByDescending(t => t.Alintilanma).Include(p => p.AnaBilimDallari);

            return View(await databaseContext.ToListAsync());
        }
        public async Task<IActionResult> FindIndex()
        {
            var databaseContext = _context.Personels.OrderByDescending(t => t.Alintilanma).Include(p => p.AnaBilimDallari);

            return View(await databaseContext.ToListAsync());
        }

        // GET: Personeller/Details/5
        ////[Authorize]
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
        ////[Authorize]
        public IActionResult Create()
        {
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "Name");
            ViewData["AnaBilimDallariId"] = new SelectList(_context.AnaBilimDals, "Id", "Name");
            return View();
        }

        // POST: Personeller/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        ////[Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,SurName,AnaBilimDallariId,ScholarName,User")] Personel personel)
        {
            try
            {
                personel.User = personel.Name + " " + personel.SurName;
            }
            catch (Exception ex)
            {

                personel.User = "yok";
            }

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
        ////[Authorize]
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
            personel.AnaBilimDallari = _context.AnaBilimDals.Find(personel.AnaBilimDallariId);
            ViewData["AnaBilimDallariId"] = new SelectList(_context.AnaBilimDals, "Id", "Name", personel.AnaBilimDallariId);
            return View(personel);
            //return RedirectToAction(nameof(Index));
        }

        // POST: Personeller/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        ////[Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SurName,AnaBilimDallariId,ScholarName,User")] Personel personel)
        {
            if (id != personel.Id)
            {
                return NotFound();
            }
            if (personel.AnaBilimDallariId > 0)
            {
                var anabilims = await _context.AnaBilimDals.FindAsync(personel.AnaBilimDallariId);
                personel.AnaBilimDallari = anabilims;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
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

            }
            ViewData["AnaBilimDallariId"] = new SelectList(_context.AnaBilimDals, "Id", "Name", personel.AnaBilimDallariId);
            return View(personel);
        }

        //alıntı güncelle
        ////[Authorize]
        public async Task<IActionResult> AGuncelle(int? id)
        {


            YayinAlintiBilgisi yAB = null;
            /**/
            HtmlDocument doc = null;
            /**/
            var Pyb = await _context.PersonelYayinBilgileris.Where(r => r.Alinti > 0 && r.PersonelId == id).ToListAsync();
            if (Pyb != null && Pyb.Count > 0)
            {
                foreach (var pyb in Pyb)
                {
                    //Alinti Getir

                    HtmlWeb web = new HtmlWeb();
                    web.UseCookies = true;
                    web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";

                    //doc = web.Load(pyb.BaslikCites, "get");
                    //TaramaMVC.Helper.Helperim.GetPage(pyb.BaslikCites);
                    doc = TaramaMVC.Helper.Helperim.GetPage(pyb.BaslikCites);
                    List<YayinAlintiBilgisi> liste = new List<YayinAlintiBilgisi>();


                    // var liste = Helperim.AlintiGetir(pyb.BaslikCites);//, ylo,yhi);
                    if (liste != null && liste.Count > 0)
                    {
                        foreach (var item in liste)
                        {
                            yAB = null;
                            yAB = new YayinAlintiBilgisi();
                            yAB.personelYayinBilgileriId = pyb.Id;
                            //yAB.Ad = item;
                            await _context.YayinAlintiBilgisis.AddAsync(yAB);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // return Content(doc.Text, System.Net.Mime.MediaTypeNames.Text.Html, Encoding.GetEncoding("iso-8859-9"));
                    }

                }
                return Content(doc.Text, System.Net.Mime.MediaTypeNames.Text.Html, Encoding.GetEncoding("iso-8859-9"));

            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Personeller/Delete/5 AGuncelle 
        ////[Authorize]
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
            // ViewData["AnaBilimDallariId"] = new SelectList(_context.AnaBilimDals, "Id", "Name", personel.AnaBilimDallariId);
            return View(personel);
        }

        // POST: Personeller/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        ////[Authorize]
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
        ////[Authorize] AlintiVerileriSil
        public async Task<IActionResult> AlintiVerileriSil()
        {
            string mesaj = "";
            try
            {

                var silinecekler2 = await _context.YayinAlintiBilgisis.ToListAsync();

                _context.YayinAlintiBilgisis.RemoveRange(silinecekler2);

                await _context.SaveChangesAsync();
                MessageModel mm = new MessageModel();
                mm.Durum = true;
                mm.Baslik = "Alıntı Verileri Silme İşlemi";
                mm.Acıklama = "Alıntı Verileri Silindi. İşlem tamam.";
                return View("Message", mm);
            }
            catch (Exception ex)
            {

                mesaj = "Hata : " + ex.Message;
            }

            return Content(mesaj, System.Net.Mime.MediaTypeNames.Text.Html);
        }
        public async Task<IActionResult> VerileriSil()
        {
            string mesaj = "";
            try
            {
                var silinecekler = await _context.PersonelYayinBilgileris.ToListAsync();
                _context.PersonelYayinBilgileris.RemoveRange(silinecekler);
                var silinecekler2 = await _context.YayinAlintiBilgisis.ToListAsync();

                _context.YayinAlintiBilgisis.RemoveRange(silinecekler2);

                _context.Personels.ExecuteUpdate(p => p.SetProperty(x => x.Alintilanma, x => 0));
                _context.Personels.ExecuteUpdate(p => p.SetProperty(x => x.i10_endex, x => 0));
                _context.Personels.ExecuteUpdate(p => p.SetProperty(x => x.h_endex, x => 0));
                await _context.SaveChangesAsync();
                MessageModel mm = new MessageModel();
                mm.Durum = true;
                mm.Baslik = "Makale ve Alıntı Verilerini Silme İşlemi";
                mm.Acıklama = "Makale ve Alıntı Verileri başarıyla silindi. İşlem tamam.";
                return View("Message", mm);
            }
            catch (Exception ex)
            {

                mesaj = "Hata : " + ex.Message;
                return Content(mesaj, System.Net.Mime.MediaTypeNames.Text.Html);
            }


        }
        public async Task<IActionResult> VeriGuncelle()
        {
            List<PersonelYayinBilgileri>? UserList = new List<PersonelYayinBilgileri>();
            //var databaseContext = _context.Personels.Where(r=>r.Name.Contains("slam")).ToList();
            var databaseContext = _context.Personels.ToList();
            foreach (var item in databaseContext)
            {
                //web servisten kullanıcı bilgilerini al
                TempModel temptble = Helperim.KullaniciBilgileri(item.User);
                UserList = temptble.personelYayinBilgileri;
                var person = await _context.Personels.FirstOrDefaultAsync(i => i.Id == item.Id);
                if (UserList != null && UserList.Count > 0)
                {
                    person.h_endex = Convert.ToInt32(temptble.h_endex);
                    person.i10_endex = Convert.ToInt32(temptble.i10_endex);
                     
                    foreach (PersonelYayinBilgileri py in UserList)
                    {
                        person.Alintilanma = py.Personel.Alintilanma;
                        py.Personel = person;
                        py.PersonelId = person.Id;
                        py.UpdateDate = DateTime.Today;

                        await _context.PersonelYayinBilgileris.AddAsync(py);
                        _context.Personels.Update(person);
                    }
                    _context.Personels.Update(person);

                    await _context.SaveChangesAsync();
                }


            }

            return RedirectToAction("Index");
        }
        /*
         AlintiGuncelleApi
         */

        public async Task<IActionResult> AlintiGuncelleApi(int id)
        {

            #region CiteID
            YayinAlintiBilgisi yAB = null;
            string apikey = "";
            var Pyb = await _context.PersonelYayinBilgileris.Where(r => r.Alinti > 0 && r.PersonelId == id).ToListAsync();
            if (Pyb != null && Pyb.Count > 0)
            {
                foreach (var pyb in Pyb)
                {
                    string key = GetApiKeyString();

                    if (key != "")
                    {

                        Hashtable ht = new Hashtable();
                        var degerler = _context.Parametrelers.Where(r => r.GrupId == 1);
                        if (degerler != null)
                        {
                            foreach (var item in degerler)
                            {
                                ht.Add(item.Name, item.Value);
                            }
                        }
                        //ht.Add("engine", "google_scholar");//   ht.Add("engine", "google_scholar_cite");
                        //ht.Add("hl", "tr");
                        //ht.Add("num", "20");
                        //ht.Add("as_ylo", string.Format("{0}", (DateTime.Now.Year ))); // 1 yıl öncesinden itibaren gösteriyor.
                        ht.Add("cites", TaramaMVC.Helper.Helperim.UrltoCites(pyb.BaslikCites));

                        try
                        {
                            GoogleSearch search = new GoogleSearch(ht, key);
                            SetApiKey(key);//key i 1 arttır -- kullan

                            JObject data = search.GetJson();
                            string ifade = "";
                            SearchResult res = null;
                            if (search != null)
                            {
                                try
                                {
                                    res = JsonConvert.DeserializeObject<SearchResult>(data.ToString(), Helperim.settings);
                                    if (res != null)
                                    {
                                        if (res.search_metadata.status == "Success")
                                        {
                                            YayinAlintiBilgisi yab = null;
                                            SearchResultDetail detay = null;
                                            foreach (OrganicResult item in res.organic_results)
                                            {
                                                yab = new YayinAlintiBilgisi();
                                                yab.Title = item.title;
                                                yab.Link = item.link;
                                                yab.SID = item.result_id;
                                                yab.PublicationInfo = item.publication_info.summary;
                                                yab.Snippet = item.snippet;
                                                yab.personelYayinBilgileriId = pyb.Id;
                                                yab.personelYayinBilgileri = pyb;
                                                yab.status = 0;
                                                try
                                                {
                                                    #region Detay
                                                    key = GetApiKeyString();
                                                    detay = GetSearchResultDeatailCitedID(key, yab.SID);

                                                    if (detay != null)
                                                    {
                                                        if (detay.search_metadata.status == "Success")
                                                        {
                                                            if (detay.citations.Count > 0)
                                                            {

                                                                foreach (citations citations_ in detay.citations)
                                                                {
                                                                    if (citations_.title == "APA")
                                                                    {
                                                                        yab.Ad = citations_.snippet;
                                                                        yab.Tip = citations_.title;
                                                                        yab.status = 1;
                                                                    }
                                                                }


                                                            }

                                                        }

                                                    }
                                                    #endregion
                                                }
                                                catch (Exception ex)
                                                {
                                                    yab.Tip = "APA_";
                                                    yab.status = 0;
                                                    Trace.WriteLine("hata Alıntı detay : " + ex.Message);
                                                }



                                                await _context.YayinAlintiBilgisis.AddAsync(yab);
                                                await _context.SaveChangesAsync();

                                                //item.title
                                                //item.snippet
                                                //item.position
                                                //item.inline_links
                                                //item.link
                                                //item.publication_info
                                                //item.result_id

                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                    Trace.WriteLine("hata Alıntı : " + ex.Message);
                                }

                            }



                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine("hata genel : " + ex.Message);

                        }
                    }

                }

            }
            #endregion
            return RedirectToAction("Index");
        }
        public SearchResultDetail GetSearchResultDeatailCitedID(string ApiKey, string CiteID)
        {
            YayinAlintiBilgisi liste = new YayinAlintiBilgisi();
            SearchResultDetail res = null;
            String apiKey = ApiKey;//"1f9146a88abddffbb064fbc4e60a22b0f85f0e59068b5e4275b4e454c598c333";
            Hashtable ht = new Hashtable();
            var degerler = _context.Parametrelers.Where(r => r.GrupId == 2);
            if (degerler != null)
            {
                foreach (var item in degerler)
                {
                    ht.Add(item.Name, item.Value);
                }
            }
            //ht.Add("engine", "google_scholar_cite");
            ht.Add("q", CiteID);// "LSsXyncAAAAJ");

            try
            {
                GoogleSearch search = new GoogleSearch(ht, apiKey);
                JObject data = search.GetJson();

                res = JsonConvert.DeserializeObject<SearchResultDetail>(data.ToString(), Helperim.settings);
                SetApiKey(apiKey);

            }
            catch (SerpApiSearchException ex)
            {

            }
            return res;
        }

        [HttpGet]
        public async Task<IActionResult> YayinAlinti()
        {


            string url = "https://scholar.google.com/scholar?oi=bibs&hl=tr&cites=4986682506114850678";
            //https://scholar.google.com/scholar?oi=bibs&hl=tr&cites=1186450095630501573
            System.Threading.Thread.Sleep(Random.Shared.Next(10000, 11000));
            HtmlWeb web = new HtmlWeb();
            //web.UseCookies = true;
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
            web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";


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
                    //
                    // string ylo = ConfigurationManager.AppSetting["ylo"];
                    // string yhi = ConfigurationManager.AppSetting["yhi"];
                    var liste = Helperim.AlintiGetir(pyb.BaslikCites);//,ylo,yhi);
                    if (liste != null && liste.Count > 0)
                    {
                        foreach (var item in liste)
                        {
                            yAB = null;
                            yAB = new YayinAlintiBilgisi();
                            yAB.personelYayinBilgileriId = pyb.Id;
                            yAB.Ad = item;
                            yAB.Tip = "APA";
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

        public IActionResult Getir()
        {
            //string UserId = "";VeriGuncelle
            string[] UserId = new string[3] { string.Empty, string.Empty, string.Empty };
            var databaseContext = _context.Personels.ToList();
            foreach (var item in databaseContext)
            {
                //UserId = Helperim.GetUserId(item.ScholarName);
                UserId = Helperim.GetUserId(item.User, true);
                var person = _context.Personels.FirstOrDefault(i => i.Id == item.Id);
                person.User = UserId[0];
                person.Alintilanma = Convert.ToInt32(!string.IsNullOrEmpty(UserId[1]) ? UserId[1] : 0);
                _context.Update(person);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");


        }
        //public async Task<IActionResult> AlintiGuncelleYeni(int? id)
        //{

        //    List<YayinAlintiBilgisi> liste = new List<YayinAlintiBilgisi>();
        //    YayinAlintiBilgisi yab = null;
        //    string urll = "";
        //    /**/
        //    HtmlDocument doc = null;
        //    /*SNID*/
        //    var options = new ChromeOptions();
        //    options.AddArgument("--disable-blink-features=AutomationControlled");
        //    options.AddArgument("headless");
        //    //options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36");
        //    options.AddArgument("--no-sandbox");
        //    options.AddArgument("headless");
        //    options.AddArgument("ignore-certificate-errors");
        //    options.AddArgument("start-maximized");
        //    options.AddArgument("--allow-insecure-localhost");

        //    options.Proxy = new Proxy()
        //    {
        //        Kind = ProxyKind.Direct
        //    };
        //    _driver = new ChromeDriver(options);
        //    DateTime dt1 = DateTime.Parse("2023-09-29T07:37:23.748Z", CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal);
        //    DateTime dt2 = DateTime.Parse("2024-05-03T07:37:22.724Z", CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal);
        //    DateTime dt3 = DateTime.Parse("2023-09-29T07:44:21.711Z", CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal);

        //    _driver.Manage().Cookies.AddCookie(new Cookie("NID", "511=PQTeU4yiJsbxZDHaR13vBP5p9Yl0LlzQw_9X9jEPF0oPUUKNXczVLX8eK_Nl3dFjLyM9XEh-IZi58yoebI3NDrxO2DjlozN_zSfx3_Pie1hcvuNNc9gHrrcfVCZJqCZN0VcwurocJFzSyddVRp8_mT2IfZzegfJAD94Uk44VC94", ".google.com", "/", dt1));
        //    _driver.Manage().Cookies.AddCookie(new Cookie("GSP", "A=hYEC8Q:CPTS=1680161841:LM=1680161841:S=dGHvNiJF3wzrDz5B", ".scholar.google.com", "/", dt2));
        //    _driver.Manage().Cookies.AddCookie(new Cookie("SNID", "ANHfhbPdsJjNHBPcTgvA1_lmwRcFPS1RCe3JjmfPSDlnj0Z_OFqPhM0sWWuKo8aHF2b7mYNf__uYGGn2CSJJY9VWhcpu", ".google.com", "/verify", dt3));

        //    var Pyb = await _context.PersonelYayinBilgileris.Where(r => r.Alinti > 0 && r.PersonelId == id).ToListAsync();
        //    if (Pyb != null && Pyb.Count > 0)
        //    {
        //        foreach (var pyb in Pyb)
        //        {
        //            // https://scholar.google.com/scholar?start=20&hl=tr&oe=Latin5&as_sdt=1990&as_ylo=1900&as_yhi=2025&cites=1596913064683209329&scipsc=
        //            // https://scholar.google.com/scholar?oi=bibs&hl=tr&oe=Latin5&cites=1596913064683209329

        //            for (int t = 0; t < pyb.Alinti;t+= 10)
        //            {

        //                System.Threading.Thread.Sleep(Random.Shared.Next(5100, 7500));
        //                //doc = await TaramaMVC.Helper.Helperim.GetPageYeni(pyb.BaslikCites+ "&start="+t.ToString());

        //                // driver.SwitchTo().NewWindow(WindowType.Window);

        //                string url = "";
        //                if (t==0)
        //                {
        //                    url = pyb.BaslikCites + "&start=" + t.ToString();
        //                }
        //                else
        //                {
        //                    url = pyb.BaslikCites + "&start=" + t.ToString() + "&as_sdt=2005&sciodt=0,5&scipsc=";
        //                }

        //                _driver.Navigate().GoToUrl(url);

        //                doc = new HtmlDocument();
        //                doc.LoadHtml(_driver.PageSource);

        //                var nods = doc.DocumentNode.SelectNodes("//div[@class = 'gs_r gs_or gs_scl']");
        //            if (nods != null && nods.Count > 0)
        //            { 

        //                for (int i = 0; i < nods.Count; i++)
        //                {
        //                    yab = new YayinAlintiBilgisi();
        //                        string sId = "";
        //                        string gs_rs = "";
        //                        string gs_a = "";
        //                        string gs_ri = "";
        //                        string gs_ri1 = "";
        //                        string gs_ri2 = "";
        //                        string gs_ri3 = "";
        //                        //System.Threading.Thread.Sleep(Random.Shared.Next(2000, 3100)); //div[@id='gs_cit_list_wrp']//h3[@class='gs_rt']/a
        //                        try
        //                        {
        //                            if (string.IsNullOrEmpty(nods[i].SelectNodes("//div[@class='gs_r gs_or gs_scl']")[i].GetAttributeValue("data-cid", "")))
        //                            {
        //                                sId = "";
        //                            }
        //                            else
        //                            {
        //                                sId = nods[i].SelectNodes("//div[@class='gs_r gs_or gs_scl']")[i].GetAttributeValue("data-cid", "");//id
        //                            }

        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            sId = "";

        //                        }
        //                        try
        //                        {
        //                            if (string.IsNullOrEmpty(nods[i].SelectNodes("//div[@class='gs_rs']")[i].InnerText))
        //                            {
        //                                gs_rs = "";
        //                            }
        //                            else
        //                            {
        //                                gs_rs = nods[i].SelectNodes("//div[@class='gs_rs']")[i].InnerText;// snippet
        //                            }


        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            gs_rs = "";

        //                        }
        //                        try
        //                        {
        //                            if (string.IsNullOrEmpty(nods[i].SelectNodes("//div[@class='gs_a']")[i].InnerText))
        //                            {
        //                                gs_a = "";
        //                            }
        //                            else
        //                            {
        //                                gs_a = nods[i].SelectNodes("//div[@class='gs_a']")[i].InnerText;//publication info
        //                            }

        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            gs_a = "";

        //                        }
        //                        try
        //                        {
        //                            if (string.IsNullOrEmpty(nods[i].SelectNodes("//div[@class='gs_ri']")[i].InnerText))
        //                            {
        //                                gs_ri = "";
        //                            }
        //                            else
        //                            {
        //                                gs_ri = nods[i].SelectNodes("//div[@class='gs_ri']")[i].InnerText;//tamamı
        //                            }

        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            gs_ri = "";

        //                        }
        //                        try
        //                        {
        //                            if (string.IsNullOrEmpty(nods[i].SelectNodes("//div[@class='gs_ri']//h3[@class='gs_rt']")[i].InnerText))
        //                            {
        //                                gs_ri1 = "";
        //                            }
        //                            else
        //                            {
        //                                gs_ri1 = nods[i].SelectNodes("//div[@class='gs_ri']//h3[@class='gs_rt']")[i].InnerText;//title
        //                            }

        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            gs_ri1 = "";

        //                        }
        //                        try
        //                        {
        //                            if (string.IsNullOrEmpty(nods[i].SelectNodes("//div[@class='gs_ri']//h3[@class='gs_rt']//a")[i].GetAttributeValue("href", "")))
        //                            {
        //                                gs_ri2 = "";
        //                            }
        //                            else
        //                            {
        //                                gs_ri2 = nods[i].SelectNodes("//div[@class='gs_ri']//h3[@class='gs_rt']//a")[i].GetAttributeValue("href", "");//link
        //                            }

        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            gs_ri2 = "";

        //                        }
        //                        try
        //                        {
        //                            if (string.IsNullOrEmpty(nods[i].SelectNodes("//div[@class='gs_ri']//h3[@class='gs_rt']//a")[i].InnerText))
        //                            {
        //                                gs_ri3 = "";
        //                            }
        //                            else
        //                            {
        //                                gs_ri3 = nods[i].SelectNodes("//div[@class='gs_ri']//h3[@class='gs_rt']//a")[i].InnerText;//link text
        //                            }

        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            gs_ri3 = "";

        //                        }

        //                    yab.Title = gs_ri1;
        //                    yab.Link = gs_ri2;
        //                    yab.SID = sId;
        //                    yab.PublicationInfo = gs_a;
        //                    yab.Snippet = gs_rs;
        //                    yab.personelYayinBilgileriId = pyb.Id;
        //                    yab.personelYayinBilgileri = pyb;
        //                    yab.Ad = gs_ri1;
        //                        yab.Tip = "APA";
        //                        yab.status = 0;
        //                    await _context.YayinAlintiBilgisis.AddAsync(yab);
        //                        await _context.SaveChangesAsync();
        //                    }

        //            }

        //            }
        //        }

        //        // return Content(doc.Text, System.Net.Mime.MediaTypeNames.Text.Html, Encoding.GetEncoding("iso-8859-9"));

        //    }
        //    return RedirectToAction(nameof(Index));
        //}
        //public static string GetUserAgent()
        // {
        //     // HttpContextAccessor kullanarak HttpContext.Current.Request.UserAgent gibi bir erişim sağlayabilirsiniz
        //     string userAgent = HttpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();

        //     return userAgent;
        // }

        // public static string GetAcceptHeader()
        // {
        //     // HttpContextAccessor kullanarak HttpContext.Current.Request.Headers["Accept"] gibi bir erişim sağlayabilirsiniz
        //     string acceptHeader =string.IsNullOrEmpty(HttpContextAccessor.HttpContext.Request.Headers["Accept"].ToString())?"": HttpContextAccessor.HttpContext.Request.Headers["Accept"].ToString();

        //     return acceptHeader;
        // }
        public async Task<IActionResult> AlintiGuncelleYeniEski(int? id)
        {

            List<YayinAlintiBilgisi> liste = new List<YayinAlintiBilgisi>();
            YayinAlintiBilgisi yab = null;
            string urll = "";
            string url = "";
            /**/
            HtmlDocument doc = null;int sayac1 = 0;
            /**/
            var Pyb = await _context.PersonelYayinBilgileris.Where(r => r.Alinti > 0 && r.PersonelId == id).ToListAsync();
            if (Pyb != null && Pyb.Count > 0)
            {
                foreach (var pyb in Pyb)
                {
                    sayac1++;
                    //https://scholar.google.com/scholar?as_ylo=2019&hl=tr&as_sdt=2005&sciodt=0,5&cites=1231811585781593305&scipsc=
                    //link
                    //https://scholar.google.com/scholar?hl=tr&as_sdt=2005&sciodt=0%2C5&cites=4986682506114850678&scipsc=&as_ylo=2019&as_yhi=2023


                        System.Threading.Thread.Sleep(Random.Shared.Next(20001 + sayac1));
                        //doc = await TaramaMVC.Helper.Helperim.GetPageYeni(pyb.BaslikCites + "&as_ylo=2022&start=" + t.ToString());
                        urll = TaramaMVC.Helper.Helperim.UrltoCites(pyb.BaslikCites);
                        url = string.Format("https://scholar.google.com/scholar?hl=tr&as_sdt=2005&sciodt=0%2C5&cites={0}&scipsc=&as_ylo={1}", urll, 2023);
                        //url = string.Format("https://scholar.google.com/scholar?as_ylo={1}&hl=tr&as_sdt=2005&sciodt=0,5&cites={0}&scipsc=", urll, t);
                        doc = TaramaMVC.Helper.Helperim.GetPage(url);


                        var nods = doc.DocumentNode.SelectNodes("//div[@class = 'gs_r gs_or gs_scl']");

                        if (nods != null && nods.Count > 0)
                        {

                            for (int i = 0; i < nods.Count; i++)
                            {
                                yab = new YayinAlintiBilgisi();
                                string sId = "";
                                string gs_rs = "";
                                string gs_a = "";
                                string gs_ri = "";
                                string gs_ri1 = "";
                                string gs_ri2 = "";
                                string gs_ri3 = "";
                                //System.Threading.Thread.Sleep(Random.Shared.Next(2000, 3100)); //div[@id='gs_cit_list_wrp']//h3[@class='gs_rt']/a
                                try
                                {
                                    sId = nods[i].SelectNodes("//div[@class='gs_r gs_or gs_scl']")[i].GetAttributeValue("data-cid", "");//id
                                }
                                catch (Exception ex)
                                {
                                    sId = "";

                                }
                                try
                                {
                                    gs_rs = nods[i].SelectNodes("//div[@class='gs_rs']")[i].InnerText;// snippet
                                }
                                catch (Exception ex)
                                {
                                    gs_rs = "";

                                }
                                try
                                {
                                    gs_a = nods[i].SelectNodes("//div[@class='gs_a']")[i].InnerText;//publication info
                                }
                                catch (Exception ex)
                                {
                                    gs_a = "";

                                }
                                try
                                {
                                    gs_ri = nods[i].SelectNodes("//div[@class='gs_ri']")[i].InnerText;//tamamı
                                }
                                catch (Exception ex)
                                {
                                    gs_ri = "";

                                }
                                try
                                {
                                    gs_ri1 = nods[i].SelectNodes("//div[@class='gs_ri']//h3[@class='gs_rt']")[i].InnerText;//title
                                }
                                catch (Exception ex)
                                {
                                    gs_ri1 = "";

                                }
                                try
                                {
                                    gs_ri2 = nods[i].SelectNodes("//div[@class='gs_ri']//h3[@class='gs_rt']//a")[i].GetAttributeValue("href", "");//link
                                }
                                catch (Exception ex)
                                {
                                    gs_ri2 = "";

                                }
                                try
                                {
                                    gs_ri3 = nods[i].SelectNodes("//div[@class='gs_ri']//h3[@class='gs_rt']//a")[i].InnerText;//link text
                                }
                                catch (Exception ex)
                                {
                                    gs_ri3 = "";

                                }

                                yab.Title = gs_ri1;
                                yab.Link = gs_ri2;
                                yab.SID = sId;
                                yab.PublicationInfo = gs_a;
                                yab.Snippet = gs_rs;
                                yab.personelYayinBilgileriId = pyb.Id;
                                yab.personelYayinBilgileri = pyb;
                                yab.Ad = gs_ri;
                                yab.Tip = "APA";

                                await _context.YayinAlintiBilgisis.AddAsync(yab);
                                await _context.SaveChangesAsync();
                                /*
                                 https://scholar.google.com/scholar?q=info:{0}:scholar.google.com/&output=cite&scirp=2&hl=tr
                                */
                            }

                        }

                    
                }
                //await _context.SaveChangesAsync();
                // return Content(doc.Text, System.Net.Mime.MediaTypeNames.Text.Html, Encoding.GetEncoding("iso-8859-9"));
            }
            return RedirectToAction(nameof(Index));
        }
        public string GetApiKeyString()
        {
            string apiKey = "";
            try
            {
                var deger = _context.ApiKey.FirstOrDefault(r => r.IsTamam == false && r.Sayi < 100);
                if (deger != null && deger.Key != "" && deger.Key != null && deger.Sayi < 100)
                {

                    apiKey = deger.Key;


                }
            }
            catch (Exception ex)
            {

                apiKey = "";
            }

            return apiKey;
        }
        public ApiKeys GetApiKey()
        {
            ApiKeys apiKey = null;
            try
            {
                var deger = _context.ApiKey.FirstOrDefault(r => r.IsTamam == false && r.Sayi < 100);
                if (deger != null && deger.Key != "" && deger.Key != null && deger.Sayi < 100)
                {
                    apiKey = new ApiKeys();
                    apiKey.Key = deger.Key;
                    apiKey.Id = deger.Id;
                    apiKey.Sayi = deger.Sayi;
                    apiKey.IsTamam = deger.IsTamam;

                }
            }
            catch (Exception ex)
            {

                apiKey = null;
            }

            return apiKey;
        }
        public bool SetApiKey(string ApiKey)
        {
            string apiKey = ApiKey;
            bool sonuc = false;

            try
            {
                var deger = _context.ApiKey.SingleOrDefault(r => r.Key == apiKey);
                if (deger != null && deger.Key != "" && deger.Key != null)
                {
                    if (deger.Sayi == 99)
                    {
                        deger.Sayi++;
                        deger.IsTamam = true;
                        _context.ApiKey.Update(deger);
                        _context.SaveChanges();
                        sonuc = true;
                    }
                    else
                    {
                        deger.Sayi++;
                        _context.ApiKey.Update(deger);
                        _context.SaveChanges();
                        sonuc = true;
                    }

                }
                else
                {
                    sonuc = false;
                }
            }
            catch (Exception ex)
            {

                sonuc = false;
            }

            return sonuc;
        }
    }
}
