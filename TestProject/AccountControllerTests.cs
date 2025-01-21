using DAL.DBContext;
using DAL.Entities;
using DAL.ModelsVM.UserVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProductManage.Controllers;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class AccountControllerTests
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController1> _logger;
        private readonly AccountController1 _controller;

        public AccountControllerTests()
        {
            // إعداد قاعدة البيانات في الذاكرة
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb") // تعيين قاعدة بيانات في الذاكرة
                .Options;

            _context = new ApplicationDBContext(options);

            // إنشاء UserManager و SignInManager باستخدام EF Core In-Memory
            _userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(_context),
                null, null, null, null, null, null, null, null);

            _signInManager = new SignInManager<ApplicationUser>(
                _userManager,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null);

            _logger = new Logger<AccountController1>(new LoggerFactory());
            _controller = new AccountController1(_signInManager, _userManager, _logger);
        }

        // اختبار Login GET
        [Fact]
        public void Login_Get_ReturnsViewResult()
        {
            var result = _controller.Login();
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        // اختبار Login POST - تسجيل دخول ناجح
        [Fact]
        public async Task Login_Post_ReturnsRedirectToActionResult_WhenLoginIsSuccessful()
        {
            var model = new LoginModel { Email = "test@example.com", Password = "Password123", RememberMe = false };
            var user = new ApplicationUser { UserName = "test@example.com", Email = "test@example.com" };

            // إضافة المستخدم إلى قاعدة البيانات
            await _userManager.CreateAsync(user, model.Password);

            // استدعاء Action الخاص بتسجيل الدخول
            var result = await _controller.Login(model);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        // اختبار Login POST - تسجيل دخول فاشل
        [Fact]
        public async Task Login_Post_ReturnsViewResult_WhenLoginFails()
        {
            var model = new LoginModel { Email = "test@example.com", Password = "WrongPassword", RememberMe = false };

            // لا يتم إضافة المستخدم هنا لأن كلمة المرور خاطئة

            var result = await _controller.Login(model);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
        }

        // اختبار Logout
        [Fact]
        public async Task Logout_ReturnsRedirectToAction_WhenLogoutIsSuccessful()
        {
            var result = await _controller.Logout();
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectResult.ActionName);
        }

        // اختبار Register GET
        [Fact]
        public void Register_Get_ReturnsViewResult()
        {
            var result = _controller.Register();
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        // اختبار Register POST - نجاح التسجيل
        [Fact]
        public async Task Register_Post_ReturnsRedirectToActionResult_WhenRegisterIsSuccessful()
        {
            var model = new RegisterModel { Email = "test@example.com", Password = "Password123", Username = "TestUser", FirstName = "Test", LastName = "User", Phone = "123456789", TypeUser =TypeUser.Admin };
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };

            // إضافة المستخدم إلى قاعدة البيانات
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                var redirectResult = await _controller.Register(model);
                var viewResult = Assert.IsType<RedirectToActionResult>(redirectResult);
                Assert.Equal("Index", viewResult.ActionName);
            }
        }

        // اختبار Register POST - فشل التسجيل
        [Fact]
        public async Task Register_Post_ReturnsViewResult_WhenRegisterFails()
        {
            var model = new RegisterModel { Email = "test@example.com", Password = "Password123", Username = "TestUser", FirstName = "Test", LastName = "User", Phone = "123456789", TypeUser = TypeUser.Admin };

            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };

            // محاكاة فشل إضافة المستخدم
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var viewResult = await _controller.Register(model);
                var resultView = Assert.IsType<ViewResult>(viewResult);
                Assert.Equal(model, resultView.Model);
            }
        }
    }
}
