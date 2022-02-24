using CarManagementSystem.Data.Data;
using CarManagementSystem.Data.Models;
using CarManagementSystem.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;

namespace CarManagementSystem.Web.Controllers
{
   
    public class ModelController : Controller
    {

        private readonly CarManagementSystemDbContext _context;
        private readonly ModelService _modelService;
      
        public ModelController(ModelService modelService,CarManagementSystemDbContext context )
        {
            _modelService = modelService;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
       
        public IActionResult ModelDetail()
        {
            var list = _modelService.GetCarList();
            ViewBag.carList = list;
            return View();
        }


        [HttpPost]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetModelDetail()
        {

            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = HttpContext.Request.Form["start"].FirstOrDefault();
            var length = HttpContext.Request.Form["length"].FirstOrDefault();

            //sorting
            var sortColumn = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"].FirstOrDefault() +
                                          "][name]"].FirstOrDefault();
            string sortColumnDirection = HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();


            //searching
            var nameSearch = HttpContext.Request.Form["columns[0][search][value]"].FirstOrDefault();



            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var modelData = await _modelService.GetModel();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                modelData = modelData.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            if (!string.IsNullOrEmpty(nameSearch))
            {
                modelData = modelData.Where(m => m.MO_Name.Contains(nameSearch)
                                            || m.MO_Discription.Contains(nameSearch)
                                           ||  m.MO_Feature.Contains(nameSearch)
                                           || m.CR_Name.Contains(nameSearch)
                );

            }
        
            recordsTotal = modelData.Count();
            var data = modelData.Skip(skip).Take(pageSize).ToList();

            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(JsonConvert.SerializeObject(jsonData));


        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrEditModel(Model model)
        {

            var output = false;
            try
            {
                if (model.MO_Id != Guid.Empty)
                {
                    var result = await _modelService.EditModel(model);
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
                    var result = await _modelService.AddModel(model);
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
        public async Task<IActionResult> GetModelId(Guid id)
        {
            Model model = _modelService.GetModelByID(id);
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveModel(Guid id)
        {
            try
            {
                Model model = _modelService.GetModelByID(id);
                _modelService.DeleteModel(id);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return RedirectToAction("ModelDetail", "Model");
        }

    }
}
