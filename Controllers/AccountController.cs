using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaramaMVC.Models;

namespace TaramaMVC.Controllers
{

    // ialtin@ogu.edu.tr 
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
            public IActionResult Login()
            {
                Login login = new Login();
                
                return View(login);
            }

            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Login(Login  login, string? returnUrl="")
            {
                if (ModelState.IsValid)
                {
             
                    var appUser = await userManager.FindByEmailAsync(login.Email);
                    if (appUser != null)
                    {
                  
                        await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUser, login.Password, login.Remember, false);

                    if (result.Succeeded)
                    {

                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            if (Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl ?? "/");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(login.Email), "Login Failed: Invalid Email or password");
                      
                    }

                }
                else
                {
                    ModelState.AddModelError(nameof(login.Email), "Login Failed: Invalid Email or password");
                   
                }
                   
                }
            return View(login);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            await signInManager.SignOutAsync();
           // HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        //public  IActionResult Logout()
        //{
        //     signInManager.SignOutAsync();
        //    HttpContext.Session.Clear();
        //    return RedirectToAction("Index", "Home");
        //}
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
