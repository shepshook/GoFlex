using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using GoFlex.Core.Entities;
using GoFlex.Web.Services.Abstractions;
using GoFlex.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoFlex.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("[action]")]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (string.IsNullOrEmpty(email))
                ModelState.AddModelError("email", "Email cannot be empty");
            else if (email.Length < 4 || email.Length > 128)
                ModelState.AddModelError("email", "Email's length must be [4..128]");

            if (string.IsNullOrEmpty(password))
                ModelState.AddModelError("password", "Password cannot be empty");
            else if (password.Length < 4 || password.Length > 128)
                ModelState.AddModelError("password", "Password's length must be [5..128]");

            if (!ModelState.IsValid)
                return View();

            var user = _authService.GetUser(email);
            if (user == null || !_authService.VerifyPassword(user, password))
                return View(LoginResult.IncorrectPassword);

            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "GoFlexType"));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties { AllowRefresh = true });

            //_logger.Information("User {Name} with Id {Id} has logged in.", user.FullName, user.Id);

            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("List", "Event");

            return Redirect(returnUrl);
        }

        [HttpGet("[action]")]
        public IActionResult SignUp(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new SignUpViewModel());
        }

        [HttpPost("[action]")]
        public IActionResult SignUp(string email, string password, string confirmPassword, bool isOrganizer, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (string.IsNullOrEmpty(email))
                ModelState.AddModelError("email", "Email cannot be empty");
            else if (email.Length < 4 || email.Length > 128)
                ModelState.AddModelError("email", "Email's length must be [4..128]");
            else
            {
                try
                {
                    email = new MailAddress(email).Address;
                }
                catch (FormatException)
                {
                    ModelState.AddModelError("email", "Please provide a valid email");
                }
            }

            if (string.IsNullOrEmpty(password))
                ModelState.AddModelError("password", "Password cannot be empty");
            else if (password.Length < 4 || password.Length > 128)
                ModelState.AddModelError("password", "Password's length must be [5..128]");

            if (!string.Equals(password, confirmPassword))
                ModelState.AddModelError("password", "Passwords must match");

            if (!ModelState.IsValid)
                return View(new {returnUrl = returnUrl});

            var roleName = isOrganizer ? "Organizer" : "User";

            if (_authService.CreateUser(email, password, roleName))
                return Redirect(returnUrl);

            ModelState.AddModelError("email", "This email is already occupied");
            return View(new {returnUrl = returnUrl});
        }

        [Authorize]
        [Route("[action]")]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            //_logger.Information("User {Name} with Id {Id} has logged out.",
            //    HttpContext.User.FindFirst(ClaimTypes.Name).Value, HttpContext.User.FindFirst("userId").Value);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (string.IsNullOrWhiteSpace(returnUrl))
                return RedirectToAction("List", "Event");

            return Redirect(returnUrl);
        }
    }
}
