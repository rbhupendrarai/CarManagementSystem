
using Microsoft.AspNetCore.Http;
using CarManagementSystem.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CarManagementSystem.Data.Entities;
using CarManagementSystem.Data.Data;
using System.IO;
using System;

namespace CarManagementSystem.Web.Controllers
{
    public class ImageController : Controller
    {
        private readonly CarManagementSystemDbContext _context;
        private readonly ImageService _imageService;

        public ImageController(CarManagementSystemDbContext context, ImageService imageService)
        {
            _imageService = imageService;
            _context = context;
        }


        [HttpGet]
        public IActionResult AddImg()
        {   


            return View();
        }
        [HttpPost]
        public ActionResult AddImg(Images img,IFormFile[] file1)
        {
            try
            {
                foreach (var file in file1)
                {
                    
                    img.CreatedDate = DateTime.Now;
                    img.CreatedBy = "Admin";
                    img.ModifiedDate = DateTime.Now;
                    img.ModifiedBy = "Admin";
                    img.MO_Id = Guid.Parse("a40f50ae-6aae-4f27-c8f7-08d9f2a801fd");
                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    img.Img = ms.ToArray();
                    ms.Close();
                    ms.Dispose();
                    _context.Image.Add(img);
                    _context.SaveChanges();
                }
         
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();

        }


    }
}
