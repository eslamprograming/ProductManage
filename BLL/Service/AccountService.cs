//using BLL.IService;
//using DAL.Entities;
//using DAL.ModelsVM.SheardModel;
//using DAL.ModelsVM.UserVM;
//using Microsoft.AspNetCore.Identity;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BLL.Service
//{
//    public class AccountService : IAccountService
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly SignInManager<ApplicationUser> _signInManager;
//        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> _signInManager)
//        {
//            this._userManager = userManager;
//            this._signInManager = _signInManager;
//        }

//        public async Task<Response<object>> LoginAsync(LoginUser loginuser)
//        {
//            var user = await _userManager.FindByEmailAsync(loginuser.Email);

//            if (user == null || !await _userManager.CheckPasswordAsync(user, loginuser.Password))
//                return new() {success=false, message = "Invalid password or email" };

//            var studentRoles = await _userManager.GetRolesAsync(user);


//            return new Response<object>()
//            {
//                success=true,
//                Value=user
//            };
//        }

//        public async Task<Response<object>> RegisterAsync(RegisterModel model)
//        {
//            if (await _userManager.FindByEmailAsync(model.Email) is not null)
//                return new Response<object> { message = "Email is already registered!",success=false };

//            if (await _userManager.FindByNameAsync(model.Username) is not null)
//                return new Response<object> { success=false,message = "Username is already registered!" };

//            var user = new ApplicationUser
//            {
//                UserName = model.Username,
//                Email = model.Email,
//                FirstName = model.FirstName,
//                LastName = model.LastName,
//                PhoneNumber = model.Phone,
//            };

//            var result = await _userManager.CreateAsync(user, model.Password);

//            if (!result.Succeeded)
//            {
//                var errors = string.Empty;

//                foreach (var error in result.Errors)
//                    errors += $"{error.Description},";

//                return new Response<object> { message = errors };
//            }


//            await _userManager.AddToRoleAsync(user, model.TypeUser.ToString());


//            return new Response<object>()
//            {
//                success=true,
//                Value=user
//            };
//        }
//    }
//}
