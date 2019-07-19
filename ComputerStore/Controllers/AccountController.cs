using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ComputerStore.Models;
using ComputerStore.Models.ViewModels;

namespace ComputerStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IPasswordHasher<AppUser> passwordHasher;
        private readonly IUserValidator<AppUser> userValidator;
        private readonly IPasswordValidator<AppUser> passwordValidator;

        public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signInMgr, 
                                IPasswordHasher<AppUser> pswHasher, IUserValidator<AppUser> userValid, IPasswordValidator<AppUser> passwordValid)
        {
            userManager = userMgr;
            signInManager = signInMgr;
            passwordHasher = pswHasher;
            userValidator = userValid;
            passwordValidator = passwordValid;
        }

        #region Login

        [AllowAnonymous]
        public IActionResult Login(string returnUrl) => View(new LoginViewModel { ReturnUrl = returnUrl });

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            { 
                AppUser user = await userManager.FindByNameAsync(loginModel.Username);
                string role = userManager.GetRolesAsync(user).Result.FirstOrDefault();
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    var result = await signInManager.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if (role == "Admin")
                            return Redirect(loginModel.ReturnUrl ?? "/Admin");

                        return Redirect(loginModel.ReturnUrl ?? "/");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Wrong password.");
                    }

                } else
                {
                    ModelState.AddModelError("", "Wrong username.");
                }
            }

            return View(loginModel);
        }
        
        #endregion

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByNameAsync(registerModel.Username);
                if (user == null)
                {
                    user = new AppUser
                    {
                        FirstName = registerModel.FirstName,
                        LastName = registerModel.LastName,
                        UserName = registerModel.Username,
                        Email = registerModel.Email,
                        PasswordHash = passwordHasher.HashPassword(user, registerModel.Password),
                        SecurityQuestion = registerModel.SecurityQuestion,
                        SecurityQuestionAnswer = registerModel.SecurityQuestionAnswer.ToLower(),
                        DateCreated = DateTime.Now
                    };

                    await signInManager.SignOutAsync();
                    IdentityResult result = await userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "User");
                        await signInManager.SignInAsync(user, false);
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(registerModel);
        }

        [AllowAnonymous]
        public async Task<JsonResult> ValidateUsername(string username)
        {
            AppUser user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                return Json("This username is already taken.");
            }
            else
            {
                user = new AppUser { UserName = username };
                IdentityResult result = await userValidator.ValidateAsync(userManager, user);
                if (result.Succeeded)
                {
                    return Json(true);
                }
                else
                {
                    return Json("Username is not valid.");
                }
            }
        }

        [AllowAnonymous]
        public async Task<JsonResult> ValidateEmail(string email)
        {
            AppUser user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return Json("User with this email already exists.");
            }
            else
            {
                Regex regex = new Regex(@"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$");
                if (regex.IsMatch(email))
                {
                    return Json(true);
                }
                else
                {
                    return Json("Email is not valid.");
                }
            }
        }

        [AllowAnonymous]
        public async Task<JsonResult> ValidatePassword(string password, string username)
        {
            AppUser user = new AppUser { UserName = username };
            IdentityResult result = await passwordValidator.ValidateAsync(userManager, user, password);
            if (result.Succeeded)
            {
                return Json(true);
            }
            else
            {
                return Json("Password is not valid.");
            }
        }

        [AllowAnonymous]
        public ViewResult AccessDenied() => View();

        [AllowAnonymous]
        public ViewResult ForgotPassword() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByNameAsync(viewModel.Username);
                string resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                await userManager.ResetPasswordAsync(user, resetToken, viewModel.Password);
                return RedirectToAction("Login", "Account");
            }
            return View(viewModel);
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetUser(string username)
        {
            AppUser user = await userManager.FindByNameAsync(username);
            return Json(user);
        }
    }
}