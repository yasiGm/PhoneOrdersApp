using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhoneOrdersApp.Models;

namespace PhoneOrdersApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (HttpContext.Session.GetString("Role") != "Operator")
            {
                return Unauthorized();
            }
            ViewBag.Customers = new SelectList(_context.Customers.ToList(), "Id", "Name");
            ViewBag.Products = new SelectList(_context.Products.ToList(), "Id", "Name");

            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            if (HttpContext.Session.GetString("Role") != "Operator")
                return Unauthorized();

            ViewBag.Customers = new SelectList(_context.Customers.ToList(), "Id", "Name");
            ViewBag.Products = new SelectList(_context.Products.ToList(), "Id", "Name");

            return View();
        }
        [HttpPost]
        public IActionResult Create(int customerId, int productId, int quantity)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            if (HttpContext.Session.GetString("Role") != "Operator")
                return Unauthorized();

            var employeeId = HttpContext.Session.GetInt32("UserId").Value;

            var product = _context.Products.Find(productId);
            if (product == null) return NotFound();

            var order = new Order
            {
                CustomerId = customerId,
                CreatedByEmployeeId = employeeId,
                Status = "Pending",
                OrderDate = DateTime.Now,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = productId,
                        Quantity = quantity,
                        UnitPrice = product.Price
                    }
                }
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
