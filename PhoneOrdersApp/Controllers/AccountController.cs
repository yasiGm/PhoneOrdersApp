using Microsoft.AspNetCore.Mvc;
using PhoneOrdersApp.Models;
using System.Linq;

namespace PhoneOrdersApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Employees
                .FirstOrDefault(e => e.Username == username && e.PasswordHash == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Role", user.Role);

                // هدایت بر اساس نقش
                switch (user.Role)
                {
                    case "Operator":
                        return RedirectToAction("Create", "Orders");
                    case "Warehouse":
                        return RedirectToAction("Index", "Warehouse");
                    case "Manager":
                        return RedirectToAction("Index", "Dashboard");
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }
    }
    }
