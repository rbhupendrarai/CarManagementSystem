using Microsoft.AspNetCore.Http;
using CarManagementSystem.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CarManagementSystem.Data.Data;
using System.IO;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using CarManagementSystem.Data.Models;
namespace CarManagementSystem.Web.Controllers
{
    public class ImageController : Controller
    {
        private readonly CarManagementSystemDbContext _context;
        private readonly ImageService _imageService;
        private readonly SubModelService _subModelService;

        public ImageController(SubModelService subModelService,CarManagementSystemDbContext context, ImageService imageService)
        {
            _imageService = imageService;
            _context = context;
            _subModelService = subModelService;
        }


        [HttpGet]
        public IActionResult ImageDetail()
        {
            var list = _subModelService.GetModelList();
            ViewBag.moList = list;
            return View();

         
          
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult GetImageDetail()
        {

            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = HttpContext.Request.Form["start"].FirstOrDefault();
            var length = HttpContext.Request.Form["length"].FirstOrDefault();

            //sorting
            var sortColumn = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"].FirstOrDefault() +
                                          "][name]"].FirstOrDefault();
            string sortColumnDirection = HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();

            //searching
            var nameSearch = HttpContext.Request.Form["columns[2][search][value]"].FirstOrDefault();
        

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;


            var imgData = from m in _context.Image // outer sequence
                            join c in _context.Models //inner sequence 
                            on m.MO_Id equals c.MO_Id // key selector 
                            select new
                            { // result selector 
                                Img_Id = m.Img_Id,     
                                Img=m.Img,
                                MO_Name = c.MO_Name
                            };
            // var carData = _carService.GetCarDetail();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                imgData = imgData.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            if (!string.IsNullOrEmpty(nameSearch))
            {
                imgData = imgData.Where(m => m.MO_Name.Contains(nameSearch));

            }
           
            recordsTotal = imgData.Count();
            var data = imgData.Skip(skip).Take(pageSize).ToList();

            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(JsonConvert.SerializeObject(jsonData));


        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditImage(Images img,IFormFile[] fileupload)
        {
            try
            {
                if (fileupload != null && img.Img_Id != Guid.Empty)
                {
                    var result = await _imageService.EditImage(img,fileupload);
                    if (result == true)
                    {
                        ViewBag.Message = "Update Successfully";

                    }
                    else
                    {
                        ViewBag.Message = "Not Add";
                    }


                }
                else
                {
                    if (fileupload != null)
                    {
                        foreach (IFormFile file in fileupload)
                        {

                            MemoryStream ms = new MemoryStream();
                            file.CopyTo(ms);
                            img.Img = ms.ToArray();
                            ms.Close();
                            ms.Dispose();
                        }


                    
                    img.CreatedDate = DateTime.Now;
                    img.CreatedBy = "Admin";
                    img.ModifiedDate = DateTime.Now;
                    img.ModifiedBy = "Admin";
                    _context.Image.Add(img);
                    _context.SaveChanges();

                    
                        //var result = await _imageService.AddImage(img, fileupload);
                        //if (result == true)
                        //{
                        //    RedirectToAction("RetrieveImage", "Image");
                        //}

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();

        }

        [HttpGet]
        public IActionResult RetrieveImage()
        {   
            Images img = _context.Image.OrderByDescending(i=>i.Img_Id).FirstOrDefault();
            string imageBase64Data = Convert.ToBase64String(img.Img);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewBag.ImageDataUrl = imageDataURL;
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult GetImageId(Guid id)
        {
            Images img = _imageService.GetImageById(id);
            string value = string.Empty;
            value = JsonConvert.SerializeObject(img, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult RemoveImage(Guid id)
        {
            try
            {
                Images img = _imageService.GetImageById(id);
                _imageService.DeleteImage(id);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return RedirectToAction("ImageDetail", "Image");
        }

    }
}
