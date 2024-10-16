using LibraryFinalProject.IRepository;
using LibraryFinalProject.Models;
using LibraryFinalProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace LibraryFinalProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        IMemberRepo MemberRepository;
        ILibrarianRepo LibrarianRepository;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMemberRepo MemberRepo, ILibrarianRepo LibrarianRepo)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            MemberRepository = MemberRepo;
            LibrarianRepository = LibrarianRepo;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel UserVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userModel = await userManager.FindByNameAsync(UserVM.UserName);
                if (userModel != null)
                {
                    bool found = await userManager.CheckPasswordAsync(userModel, UserVM.Password);
                    if (found)
                    {
                        await signInManager.SignInAsync(userModel, UserVM.RememberMe);
                        return RedirectToAction("Index", "Book");
                    }
                }
                ModelState.AddModelError("", "User Name or Password Wrong");
            }
            return View(UserVM);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel NewUserVM)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser Model = new ApplicationUser();
                Model.UserName = NewUserVM.UserName;
                Model.Address = NewUserVM.Address;
                Model.PasswordHash = NewUserVM.Password;
                Model.Full_Name = NewUserVM.FullName;
                Model.Email = NewUserVM.Email;
                Model.PhoneNumber = NewUserVM.Phone;
                IdentityResult result = await userManager.CreateAsync(Model, NewUserVM.Password);
                if (result.Succeeded)
                {
                    MemberRepository.Insert(NewUserVM);
                    await signInManager.SignInAsync(Model, false);
                    return RedirectToAction("Index", "Book");
                }
                else
                {
                    foreach (var ErrorItem in result.Errors)
                    {
                        ModelState.AddModelError("Password", ErrorItem.Description);
                    }
                }
            }
            return View(NewUserVM);
        }
        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddAdmin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdmin(RegisterViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser Model = new ApplicationUser();
                Model.UserName = userVM.UserName;
                Model.Address = userVM.Address;
                Model.PasswordHash = userVM.Password;
                Model.Full_Name = userVM.FullName;
                Model.Email = userVM.Email;
                Model.PhoneNumber = userVM.Phone;
                IdentityResult result = await userManager.CreateAsync(Model, userVM.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(Model, "Admin");
                    return RedirectToAction("Index","Book");
                }
                else
                {
                    foreach (var ErrorItem in result.Errors)
                    {
                        ModelState.AddModelError("Password", ErrorItem.Description);
                    }
                }
            }
            return View(userVM);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AddLibrarian()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLibrarian(RegisterViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser Model = new ApplicationUser();
                Model.UserName = userVM.UserName;
                Model.Address = userVM.Address;
                Model.PasswordHash = userVM.Password;
                Model.Full_Name = userVM.FullName;
                Model.Email = userVM.Email;
                Model.PhoneNumber = userVM.Phone;
                IdentityResult result = await userManager.CreateAsync(Model, userVM.Password);
                if (result.Succeeded)
                {
                    LibrarianRepository.Insert(userVM);
                    await userManager.AddToRoleAsync(Model, "Librarian");
                    return RedirectToAction("Index", "Book");
                }
                else
                {
                    foreach (var ErrorItem in result.Errors)
                    {
                        ModelState.AddModelError("Password", ErrorItem.Description);
                    }
                }
            }
            return View(userVM);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string user)
        
        {
            ApplicationUser userModel = await userManager.FindByNameAsync(user);
            return View(userModel);
        }
        public async Task<IActionResult> Details(string olduser,ApplicationUser NewUserVM, string? oldPassword, string? newPassword)
        {
            if (ModelState.IsValid)
            {
                // البحث عن المستخدم بناءً على اسم المستخدم
                ApplicationUser existingUser = await userManager.FindByNameAsync(olduser);

                if (existingUser != null)
                {
                    // تحديث الحقول الأخرى مثل العنوان، الاسم الكامل، البريد الإلكتروني، رقم الهاتف
                    existingUser.Address = NewUserVM.Address;
                    existingUser.Full_Name = NewUserVM.Full_Name;
                    existingUser.Email = NewUserVM.Email;
                    existingUser.PhoneNumber = NewUserVM.PhoneNumber;
                    existingUser.UserName = NewUserVM.UserName;
                    // تحديث المستخدم
                    var result = await userManager.UpdateAsync(existingUser);
                    if (result.Succeeded)
                    {
                        if (User.IsInRole("Librarian"))
                            LibrarianRepository.Update(olduser,existingUser);
                        else if (User.IsInRole("Admin"))
                        {
                            if (olduser != NewUserVM.UserName)
                            {
                                return RedirectToAction("Logout");
                            }   
                            return RedirectToAction("Index", "Book");
                        }
                        else
                        {
                            MemberRepository.Update(olduser,existingUser);
                        }
                        // التحقق إذا كان يجب تغيير كلمة المرور
                        if (!string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(oldPassword))
                        {
                            var passwordCheck = await userManager.CheckPasswordAsync(existingUser, oldPassword);
                            if (!passwordCheck)
                            {
                                ModelState.AddModelError(string.Empty, "Old password is incorrect.");
                                return View(NewUserVM);
                            }

                            var passwordChangeResult = await userManager.ChangePasswordAsync(existingUser, oldPassword, newPassword);
                            if (!passwordChangeResult.Succeeded)
                            {
                                foreach (var error in passwordChangeResult.Errors)
                                {
                                    ModelState.AddModelError(string.Empty, error.Description);
                                }
                                return View(NewUserVM);
                            }
                        }
                        if (olduser != NewUserVM.UserName)
                        {
                            return RedirectToAction("Logout");
                        }
                        return RedirectToAction("Index","Book");

                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }

            // إعادة عرض النموذج في حالة حدوث خطأ
            return View(NewUserVM);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string user)
        {
            ApplicationUser userModel = await userManager.FindByNameAsync(user);
            return View(userModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string user, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
            {
                ModelState.AddModelError(string.Empty, "Both old and new passwords are required to change the password.");
                return View(await userManager.FindByNameAsync(user));
            }

            var existingUser = await userManager.FindByNameAsync(user);
            if (existingUser != null)
            {
                var passwordCheck = await userManager.CheckPasswordAsync(existingUser, oldPassword);
                if (!passwordCheck)
                {
                    ModelState.AddModelError(string.Empty, "Old password is incorrect.");
                    return View(existingUser);
                }

                var passwordChangeResult = await userManager.ChangePasswordAsync(existingUser, oldPassword, newPassword);
                if (!passwordChangeResult.Succeeded)
                {
                    foreach (var error in passwordChangeResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(existingUser);
                }
                return RedirectToAction("Logout");
            }

            return View(await userManager.FindByNameAsync(user));
        }


    }
}