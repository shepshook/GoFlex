using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using GoFlex.Web.Services.Abstractions;
using GoFlex.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GoFlex.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger _logger;

        public AuthController(IAuthService authService, ILogger logger)
        {
            _authService = authService;
            _logger = logger.ForContext<AuthController>();
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
            {
                _logger.Here().Information("Failed login attempt for {Email}", email);
                return View(LoginResult.IncorrectPassword);
            }

            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "GoFlexType"));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties { AllowRefresh = true });

            _logger.Here().Information("User {Email} with Id {Id} has logged in.", user.Email, user.Id);

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
        public IActionResult SignUp(SignUpViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (string.IsNullOrEmpty(model.Email))
                ModelState.AddModelError("email", "Email cannot be empty");
            else if (model.Email.Length < 4 || model.Email.Length > 128)
                ModelState.AddModelError("email", "Email's length must be [4..128]");
            else
            {
                try
                {
                    model.Email = new MailAddress(model.Email).Address;
                }
                catch (FormatException)
                {
                    ModelState.AddModelError("email", "Please provide a valid email");
                }
            }

            if (string.IsNullOrEmpty(model.Password))
                ModelState.AddModelError("password", "Password cannot be empty");
            else if (model.Password.Length < 4 || model.Password.Length > 128)
                ModelState.AddModelError("password", "Password's length must be [5..128]");

            if (!string.Equals(model.Password, model.ConfirmPassword))
                ModelState.AddModelError("password", "Passwords must match");

            if (!ModelState.IsValid)
                return View("SignUp", model);

            var roleName = model.IsOrganizer ? "Organizer" : "User";

            if (_authService.CreateUser(model.Email, model.Password, roleName))
            {
                _logger.Here().Information("New user created: {Email}", model.Email);
                return Redirect(returnUrl);
            }

            ModelState.AddModelError("email", "This email is already occupied");
            return View(model);
        }

        [Authorize]
        [Route("[action]")]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            _logger.Here().Information("User with Id {Id} has logged out.", HttpContext.User.FindFirst("userId").Value);

            if (string.IsNullOrWhiteSpace(returnUrl))
                return RedirectToAction("List", "Event");

            return Redirect(returnUrl);
        }
    }
}
