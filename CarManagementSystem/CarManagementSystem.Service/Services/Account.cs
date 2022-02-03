using AutoMapper;
using CarManagementSystem.Data;
using CarManagementSystem.Data.Data;

using CarManagementSystem.Data.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CarManagementSystem.Service.Services
{
    public class Account
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public Account(DbContextOptions context, UserManager<IdentityUser> userManager,
                                      SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;


        }
        public async Task<long> CreateUser(RegisterVModel registerVModel)
        {
            try
            {

                var user = new IdentityUser
                {
                    UserName = registerVModel.UserName,
                    Email = registerVModel.Email,
                };

                var result = await _userManager.CreateAsync(user, registerVModel.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return 0;

                }


                foreach (var error in result.Errors)
                {
                    ModelState.Equals("", error.Description);
                }
            }
            catch (Exception ex)
            {
                ModelState.Equals(ex.Message, "Invalid Login Attempt");
            }
            return 0;
        }
        public async Task<long> UpdateUser(RegisterVModel registerVModel)
        {
            try
            {

                var user = new IdentityUser
                {
                    UserName = registerVModel.UserName,
                    Email = registerVModel.Email,
                };

                var result = await _userManager.CreateAsync(user, registerVModel.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return 0;

                }


                foreach (var error in result.Errors)
                {
                    ModelState.Equals("", error.Description);
                }
            }
            catch (Exception ex)
            {
                ModelState.Equals(ex.Message, "Invalid Login Attempt");
            }
            return 0;
        }

        public async Task<int> LoginUser(LoginVModel loginVModel)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(loginVModel.UserName, loginVModel.Password, loginVModel.RememberMe, false);

                if (result.Succeeded)
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }

            return 0;
        }
  
        public async Task<int> Logout()
        {
            await _signInManager.SignOutAsync();

            return 0;
        }
    }
}