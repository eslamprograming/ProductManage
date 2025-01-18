using BLL.IService;
using DAL.Entities;
using DAL.ModelsVM.ProductVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProductManage.Controllers
{
    public class ProductAdmin : Controller
    {
        private readonly IProductServic productServic;
        private readonly ICategoryService categoryService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ProductAdmin> _logger; // إضافة الـ Logger

        public ProductAdmin(ICategoryService categoryService, IProductServic productServic, UserManager<ApplicationUser> userManager, ILogger<ProductAdmin> logger)
        {
            this.productServic = productServic;
            this.categoryService = categoryService;
            this._userManager = userManager;
            _logger = logger; // تهيئة الـ Logger
        }

        // عرض المنتجات مع الفلترة حسب الفئة
        public async Task<IActionResult> Index(int? categoryId)
        {
            _logger.LogInformation("Executing Index method with categoryId: {CategoryId}", categoryId); // تسجيل دخول المستخدم مع الـ categoryId
            var result = await categoryService.GetAllCategoryAsync();
            if (result.success)
            {
                ViewBag.Categories = new SelectList(result.values, "Id", "Name", categoryId);
            }
            else
            {
                _logger.LogWarning("Failed to get categories in Index method.");
            }

            dynamic products = null;

            if (categoryId != null && categoryId != 0)
            {
                products = await productServic.GetAllProductProductInCategoryAsync((int)categoryId);
            }
            else
            {
                products = await productServic.GetAllAllProductAsync();
            }

            return View(products.values);
        }

        // عرض تفاصيل المنتج
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Fetching details for product with id: {ProductId}", id); // تسجيل دخول المستخدم مع الـ productId
            var result = await productServic.GetProductEntitybyId(id);
            if (result.success)
            {
                return View(result.Value);
            }
            _logger.LogWarning("Product with id {ProductId} not found.", id); // إذا لم يتم العثور على المنتج
            return NotFound();
        }

        // إضافة منتج جديد (GET)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct()
        {
            _logger.LogInformation("Loading AddProduct view."); // تسجيل الدخول عندما يتم تحميل صفحة إضافة المنتج
            var result = await categoryService.GetAllCategoryAsync();
            if (result.success)
            {
                ViewBag.Categories = result.values;
            }
            else
            {
                _logger.LogWarning("Failed to load categories for AddProduct.");
            }
            return View();
        }

        // إضافة منتج جديد (POST)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(productVM product)
        {
            _logger.LogInformation("Attempting to add product: {ProductName}", product.Name); // تسجيل دخول محاولة إضافة منتج
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                string userId = user.Id;

                var result = await productServic.AddProductAsync(userId, product);
                if (result.success)
                {
                    _logger.LogInformation("Product {ProductName} added successfully.", product.Name); // تسجيل إضافة المنتج بنجاح
                    return RedirectToAction("Index");
                }

                _logger.LogError("Failed to add product {ProductName}.", product.Name); // تسجيل خطأ في إضافة المنتج
                ModelState.AddModelError("", "Failed to add product.");
            }

            var categories = await categoryService.GetAllCategoryAsync();
            if (categories.success)
            {
                ViewBag.Categories = new SelectList(categories.values, "Id", "Name");
            }

            return View(product);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditImage(int id)
        {
            _logger.LogInformation("Loading Edit view for product with id: {ProductId}", id);
            ViewBag.Id = id;
            return View();


        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> EditImage(int id, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    string userId = user.Id;
                    var result = await productServic.UpdateProductAsync(userId, id, ImageFile);
                    if (result.success)
                    {

                        return RedirectToAction("Index");
                    }
                    else { return View(); }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"An error occurred while uploading the image: {ex.Message}";
                    return RedirectToAction("Index");
                }
            }

            TempData["Error"] = "Please select a valid image.";
            return RedirectToAction("Details", new { id });
        }
        // عرض نموذج التعديل (GET)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Loading Edit view for product with id: {ProductId}", id); // تسجيل دخول محاولة تعديل المنتج
            var result = await productServic.GetProductEntitybyId(id);
            if (result.success)
            {
                var categoryResult = await categoryService.GetAllCategoryAsync();
                if (categoryResult.success)
                {
                    ViewBag.Categories = categoryResult.values;
                }
                return View(result.Value);
            }
            _logger.LogWarning("Product with id {ProductId} not found for editing.", id); // إذا لم يتم العثور على المنتج
            return NotFound();
        }

        // تنفيذ التعديل (POST)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(DAL.Entities.Product product)
        {
            _logger.LogInformation("Attempting to update product: {ProductName}", product.Name); // تسجيل دخول محاولة تحديث المنتج
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                string userId = user.Id;
                var result = await productServic.UpdateProductAsync(userId, (int)product.Id, product);
                if (result.success)
                {
                    _logger.LogInformation("Product {ProductName} updated successfully.", product.Name); // تسجيل تحديث المنتج بنجاح
                    return RedirectToAction("Index");
                }

                _logger.LogError("Failed to update product {ProductName}.", product.Name); // تسجيل خطأ في تحديث المنتج
                ModelState.AddModelError("", "Failed to update product.");
            }

            var categories = await categoryService.GetAllCategoryAsync();
            if (categories.success)
            {
                ViewBag.Categories = new SelectList(categories.values, "Id", "Name", product.CategoryId);
            }

            return View(product);
        }

        // تنفيذ الحذف (POST)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            _logger.LogInformation("Attempting to delete product with id: {ProductId}", id); // تسجيل دخول محاولة حذف المنتج
            var result = await productServic.DeleteProductAsync(id);
            if (result.success)
            {
                _logger.LogInformation("Product with id {ProductId} deleted successfully.", id); // تسجيل حذف المنتج بنجاح
                return RedirectToAction("Index");
            }

            _logger.LogError("Failed to delete product with id {ProductId}.", id); // تسجيل خطأ في حذف المنتج
            return NotFound();
        }
    }
}
