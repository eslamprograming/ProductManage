using BLL.IService;
using DAL.Entities;
using DAL.ModelsVM.ProductVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ProductManage.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductServic productServic;
        private readonly ICategoryService categoryService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(ILogger<ProductController> logger, IProductServic productServic, ICategoryService categoryService, UserManager<ApplicationUser> _userManager)
        {
            this.productServic = productServic;
            this.categoryService = categoryService;
            this._userManager = _userManager;
            _logger = logger;
        }

        // عرض جميع المنتجات
        
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Accessing the product list page.");
            var result = await productServic.GetAllProductAsync();
            if (result.success)
            {
                var result2 = await categoryService.GetAllCategoryAsync();
                if (result2.success)
                {
                    ViewBag.Categories = result2.values;
                    _logger.LogInformation("Categories fetched successfully.");
                }
                return View(result.values);
            }

            _logger.LogWarning("Failed to fetch products.");
            return View(); // إعادة عرض الصفحة في حالة الفشل
        }

        // دالة GET لإضافة منتج جديدة - محمية للمستخدمين الذين لديهم دور Admin
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct()
        {
            _logger.LogInformation("Accessing the Add Product page.");
            var result = await categoryService.GetAllCategoryAsync();
            if (result.success)
            {
                ViewBag.Categories = result.values;
                _logger.LogInformation("Categories for new product fetched successfully.");
            }
            else
            {
                _logger.LogWarning("Failed to fetch categories for new product.");
            }
            return View();
        }

        // دالة POST لإضافة منتج جديدة - محمية للمستخدمين الذين لديهم دور Admin
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(productVM product)
        {
            _logger.LogInformation("Attempting to add a new product.");
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                string userId = user.Id;

                var result = await productServic.AddProductAsync(userId, product);
                if (result.success)
                {
                    _logger.LogInformation("Product added successfully.");
                    return RedirectToAction("Index");
                }
                _logger.LogError("Failed to add product.");
                ModelState.AddModelError("", "Failed to add product.");
            }
            return RedirectToAction("AddProduct");
        }

        // دالة POST لتحديث المنتج
        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> UpdateProduct(int id, DAL.Entities.Product product)
        //{
        //    _logger.LogInformation($"Attempting to update product with ID: {id}");
        //    if (ModelState.IsValid)
        //    {
        //        var result = await productServic.UpdateProductAsync(id, product);
        //        if (result.success)
        //        {
        //            _logger.LogInformation("Product updated successfully.");
        //            return RedirectToAction("Index");
        //        }
        //        _logger.LogError("Failed to update product.");
        //        ModelState.AddModelError("", "Failed to update product.");
        //    }
        //    return RedirectToAction("Index");
        //}

        // دالة POST لحذف المنتج
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            _logger.LogInformation($"Attempting to delete product with ID: {id}");
            var result = await productServic.DeleteProductAsync(id);
            if (result.success)
            {
                _logger.LogInformation("Product deleted successfully.");
                return RedirectToAction("Index");
            }
            _logger.LogError("Failed to delete product.");
            ModelState.AddModelError("", "Failed to delete product.");
            return View();
        }
    }
}
