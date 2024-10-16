using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LibraryFinalProject.Models;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryFinalProject.Models.DbContext
{
    public class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // تطبيق الترحيلات (Migrations)
            context.Database.Migrate();

            // تعريف الأدوار المطلوبة
            string[] roles = { "Admin", "Librarian" };

            // إنشاء الأدوار إذا لم تكن موجودة
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // إنشاء مستخدم Admin إذا لم يكن موجوداً
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Address = "Address3",
                    Full_Name = "Admin1"
                };
                var result = await userManager.CreateAsync(adminUser, "Admin@123"); // تأكد من استخدام كلمة مرور قوية
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // إنشاء مستخدم Librarian إذا لم يكن موجوداً
            var librarianEmail = "librarian@example.com";
            var librarianUser = await userManager.FindByEmailAsync(librarianEmail);
            if (librarianUser == null)
            {
                librarianUser = new ApplicationUser
                {
                    UserName = "librarian",
                    Email = librarianEmail,
                    EmailConfirmed = true,
                    Address = "Address2",
                    Full_Name = "Librarian1"
                };
                var result = await userManager.CreateAsync(librarianUser, "Librarian@123"); // تأكد من استخدام كلمة مرور قوية
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(librarianUser, "Librarian");
                }
            }

            // إنشاء مستخدم Member إذا لم يكن موجوداً
            var memberEmail = "member@example.com";
            var memberUser = await userManager.FindByEmailAsync(memberEmail);
            if (memberUser == null)
            {
                memberUser = new ApplicationUser
                {
                    UserName = "Member",
                    Email = memberEmail,
                    EmailConfirmed = true,
                    Address = "Address1",
                    Full_Name = "Member1"
                };
                var result = await userManager.CreateAsync(memberUser, "Member@123"); // تأكد من استخدام كلمة مرور قوية
                if (result.Succeeded)
                {
                    // يمكنك إضافة دور إذا كان لديك دور للمستخدمين العاديين
                    // await userManager.AddToRoleAsync(memberUser, "User");
                }
            }

            // إضافة بيانات أولية أخرى إذا لزم الأمر
            if (!context.Members.Any())
            {
                var member = new Member()
                {
                    FullName = "Member1",
                    UserName = "Member",
                    Address = "Address1",
                    Email = "member@example.com",
                    Phone = "01126235696"
                };

                context.Members.Add(member);
                await context.SaveChangesAsync();
            }

            if (!context.Librarians.Any())
            {
                var librarian = new Librarian()
                {
                    FullName = "Librarian1",
                    UserName = "Librarian",
                    Address = "Address2",
                    Email = "librarian@example.com",
                    Phone = "01126235697"
                };

                context.Librarians.Add(librarian);
                await context.SaveChangesAsync();
            }
        }
    }
}
