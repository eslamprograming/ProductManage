using BLL;
using DAL;
using DAL.DBContext;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// إعدادات Logging
builder.Logging.ClearProviders();  // إزالة المزودين الافتراضيين
builder.Logging.AddConsole();      // إضافة سجلات إلى وحدة التحكم
builder.Logging.AddDebug();        // إضافة سجلات إلى أداة التصحيح
builder.Host.UseSerilog((context, config) => config
    .WriteTo.Console()            // السجلات في وحدة التحكم
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)  // السجلات في ملف
    .Enrich.FromLogContext()      // إضافة السياق إلى السجلات
    .WriteTo.Debug());            // إضافة السجلات إلى أداة التصحيح

// إعداد Identity (التحقق من الهوية)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>()  // ربط Identity مع DbContext
    .AddDefaultTokenProviders();  // إضافة مزودين للرموز المميزة (Token Providers)

// إعداد DbContext للوصول إلى قاعدة البيانات
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))  // استخدام SQL Server مع الاتصال المحدد
);

// تخصيص إعدادات الـ Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";  // مسار صفحة تسجيل الدخول
    options.AccessDeniedPath = "/Account/AccessDenied";  // مسار صفحة الوصول المرفوض
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);  // تحديد مدة صلاحية الـ Cookie
    options.SlidingExpiration = true;  // تمكين التحديث التلقائي للـ Cookie عند اقتراب انتهاء الصلاحية
});

// إضافة خدمات DAL و BLL
builder.Services.serviceDescriptors();  // إضافة خدمات DAL
builder.Services.BLLServicer();  // إضافة خدمات BLL

// إضافة خدمات الـ MVC
builder.Services.AddControllersWithViews();  // إضافة خدمات الـ Controllers مع Views

var app = builder.Build();

// تكوين HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");  // معالجة الأخطاء في بيئة الإنتاج
    app.UseHsts();  // تمكين HSTS (Strict Transport Security)
}
else
{
    app.UseDeveloperExceptionPage();  // عرض أخطاء التفصيل في بيئة التطوير
}

app.UseHttpsRedirection();  // إعادة توجيه HTTP إلى HTTPS
app.UseStaticFiles();  // خدمة الملفات الثابتة (مثل الصور، CSS، JS)
app.UseRouting();  // تمكين التوجيه (Routing)

app.UseAuthentication();  // تفعيل التوثيق (Authentication)
app.UseAuthorization();   // تفعيل التفويض (Authorization)

// تحديد المسار الافتراضي للتحكم (Controller)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")  // تعيين الـ controller والـ action الافتراضيين
    .WithStaticAssets();  // تضمين الملفات الثابتة ضمن المسار

app.Run();  // تشغيل التطبيق
