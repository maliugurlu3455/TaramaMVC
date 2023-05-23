using GoogleApi.Entities.Search.Video.Common;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TaramaMVC.Models;

namespace TaramaMVC.Controllers
{
  
    public class YonetimController : Controller
    {
        private UserManager<AppUser> userManager;
        private IPasswordHasher<AppUser> passwordHasher;

        public YonetimController(UserManager<AppUser> usrMgr, IPasswordHasher<AppUser> passwordHash)
        {
            userManager = usrMgr;
            passwordHasher = passwordHash;
        }

        public IActionResult Index()
        {
            return View(userManager.Users);
        }
      
        public  IActionResult CreateSearchUser(string UserName)
        {
            HtmlDocument doc = null;

            string[] UserID = new string[] { string.Empty, string.Empty, string.Empty };
            try
            {
                HtmlWeb web = new HtmlWeb();
                var url = new Uri(" https://scholar.google.com/citations?view_op=search_authors&mauthors=" + UserName + "&hl=tr&oi=ao");
                

                doc = web.Load(url);
                var nod = doc.DocumentNode.SelectNodes("//h3[@class = 'gs_ai_t']");
                if (nod != null)
                {
                    int a = 0;
                    foreach (var item in nod)
                    {
                        UserID[a] = item.InnerText;
                        a++;
                    }

                    return Content(doc.Text, System.Net.Mime.MediaTypeNames.Text.Html, Encoding.GetEncoding("iso-8859-9"));
                }

            }
            catch
            {
                // UserID = new string[] { string.Empty, string.Empty, string.Empty };
            }
            return Content("");
        }
     
        public ViewResult Create() => View();

        [HttpPost]
        ////[Authorize]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    UserName = user.Name,
                    Email = user.Email
                };

                IdentityResult result = await userManager.CreateAsync(appUser, user.Password);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }
        ////[Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }
      
        [HttpPost]
        ////[Authorize]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(email))
                    user.Email = email;
                else
                    ModelState.AddModelError("", "Email cannot be empty");

                if (!string.IsNullOrEmpty(password))
                    user.PasswordHash = passwordHasher.HashPassword(user, password);
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "Kullanıcı Bulunamadı");
            return View(user);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AppUser usr)
        {
            try
            {
                AppUser user = await userManager.FindByIdAsync(usr.Id);
                if (user != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(user);
                    
                    
                    
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
                else
                    ModelState.AddModelError("", "User Not Found");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("Genel hata :", ex.Message);
            }
           
            return View("Index", userManager.Users);
        }


    }
}
