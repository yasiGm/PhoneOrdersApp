using Microsoft.AspNetCore.Mvc;
using PhoneOrdersApp.Models;
using Microsoft.EntityFrameworkCore;

namespace PhoneOrdersApp.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly AppDbContext _context;
        public WarehouseController(AppDbContext context) => _context = context;

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Warehouse") return Unauthorized();

            var orders = _context.Orders
                .Where(o => o.Status == "Pending")
                .Include(o => o.Customer)
                .Include(o => o.CreatedByEmployee)
                .Include(o => o.OrderItems)
                .ToList();

            return View(orders);
        }

        [HttpPost]
        public IActionResult Approve(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();

            order.Status = "Shipped";
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult WarehouseOrders()
        {
            var orders = _context.Orders
                .Where(o => o.Status == "Pending" || o.Status == "Rejected")
                .Include(o => o.Customer)
                .Include(o => o.CreatedByEmployee)  
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        [HttpPost]
        public IActionResult Reject(int id, string rejectReason)
        {
            if (string.IsNullOrWhiteSpace(rejectReason))
            {
                TempData["Error"] = "Please enter a reason for rejection";
                return RedirectToAction("Index");
            }

            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();

            order.Status = "Rejected";
            order.RejectReason = rejectReason;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
