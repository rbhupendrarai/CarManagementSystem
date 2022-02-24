using CarManagementSystem.Data.Data;
using CarManagementSystem.Data.Models;
using CarManagementSystem.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;

namespace CarManagementSystem.Web.Controllers
{
 
    public class SubModelController : Controller
    {       
        private readonly SubModelService _subModelService;       
        public SubModelController(SubModelService subModelService)
        {
            _subModelService = subModelService;           
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult SubModelDetail()
        {
            var list = _subModelService.GetModelList();
            ViewBag.modelList = list;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSubModelDetail()
        { 
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = HttpContext.Request.Form["start"].FirstOrDefault();
            var length = HttpContext.Request.Form["length"].FirstOrDefault();
            var sortColumn = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"].FirstOrDefault() +
                                          "][name]"].FirstOrDefault();
            string sortColumnDirection = HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();
            var nameSearch = HttpContext.Request.Form["columns[0][search][value]"].FirstOrDefault();

            //var subModelDiscription = HttpContext.Request.Form["columns[1][search][value]"].FirstOrDefault();
       
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var subModelsData = await _subModelService.GetSubModel(); //Data Table Source

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                subModelsData = subModelsData.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            if (!string.IsNullOrEmpty(nameSearch))
            {
                subModelsData = subModelsData.Where(m => m.SM_Name.Contains(nameSearch)
                                                       || m.SM_Discription.Contains(nameSearch)
                                                        || m.SM_Feature.Contains(nameSearch)
                                                        || m.MO_Name.Contains(nameSearch)
                );
            }
  
       
            recordsTotal = subModelsData.Count();
            var data = subModelsData.Skip(skip).Take(pageSize).ToList();

            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(JsonConvert.SerializeObject(jsonData));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrEditSubModel(SubModel subModel)
        {

            var output = false;
            try
            {
                if (subModel.SM_Id != Guid.Empty)
                {
                    var result = await _subModelService.EditSubModel(subModel);
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
                    var result = await _subModelService.AddSubmodel(subModel);
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
        public IActionResult GetSMId(Guid id)
        {
            SubModel model = _subModelService.GetSubModelByID(id);
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveSubModel(Guid id)
        {
            try
            {
                SubModel model = _subModelService.GetSubModelByID(id);
                _subModelService.DeleteModel(id);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return RedirectToAction("ModelDetail", "Model");
        }
    }
}
