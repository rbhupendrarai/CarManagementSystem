using CarManagementSystem.Data.Models;
using CarManagementSystem.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using CarManagementSystem.Data.Data;
using System.Linq.Dynamic.Core;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace CarManagementSystem.Web.Controllers
{
    public class CarController : Controller
    {
        private readonly CarService _carService;      
        public CarController(CarService carService)
        {
            _carService = carService;         
        }     

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public IActionResult CarDetail()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetCarDetail()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = HttpContext.Request.Form["start"].FirstOrDefault();
                var length = HttpContext.Request.Form["length"].FirstOrDefault();
                var sortColumn = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"].FirstOrDefault() +
                                              "][name]"].FirstOrDefault();
                string sortColumnDirection = HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();
                var allSearch = HttpContext.Request.Form["columns[0][search][value]"].FirstOrDefault();


                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                // var carData = (from car in _context.Cars select car);           
                var carData = await _carService.GetCarDetail();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    carData = carData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(allSearch))
                {
                    carData = carData.Where(m => m.CR_Name.Contains(allSearch)
                                                || m.CR_Discription.Contains(allSearch)
                                           );
                }
                recordsTotal = carData.Count();
                var data = carData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(JsonConvert.SerializeObject(jsonData));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message.ToString();
                return RedirectToAction("AddOrEditCar", "Car");
            }
         
        }
      
        [HttpPost]       
        public async Task<IActionResult> AddOrEditCar(Car car,IFormFile files)
        {    
            
            var output = false;
            try
            {               
                if (car.CR_Id != Guid.Empty)
                {
                    var result = await _carService.EditCar(car);
                    if (result == true)
                    {
                        ViewBag.Message = "Update Successfully";
                        output = true;
                    }
                    else
                    {
                        ViewBag.Message = "Not Add";
                    }
                   
                    
                }
                
                else
                {
                    var result = await _carService.AddCar(car,files);
                    if (result == true)
                    {
                        ViewBag.Message = "Add Successfully";
                        output = true;
                    }
                    else
                    {
                        ViewBag.Message = "Not Add";
                    }

                }
            }
            catch (Exception ex)
            {
                output = false;
            }
            return Ok(output);
        }
      


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult GetCarId(Guid id)
        {
            Car car= _carService.GetCarByID(id);
            string value = string.Empty;
            value = JsonConvert.SerializeObject(car, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value);
        }

    
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult RemoveCar(Guid id)
        {
            try
            {
                Car car = _carService.GetCarByID(id);
                _carService.DeleteCar(id);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return RedirectToAction("CarDetail", "Car");
        }

    }

}