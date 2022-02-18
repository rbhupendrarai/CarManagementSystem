using CarManagementSystem.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarManagementSystem;
using Microsoft.AspNetCore.Identity;
using CarManagementSystem.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using CarManagementSystem.Data.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;


namespace CarManagementSystem.Web.Controllers
{

    public class AccountController : Controller
    {

        private AccountService _accountService;
        public AccountController(AccountService carService)
        {
            _accountService = carService;

        }
      
        [Authorize]
        public IActionResult Index(Car car)
        {
            //  _accountService.GetCar(car);

            return View();
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddUser(RegisterVModel registerVModel)
        {
            
                var result = await _accountService.CreateUser(registerVModel);

                if (result == true)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                     ViewBag.UserNameExist = "User Name Alredy Exist";
                  
                }
                return View(registerVModel);
            
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]

        public async Task<IActionResult> Login(LoginVModel loginVModel)
        {
            
            
            var result = await _accountService.LoginUser(loginVModel);
            if (result == true)
            {
               

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Invalid Login Attampt";

            }

            return View(loginVModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ChangePassowrd(ChangePasswordVModel changePasswordVModel)
        {
            if (ModelState.IsValid)
            {
                await _accountService.ChangePassword(changePasswordVModel);

                //if (result == 0)
                //{
                //    ViewBag.Message = "Password Update Succesfully";
                //    return RedirectToAction("Index", "Home");
                //}
                //else
                //{
                //    ViewBag.Message = "Ivalid Login Attampt";
                //}
                return View(changePasswordVModel);
            }
            return View(changePasswordVModel);
        }

      
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();

            return RedirectToAction("Login");
        }
    }
}
