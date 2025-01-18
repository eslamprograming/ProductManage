using DAL.Entities;
using DAL.ModelsVM.UserVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // إضافة الـ Logger

namespace ProductManage.Controllers
{
    public class AccountController1 : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController1> _logger; // إضافة الـ Logger

        public AccountController1(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<AccountController1> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger; // تعيين الـ Logger
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation($"User {model.Email} logged in successfully."); // تسجيل الدخول بنجاح
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        _logger.LogWarning($"Failed login attempt for user {model.Email}."); // تسجيل محاولة دخول فاشلة
                    }
                }
                else
                {
                    _logger.LogWarning($"User with email {model.Email} not found."); // تسجيل عدم العثور على المستخدم
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out."); // تسجيل الخروج
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(DAL.ModelsVM.UserVM.RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, PhoneNumber = model.Phone };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.TypeUser.ToString());
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation($"User {model.Email} registered successfully."); // تسجيل نجاح التسجيل
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogError($"Failed to register user {model.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}"); // تسجيل فشل التسجيل
                }
            }
            return View(model);
        }
    }
}
