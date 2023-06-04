using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using System.Diagnostics;
using TaramaMVC.Models;
namespace TaramaMVC.Controllers
{
   
    public class HomeController : Controller
    {
        private UserManager<AppUser> userManager;
      
        private readonly ILogger<HomeController> _logger;

        private readonly DatabaseContext _context;

     
        public HomeController(ILogger<HomeController> logger, DatabaseContext context, UserManager<AppUser> userMgr)
        {
            _logger = logger;
            _context = context;
            userManager = userMgr;
        }
        public  IActionResult Index()
        {
            return View();
        }

        public IActionResult Test()
        {

            return View();
        }
        public IActionResult Raporlar()
        {
            return View();
        }
        //[ResponseCache(Duration = 20, Location = ResponseCacheLocation.None, NoStore = true)] 
        [HttpGet]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public async Task<JsonResult>  HepsiniGetir()
        {
            var users =await (from k in  _context.PersonelYayinBilgileris join 
                                    m in _context.Personels on k.PersonelId equals m.Id
                                    join  n in _context.AnaBilimDals on m.AnaBilimDallariId equals n.Id
                                    select new 
                                    {
                                        AkademikPersonel=k.Personel.ScholarName,Ad=m.Name,Soyad=m.SurName, Alinti = k.Alinti, h_endex = m.h_endex, i10_endex = m.i10_endex, Yil=k.Yil
                                        ,AnaBilimDal=n.Name,Baslik=k.Baslik,CitesAdeti=1, BaslikCites=k.BaslikCites 
                                    }).GroupBy(x => new { x.AnaBilimDal,x.AkademikPersonel,x.Ad,x.Soyad,x.Yil ,x.Baslik,x.BaslikCites})
                                .Select(group => new
                                {
                                    AnaBilimDali=group.Key.AnaBilimDal,
                                    AcademicPersonel = group.Key.AkademikPersonel,
                                    Ad=group.Key.Ad,
                                    Soyad=group.Key.Soyad,
                                    Yil= group.Key.Yil,
                                    Baslik=group.Key.Baslik,
                                    BaslikCites = group.Key.BaslikCites,
                                    Alinti = group.Sum(x => x.Alinti),
                                    CitesAdeti = group.Sum(x => x.CitesAdeti),
                                    h_endex = group.Max(x => x.h_endex),
                                    i10_endex = group.Max(x => x.i10_endex),
                                }).ToArrayAsync();

            return Json(users);
        }
        [HttpGet]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)] // 30 dk boyunca cache den oku
        public async Task<JsonResult> ChartHepsiniGetir()
        {
            var academicData = await (from k in _context.PersonelYayinBilgileris
                                      join m in _context.Personels on k.PersonelId equals m.Id
                                      join n in _context.AnaBilimDals on m.AnaBilimDallariId equals n.Id
                                      select new { AkademikPersonel = k.Personel.ScholarName, Alinti = k.Alinti, h_endex = m.h_endex, i10_endex = m.i10_endex, Yil = k.Yil, AnaBilimDali = n.Name, Baslik = k.Baslik, CitesAdeti = 1, BaslikCites = k.BaslikCites }).ToListAsync();

            var result = academicData.GroupBy(x => x.AkademikPersonel)
                                    .Select(group => new
                                    {
                                        AcademicPersonel = group.Key,
                                        TotalAlinti = group.Sum(x => x.Alinti),
                                        TotalCitesAdeti = group.Sum(x => x.CitesAdeti),
                                        h_endex = group.Average(x => x.h_endex),
                                        i10_endex = group.Average(x => x.i10_endex),
                                    });
           
            var labels = result.Select(x => x.AcademicPersonel).ToArray();
            var alintiData = result.Select(x => x.TotalAlinti).ToArray();
            var quoteData = result.Select(x => x.TotalCitesAdeti).ToArray();
            var _h_endex = result.Select(x => x.h_endex).ToArray();
            var _i10_endex = result.Select(x => x.i10_endex).ToArray();
            return Json(new { labels, alintiData, quoteData, _h_endex, _i10_endex });

        }
        //HepsiniGetir

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
      
    }
}