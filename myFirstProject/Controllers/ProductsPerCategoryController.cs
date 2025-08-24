using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myFirstProject.Data;
using System.Threading.Tasks;
using System.Linq;

namespace myFirstProject.Controllers
{
    public class ProductsPerCategoryController : Controller
    {
        private readonly AdventureWorksContext _context;

        public ProductsPerCategoryController(AdventureWorksContext context)
        {
            _context = context;
        }

        // GET: ProductsPerCategory
        public async Task<IActionResult> Index()
        {
            // Load all product categories for the dropdown
            var categories = await _context.ProductCategories
                .OrderBy(pc => pc.Name)
                .ToListAsync();

            ViewBag.Categories = categories;
            return View();
        }

        // GET: ProductsPerCategory/GetProductsByCategory/5
        [HttpGet]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var products = await _context.Products
                .Where(p => p.ProductCategoryID == categoryId)
                .Select(p => new
                {
                    ProductID = p.ProductID,
                    Name = p.Name,
                    ListPrice = p.ListPrice,
                    Color = p.Color ?? "N/A" // Handle null colors
                })
                .OrderBy(p => p.Name)
                .ToListAsync();

            return PartialView("_ProductsPartial", products);
        }
    }
}
