using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneOrdersApp.Models;

namespace PhoneOrdersApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            if (HttpContext.Session.GetString("Role") != "Manager")
                return Unauthorized();

            var today = DateTime.Today;

            var ordersToday = _context.Orders
                .Include(o => o.CreatedByEmployee)
                .Where(o => o.OrderDate >= today)
                .ToList();

            var operatorStats = ordersToday
                .GroupBy(o => o.CreatedByEmployee)
                .Select(g => new
                {
                    Employee = g.Key,
                    Count = g.Count()
                })
                .ToList();

            var warehouseApproved = ordersToday.Count(o => o.Status == "Shipped");
            var warehouseRejected = ordersToday.Count(o => o.Status == "Rejected");

            ViewBag.OperatorStats = operatorStats;
            ViewBag.TotalOrdersToday = ordersToday.Count;
            ViewBag.WarehouseApproved = warehouseApproved;
            ViewBag.WarehouseRejected = warehouseRejected;

            return View();
        }

        public IActionResult OrdersByOperator(int employeeId)
        {
            var orders = _context.Orders
                .Where(o => o.CreatedByEmployeeId == employeeId)
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            var employee = _context.Employees.Find(employeeId);
            ViewBag.EmployeeName = employee?.Username ?? "Unknown";

            return View(orders);
        }

        public IActionResult WarehouseOrders()
        {
            var orders = _context.Orders
                .Where(o => o.Status == "Pending" || o.Status == "Rejected")
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }
    }
}
