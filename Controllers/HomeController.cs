using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaramaMVC.Models;
namespace TaramaMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly DatabaseContext _context;

    
        public HomeController(ILogger<HomeController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[ResponseCache(Duration = 20, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult>  HepsiniGetir()
        {
            var users = await( from k in  _context.PersonelYayinBilgileris join 
                                    m in _context.Personels on k.PersonelId equals m.Id
                                    join  n in _context.AnaBilimDals on m.AnaBilimDallariId equals n.Id
                                    select new {AkademikPersonel=k.Personel.ScholarName, Alinti=k.Alinti,Yil=k.Yil,AnaBilimDali=n.Name,Baslik=k.Baslik,CitesAdeti=1 }).ToListAsync() ;
            
            return Json(users);
        }
        //HepsiniGetir

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}