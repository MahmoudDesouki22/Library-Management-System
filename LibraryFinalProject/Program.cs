using LibraryFinalProject.IRepository;
using LibraryFinalProject.Models;
using LibraryFinalProject.Models.DbContext;
using LibraryFinalProject.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace LibraryFinalProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // إضافة خدمات التحكم بالواجهات (MVC)
            builder.Services.AddControllersWithViews();

            // إضافة خدمات DbContext باستخدام سلسلة الاتصال المحددة في الإعدادات
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs")));

            // تسجيل المستودعات (Repositories) في حاوية الاعتماديات (Dependency Injection)
            builder.Services.AddScoped<IBookRepo, BookRepo>();
            builder.Services.AddScoped<IMemberRepo, MemberRepo>();
            builder.Services.AddScoped<ILibrarianRepo, LibrarianRepo>();
            builder.Services.AddScoped<ICheckoutsRepo, CheckoutsRepo>();
            builder.Services.AddScoped<IReturnRepo, ReturnRepo>();
            builder.Services.AddScoped<IPenaltyRepo, PenaltyRepo>();
            builder.Services.AddScoped<IGenreRepo, GenreRepo>();

            // إعداد Identity مع دعم الأدوار وربطها بقاعدة البيانات
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                   .AddEntityFrameworkStores<ApplicationDbContext>()
                   .AddDefaultTokenProviders();

            var app = builder.Build();

            // تهيئة قاعدة البيانات والأدوار والمستخدمين عند بدء تشغيل التطبيق
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    // استدعاء DbInitializer لتهيئة قاعدة البيانات
                    DbInitializer.Initialize(context, userManager, roleManager).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    // تسجيل الأخطاء في حال حدوث أي مشكلة أثناء التهيئة
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Error");
                }
            }

            // إعدادات خط الأنابيب (Middleware) لمعالجة الطلبات HTTP
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // إضافة مصادقة المستخدمين (Authentication)
            app.UseAuthentication();

            app.UseAuthorization();

            // تعريف مسار التحكم الرئيسي
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Book}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
