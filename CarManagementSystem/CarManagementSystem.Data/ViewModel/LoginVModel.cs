using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarManagementSystem.Data.ViewModel
{
    public class LoginVModel
    {
        [Required]
        [Display(Name ="User Name")]
        [StringLength(30)]
        public string UserName { get; set; }
        [Required]
        [StringLength(15)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
  
        public bool RememberMe { get;  set; }
    }
}
