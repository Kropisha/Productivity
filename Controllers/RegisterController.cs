using System;
using System.Linq;
using System.Threading.Tasks;
using NetProductivity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using NetProductivity.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace NetProductivity.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public RegisterController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        public DbSet<Login> Logins { get; set; }
        public DbSet<User> Users { get; set; }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var existUser = _userManager.FindByEmailAsync(model.Email).Result;
                if (existUser == null)
                {
                    if (PasswordManager.ValidatePassword(model.Password))
                    {
                        string id = Guid.NewGuid().ToString();
                        User user = new User {UserName = model.Email, Role = Roles.User.ToString(), Id = id};

                        Login login = new Login
                            {Email = model.Email, Password = PasswordManager.HashPassword(model.Password), UserId = id};

                        var result = await _userManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, false);
                            //await Logins.AddAsync(login);
                            //await Users.AddAsync(user);
                            //await SaveChangesAsync();
                            return RedirectToAction("Home", "Productivity");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(String.Empty, error.Description);
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("User exception", "User with this email already exist");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new Login { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Login", "Register");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return RedirectToAction("Main", "Productivity");
        }

        [HttpGet]
        public IActionResult LogOff()
        {
            return RedirectToAction("LogOfff", "Register");
        }

        [HttpGet]
        
        public async Task<IActionResult> LogOfff()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Home", "Productivity");
        }
    }
}
