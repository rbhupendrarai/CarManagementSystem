using CarManagementSystem.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using CarManagementSystem.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CarManagementSystem.Web.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly RoleManager<IdentityRole> _rolManager;
        private AccountService _accountService;
        public AccountController(RoleManager<IdentityRole> rolManager, UserManager<IdentityUser> userManager, AccountService carService)
        {
            _accountService = carService;
            _userManager = userManager;
            _rolManager = rolManager;
        }
        public IActionResult UserDetail()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserDetail()
        {

            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = HttpContext.Request.Form["start"].FirstOrDefault();
            var length = HttpContext.Request.Form["length"].FirstOrDefault();
            var sortColumn = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"].FirstOrDefault() +
                                         "][name]"].FirstOrDefault();
            string sortColumnDirection = HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();
            var nameSearch = HttpContext.Request.Form["columns[0][search][value]"].FirstOrDefault();
            var emailSearch = HttpContext.Request.Form["columns[1][search][value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var userData = await _accountService.GetUsers();
            //var role = await _userManager.GetRolesAsync(userData.FirstOrDefault());

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                userData = userData.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            //if (!string.IsNullOrEmpty(nameSearch))
            //{
            //    userData = userData.Where(m => m..Contains(nameSearch));

            //}
            //if (!string.IsNullOrEmpty(emailSearch))
            //{
            //    userData = userData.Where(m => m.Email.Contains(emailSearch));

            //}

            recordsTotal = userData.Count();
            var data = userData.Skip(skip);

            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(JsonConvert.SerializeObject(jsonData));
        }


        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

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


                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                ViewBag.Message = "Invalid Login Attampt";

            }

            return View(loginVModel);
        }


        [HttpGet]

        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVModel changePasswordVModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.ChangePassword(changePasswordVModel);

                if (result == true)
                {
                    ViewBag.Message = "Password Update Succesfully";

                }
                else
                {
                    ViewBag.Message = "Old Password Doesn't Match";
                }

            }
            return View(changePasswordVModel);
        }


        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();

            return RedirectToAction("Login");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActiveDeactiveUser(string id)
        {
            try
            {
                var user = _accountService.GetUserByID(id);


            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return RedirectToAction("CarDetail", "Car");
        }
    }
}

