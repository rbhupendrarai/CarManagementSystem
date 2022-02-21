using CarManagementSystem.Data.Entities;
using CarManagementSystem.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagementSystem.Data.Data
{
    public partial class CarManagementSystemDbContext : IdentityDbContext
    {
      

        public CarManagementSystemDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Images> Image { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<SubModel> SubModels { get; set; }
        
      


        
    }
  
}
