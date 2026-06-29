using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartInventoryManagement.Data;

namespace SmartInventoryManagement.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Dashboard Cards
            ViewBag.TotalProducts = await _context.Products.CountAsync();

            ViewBag.TotalStock = await _context.Products.SumAsync(p => p.Quantity);

            decimal totalValue = await _context.Products.SumAsync(p => p.Quantity * p.Price);
            ViewBag.TotalValue = FormatCurrency(totalValue);

            ViewBag.LowStock = await _context.Products.CountAsync(p => p.Quantity < 5);

            ViewBag.TotalCategories = await _context.Categories.CountAsync();

            // Recent Products
            ViewBag.RecentProducts = await _context.Products
                .OrderByDescending(p => p.Id)
                .Take(5)
                .ToListAsync();

            // Low Stock Products
            ViewBag.LowStockProducts = await _context.Products
                .Where(p => p.Quantity < 5)
                .OrderBy(p => p.Quantity)
                .Take(5)
                .ToListAsync();

            // Top Expensive Products
            ViewBag.TopProducts = await _context.Products
                .OrderByDescending(p => p.Price)
                .Take(5)
                .ToListAsync();

            return View();
        }

        private string FormatCurrency(decimal amount)
        {
            if (amount >= 10000000)
                return $"₹{amount / 10000000m:0.##} Cr";

            if (amount >= 100000)
                return $"₹{amount / 100000m:0.##} Lakh";

            if (amount >= 1000)
                return $"₹{amount / 1000m:0.##} K";

            return $"₹{amount:N2}";
        }
    }
}