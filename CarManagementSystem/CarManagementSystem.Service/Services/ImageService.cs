using CarManagementSystem.Data.Data;
using CarManagementSystem.Data.Entities;
using CarManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Data.Entity;
using System.Linq.Dynamic.Core;
namespace CarManagementSystem.Service.Services
{
    public class ImageService
    {
        private readonly CarManagementSystemDbContext _context;
        public ImageService(CarManagementSystemDbContext carManagementSystemDbContext)
        {

            _context = carManagementSystemDbContext;
        }
        public List<Model> GetModel()
        {
            return _context.Models
           .Select(model => new Model()
           {
               MO_Id = model.MO_Id,
               MO_Name = model.MO_Name

           }).ToList();
        }
        public async Task<bool> AddImage(Images img, IFormFile[] fileupload)
        {
            try
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
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public async Task<bool> EditImage(Images img, IFormFile[] fileupload)
        {
            try
            {
                var result = await _context.Image.SingleOrDefaultAsync(x => x.Img_Id == img.Img_Id);
                if (result != null)
                {
                    foreach (IFormFile file in fileupload)
                    {
                        MemoryStream ms = new MemoryStream();
                        file.CopyTo(ms);
                        img.Img = ms.ToArray();
                        ms.Close();
                        ms.Dispose();
                    }
                    result.ModifiedBy = "Admin";
                    result.ModifiedDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
        public Images GetImageById(Guid id)
        {
            return _context.Image.Find(id);
        }
        public void DeleteImage(Guid id)
        {
            Images img = _context.Image.Find(id);
            _context.Image.Remove(img);
            _context.SaveChanges();
        }
         
    }
}