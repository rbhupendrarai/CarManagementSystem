using CarManagementSystem.Data.Data;
using CarManagementSystem.Data.Entities;
using CarManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace CarManagementSystem.Service.Services
{
   public class ImageService
    {
        private readonly CarManagementSystemDbContext _context;
        public ImageService(CarManagementSystemDbContext carManagementSystemDbContext)
        {

            _context = carManagementSystemDbContext;
        }
        public List<Model> AddImage()
        {
            return _context.Models
           .Select(model => new Model()
           {
               MO_Id = model.MO_Id,
               MO_Name = model.MO_Name

           }).ToList();
        }
  
    }
}