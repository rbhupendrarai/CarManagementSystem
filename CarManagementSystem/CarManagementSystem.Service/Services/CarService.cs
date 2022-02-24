using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarManagementSystem.Data.Data;
using CarManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace CarManagementSystem.Service.Services
{
    public class CarService
    {
        private readonly CarManagementSystemDbContext _context;
        public CarService(CarManagementSystemDbContext carManagementSystemDbContext)
        {
            _context = carManagementSystemDbContext;
        }
        public async Task<IQueryable<Car>> GetCarDetail() {

            return _context.Cars
              .Select(car => new Car()
              {
                  CR_Id = car.CR_Id,
                  CR_Name = car.CR_Name,
                  CR_Discription = car.CR_Discription,

              });

        }
     
        public async Task<bool> AddCar(Car car)
        {
            try
            {
                car.CreatedBy ="Admin";
                car.CreatedDate = DateTime.Now;
                car.ModifiedBy = "Admin";
                car.ModifiedDate = DateTime.Now;
                var result = await _context.Cars.AddAsync(car);                
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }           
        }       
        public async Task<bool> EditCar(Car cars)
        {
            try
            {
                var result = await _context.Cars.SingleOrDefaultAsync(x => x.CR_Id == cars.CR_Id);
                if (result != null)
                {
                    result.CR_Name = cars.CR_Name;
                    result.CR_Discription = cars.CR_Discription;                
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
        public Car GetCarByID(Guid id)
        {
            return _context.Cars.Find(id);
        }
        public void DeleteCar(Guid id)
        {
            Car car = _context.Cars.Find(id);
            _context.Cars.Remove(car);
            _context.SaveChanges();
        }

    }
}
