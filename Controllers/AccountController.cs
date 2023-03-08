using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using TaramaMVC.Models;

namespace TaramaMVC.Controllers
{
   
        [Authorize]
        public class AccountController : Controller
        {
            private UserManager<AppUser> userManager;
            private SignInManager<AppUser> signInManager;

            public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signinMgr)
            {
                userManager = userMgr;
                signInManager = signinMgr;
            }

            [AllowAnonymous]
            public IActionResult Login(string returnUrl)
            {
                Login login = new Login();
                login.ReturnUrl = returnUrl;
                return View(login);
            }

            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Login(Login  login)
            {
                if (ModelState.IsValid)
                {
             
                    AppUser appUser = await userManager.FindByEmailAsync(login.Email);
                    if (appUser != null)
                    {
                        await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUser, login.Password, login.Remember, false);

                    if (result.Succeeded)
                    {

                        if (login.ReturnUrl != null)
                        {
                            return Redirect(login.ReturnUrl ?? "/");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                           
                    }
                    ModelState.AddModelError(nameof(login.Email), "Login Failed: Invalid Email or password");
                }
                return View(login);
            }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        //public  IActionResult Logout()
        //{
        //     signInManager.SignOutAsync();
        //    HttpContext.Session.Clear();
        //    return RedirectToAction("Index", "Home");
        //}
        public IActionResult Index()
        {
            return View();
        }
    }
}
