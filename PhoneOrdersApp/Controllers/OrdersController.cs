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
                return RedirectToAction("Login", "Account");

            if (HttpContext.Session.GetString("Role") != "Operator")
                return Unauthorized();

            ViewBag.Customers = new SelectList(_context.Customers.ToList(), "Id", "Name");
            ViewBag.Products = new SelectList(_context.Products.ToList(), "Id", "Name");

            return View();
        }

        public IActionResult Invoice(int id)
        {
            var order = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
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
        public IActionResult Create(Customer customer, List<OrderItem> items)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            if (HttpContext.Session.GetString("Role") != "Operator")
                return Unauthorized();

            var employeeId = HttpContext.Session.GetInt32("UserId").Value;

            var validItems = items
                .Where(i => !string.IsNullOrWhiteSpace(i.ProductName) && i.Quantity > 0 && i.UnitPrice > 0)
                .ToList();

            if (!validItems.Any())
            {
                ModelState.AddModelError("", "Please Enter one product.");
                ViewBag.Customers = new SelectList(_context.Customers.ToList(), "Id", "Name");
                ViewBag.Products = new SelectList(_context.Products.ToList(), "Id", "Name");
                return View();
            }

            var existingCustomer = _context.Customers.FirstOrDefault(c => c.Phone == customer.Phone);
            if (existingCustomer != null)
            {
                existingCustomer.Name = customer.Name;
                existingCustomer.Address = customer.Address;
                existingCustomer.Note = customer.Note;
                customer = existingCustomer;
            }
            else
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
            }

            var order = new Order
            {
                CustomerId = customer.Id,
                CreatedByEmployeeId = employeeId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                OrderItems = validItems
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            return RedirectToAction("Invoice", new { id = order.Id });
        }

        [HttpGet]
        public IActionResult MyOrders()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || HttpContext.Session.GetString("Role") != "Operator")
                return Unauthorized();

            var orders = _context.Orders
                .Where(o => o.CreatedByEmployeeId == userId)
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }
        [HttpGet]
        public IActionResult RejectedOrders()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || HttpContext.Session.GetString("Role") != "Operator")
                return Unauthorized();

            var orders = _context.Orders
                .Where(o => o.CreatedByEmployeeId == userId && o.Status == "Rejected")
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ToList();

            return View(orders);
        }

        [HttpPost]
        public IActionResult ResubmitOrder(int id, string note)
        {
            var order = _context.Orders.Find(id);
            if (order == null || order.Status != "Rejected")
                return NotFound();

            order.Status = "Pending";
            order.ResubmissionNote = note;
            _context.SaveChanges();

            return RedirectToAction("RejectedOrders");
        }

        [HttpPost]
        public IActionResult CancelOrder(int id, string note)
        {
            var order = _context.Orders.Find(id);
            if (order == null || order.Status != "Rejected")
                return NotFound();

            order.Status = "Cancelled";
            order.ResubmissionNote = note;
            _context.SaveChanges();

            // TODO: Send notification to customer (email/sms)
            return RedirectToAction("RejectedOrders");
        }


        [HttpGet]
        [Route("api/products/search")]
        public IActionResult SearchProducts(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest("Search term is required.");

            var results = _context.Products
                .Where(p => p.Name.Contains(term))
                .OrderBy(p => p.Name)
                .Take(10)
                .Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    price = p.Price
                })
                .ToList();

            return Json(results);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
