﻿using CarManagementSystem.Data.Data;
using CarManagementSystem.Data.DTO;
using CarManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagementSystem.Service.Services
{
   public class SubModelService
    {
        private readonly CarManagementSystemDbContext _context;
        public SubModelService(CarManagementSystemDbContext carManagementSystemDbContext)
        {

            _context = carManagementSystemDbContext;
        }
        public List<Model> GetModelList()
        {
            return _context.Models
           .Select(model => new Model()
           {
               MO_Id = model.MO_Id,
               MO_Name = model.MO_Name

           }).ToList();


        }
        public async Task<IQueryable<CarModelSubModelDTO>> GetSubModel()
        {
            return from m in _context.SubModels // outer sequence
                   join c in _context.Models //inner sequence 
                   on m.MO_Id equals c.MO_Id // key selector 
                   select new CarModelSubModelDTO()
                   { // result selector 
                       SM_Id = m.SM_Id,
                       SM_Name = m.SM_Name,
                       SM_Discription = m.SM_Discription,
                       SM_Feature = m.SM_Feature,
                       SM_Price = m.SM_Price,
                       MO_Name = c.MO_Name
                   };         
        }
        public async Task<bool> AddSubmodel(SubModel subModel)
        {
            try
            {
                subModel.CreatedBy = "Admin";
                subModel.CreatedDate = DateTime.Now;
                subModel.ModifiedBy = "Admin";
                subModel.ModifiedDate = DateTime.Now;
                var result = await _context.SubModels.AddAsync(subModel);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<bool> EditSubModel(SubModel subModel)
        {
            try
            {
                var result =  _context.SubModels.SingleOrDefault(s => s.SM_Id == subModel.SM_Id);
                if (result != null)
                {
                    result.SM_Name = subModel.SM_Name;
                    result.SM_Discription = subModel.SM_Discription;
                    result.SM_Feature = subModel.SM_Feature;
                    result.SM_Price = subModel.SM_Price;
                    result.MO_Id = subModel.MO_Id;
                    result.ModifiedBy = "Admin";
                    result.ModifiedDate = DateTime.Now;
                    await _context.SaveChangesAsync();

                }
                else
                {
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public SubModel GetSubModelByID(Guid id)
        {
            return _context.SubModels.Find(id);
        }
        public void DeleteModel(Guid id)
        {
            SubModel subModel = _context.SubModels.Find(id);
            _context.SubModels.Remove(subModel);
            _context.SaveChanges();
        }

    }
}
