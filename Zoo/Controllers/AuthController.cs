using Microsoft.AspNetCore.Mvc;
using Zoo.Models;
using Zoo.Data;
using Zoo.Services;
using Microsoft.AspNetCore.Authorization;

namespace Zoo.Controllers
{
    public class AuthController : Controller
    {
        private readonly ITokenService tokenService;
        private readonly ApplicationDbContext context;

        public AuthController(ITokenService tokenService,ApplicationDbContext context)
        {
            this.tokenService = tokenService;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginAdmin admin)
        {
            if (!ModelState.IsValid)
            {
                return View(admin);
            }

            Admin? userFromDb = context.Admins.FirstOrDefault();

            if (userFromDb == null)
                return NotFound();

            if (admin.UserName != userFromDb.UserName || !BCrypt.Net.BCrypt.Verify(admin.Password, userFromDb.HashPassword))
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(admin);
            }

            string token = tokenService.CreateToken(userFromDb);

            Response.Cookies.Append("JWT", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(1)
            });


            return RedirectToAction("Index", "Catalog");
        }

        [Authorize(Policy = "AdminOnly")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("JWT");
            return RedirectToAction("Index", "Home");
        }
    }
}
