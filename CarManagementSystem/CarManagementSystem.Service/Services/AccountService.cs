using CarManagementSystem.Data.Data;

using CarManagementSystem.Data.ViewModel;
using CarManagementSystem.Service.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CarManagementSystem.Service.Services
{

    public class AccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly IUserService _userService;//get current loged user
        private readonly CarManagementSystemDbContext _context;
        public AccountService(CarManagementSystemDbContext context,UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;          
            _context=context;
        }
        public async Task<IQueryable> GetUsers()
        {

            return from user in _context.Users
                   join userRole in _context.UserRoles
                   on user.Id equals userRole.UserId
                   join role in _context.Roles
                   on userRole.RoleId equals role.Id
                   select new { 
                         Id=user.Id,
                         LockDate=user.LockoutEnd,
                         UserName=user.UserName,
                         Email=user.Email,
                         Role=role.Name     
                   
                   };
        }        

        public async Task<bool> CreateUser(RegisterVModel registerVModel)
        {
            try
            {
                var user = new IdentityUser
                {
                    UserName = registerVModel.UserName,
                    Email = registerVModel.Email,
                };

                var emailExist = await _userManager.FindByEmailAsync(user.Email);
                if (emailExist != null)
                {
                    return false;
                }
                var result = await _userManager.CreateAsync(user, registerVModel.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "user");
                    //  await _signInManager.SignInAsync(user, isPersistent: true);

                }
                foreach (var error in result.Errors)
                {
                    // _session.SetString("Error",error.Description);
                    ModelState.Equals("Error", error.Description);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {

                ModelState.Equals(ex.Message, "Invalid Login Attempt");

                return false;
            }

        }
        public async Task<bool> ChangePassword(ChangePasswordVModel model)
        {
            var userId = _userService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
       
        public async Task<bool> LoginUser(LoginVModel loginVModel)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(loginVModel.UserName, loginVModel.Password, loginVModel.RememberMe, true);

                if (result.Succeeded)
                {

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
            }
            return true;
        }

        
         public async Task<bool> GetUserByID(string id)
         {
            var result= _context.Users.Find(id);
            var cntResult= result.Id.Count();
            var GetActiveStatus= _context.Users.Where(lockDate => lockDate.LockoutEnd != null && lockDate.Id.Contains(id));//Alerdy Deactive try to Active
            var cntActiveStatus = GetActiveStatus.Count();
            var GetDeactiveStatus = _context.Users.Where(lockDate => lockDate.LockoutEnd == null && lockDate.Id.Contains(id));//Alerdy Active try to DeActive
            var cntDeactiveStatus = GetDeactiveStatus.Count();
            if (cntResult > 0)
            {
                if (cntActiveStatus > 0)
                {
                    result.LockoutEnd= DateTime.Now;                 
                    await _context.SaveChangesAsync();
                }
                if (cntDeactiveStatus > 0)
                {
                    result.LockoutEnd = DateTime.Today.AddDays(12);
                    await _context.SaveChangesAsync();
                }


            }
            return true;


        }
    

        public async Task<bool> Logout()
        {
            await _signInManager.SignOutAsync();
            //await _httpContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return true;
        }
    }
}