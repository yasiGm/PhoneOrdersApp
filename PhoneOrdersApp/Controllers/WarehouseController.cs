using Microsoft.AspNetCore.Mvc;

namespace PhoneOrdersApp.Controllers
{
    public class WarehouseController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (HttpContext.Session.GetString("Role") != "Warehouse")
            {
                return Unauthorized();
            }
            return View();
        }
    }
}
