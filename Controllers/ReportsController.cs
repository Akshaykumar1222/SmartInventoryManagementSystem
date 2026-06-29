using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartInventoryManagement.Data;

namespace SmartInventoryManagement.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalProducts = await _context.Products.CountAsync();

            ViewBag.TotalCategories = await _context.Categories.CountAsync();

            ViewBag.TotalStock = await _context.Products.SumAsync(p => p.Quantity);

            decimal totalValue = await _context.Products.SumAsync(p => p.Price * p.Quantity);
            ViewBag.TotalValue = FormatCurrency(totalValue);

            ViewBag.LowStock = await _context.Products.CountAsync(p => p.Quantity < 5);

            ViewBag.AveragePrice = await _context.Products.AnyAsync()
                ? await _context.Products.AverageAsync(p => p.Price)
                : 0;

            ViewBag.HighestPrice = await _context.Products.AnyAsync()
                ? await _context.Products.MaxAsync(p => p.Price)
                : 0;

            ViewBag.LowestPrice = await _context.Products.AnyAsync()
                ? await _context.Products.MinAsync(p => p.Price)
                : 0;

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