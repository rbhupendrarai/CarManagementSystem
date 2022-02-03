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

namespace CarManagementSystem.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        private Account _carService;
        public AccountController(Account carService)
        {
            _carService = carService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
      
        public async Task<IActionResult> AddUser(RegisterVModel registerVModel)
        {
            if (ModelState.IsValid)
            {
                await _carService.CreateUser(registerVModel);
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
       
        public async Task<IActionResult> Login(LoginVModel loginVModel)
        {
            if (ModelState.IsValid)
            {
                await _carService.LoginUser(loginVModel);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

       
        public async Task<IActionResult> Logout()
        {
            await _carService.Logout();

            return RedirectToAction("Login");
        }

    }
}
