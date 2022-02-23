using CarManagementSystem.Data.Data;
using CarManagementSystem.Data.ViewModel;
using CarManagementSystem.Service.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CarManagementSystem.Service.Services
{

    public class AccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserService _userService;//get current loged user
        public AccountService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            // _session = session;


        }

        public async Task<bool> GetUsers()
        {
            var user = await _userManager.GetUsersInRoleAsync("User");
            return true;
        }

        public async Task<bool> CreateUser(RegisterVModel registerVModel)
        {
            try
            {
                var user = new IdentityUser
                {
                    UserName = registerVModel.UserName,
                    Email = registerVModel.Email,
                };

                var emailExist = await _userManager.FindByEmailAsync(user.Email);
                if (emailExist != null)
                {
                    return false;
                }
                var result = await _userManager.CreateAsync(user, registerVModel.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "user");
                    //  await _signInManager.SignInAsync(user, isPersistent: true);

                }
                foreach (var error in result.Errors)
                {
                    // _session.SetString("Error",error.Description);
                    ModelState.Equals("Error", error.Description);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {

                ModelState.Equals(ex.Message, "Invalid Login Attempt");

                return false;
            }

        }
        public async Task<bool> ChangePassword(ChangePasswordVModel model)
        {
            var userId = _userService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        //public async Task<int> ChangePassword(ChangePasswordVModel changePasswordVModel)
        //{
        //    try
        //    {
        //        var userId = _userService.GetUserId();
        //        var user = await _userManager.FindByIdAsync(userId);
        //        var result = await _userManager.ChangePasswordAsync(user, changePasswordVModel.OldPassword, changePasswordVModel.NewPassword);
        //        if (result.Succeeded)
        //        {                 

        //            return 0;
        //        }
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.Equals("", error.Description);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.Equals(ex.Message, "Not Change");
        //    }
        //    return 0;
        //}

        public async Task<bool> LoginUser(LoginVModel loginVModel)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(loginVModel.UserName, loginVModel.Password, loginVModel.RememberMe, true);

                if (result.Succeeded)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;
        }
        public async Task<bool> Logout()
        {
            await _signInManager.SignOutAsync();
            //await _httpContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return true;
        }
    }
}