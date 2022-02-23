using CarManagementSystem.Service.Helper;
using CarManagementSystem.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authentication;
using CarManagementSystem.Data.Data;
using Newtonsoft.Json;

namespace CarManagementSystem.Web.Controllers
{
  
    public class HomeController : Controller
    {
        private readonly CarManagementSystemDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        public HomeController(CarManagementSystemDbContext context, ILogger<HomeController> logger,IUserService userService)
        {
            _logger = logger;
            _userService = userService;
            _context=context;
        }
      

        [Authorize]
        [Authorize(Roles = "Admin,User")]
        public IActionResult Dashboard()
        {

            ViewBag.userId = _userService.GetUserId();
            var isLoggedIn = _userService.IsAuthenticated();
            ViewBag.email = User.FindFirstValue(ClaimTypes.Email);
            ViewBag.userName= User.FindFirstValue(ClaimTypes.Name);
            return View();
            
        }
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetDashboard()
        {

            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = HttpContext.Request.Form["start"].FirstOrDefault();
            var length = HttpContext.Request.Form["length"].FirstOrDefault();
            var sortColumn = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"].FirstOrDefault() +
                                          "][name]"].FirstOrDefault();
            string sortColumnDirection = HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();
            var nameSearch = HttpContext.Request.Form["columns[0][search][value]"].FirstOrDefault();
            var mNameSearch = HttpContext.Request.Form["columns[1][search][value]"].FirstOrDefault();
            var smNameSearch = HttpContext.Request.Form["columns[2][search][value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var carData = from car in _context.Cars // outer sequence
                            join model in _context.Models //inner sequence 
                            on car.CR_Id equals model.CR_Id // key selector 
                            from models in _context.Models
                            join submodel in _context.SubModels
                            on models.MO_Id equals submodel.MO_Id
                            select new
                            { // result selector 
                                CR_Id =   car.CR_Id,
                                CR_Name = car.CR_Name,
                                MO_Name = model.MO_Name,
                                SM_Name = submodel.SM_Name,

                            };

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                carData = carData.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            if (!string.IsNullOrEmpty(nameSearch))
            {
                carData = carData.Where(m => m.CR_Name.Contains(nameSearch));

            }
            if (!string.IsNullOrEmpty(mNameSearch))
            {
                carData = carData.Where(m => m.MO_Name.Contains(mNameSearch));

            }
            if (!string.IsNullOrEmpty(smNameSearch))
            {
                carData = carData.Where(m => m.SM_Name.Contains(smNameSearch));

            }


            recordsTotal = carData.Count();
            var data = carData.Skip(skip).Take(pageSize).ToList();

            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(JsonConvert.SerializeObject(jsonData));


        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
