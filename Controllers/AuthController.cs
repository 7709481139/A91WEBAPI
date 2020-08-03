using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using A91WEBAPI.DAL;
using A91WEBAPI.DTOs;
using A91WEBAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;

namespace A91WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _AuthRepo;
        private readonly IConfiguration _config;
        private readonly IBusinessPartnerRepository _BPrepo;
        private UserManager<AspNetUsers> _UserManager;
        private SignInManager<AspNetUsers> _SignInManager;
        private RoleManager<IdentityRole> _RoleManager;
        public AuthController(RoleManager<IdentityRole> RoleManager, UserManager<AspNetUsers> UserManager, SignInManager<AspNetUsers> SignInManager, IAuthRepository AuthRepo, IConfiguration config,
            IBusinessPartnerRepository BPrepo)
        {
            _AuthRepo = AuthRepo;
            _config = config;
            _BPrepo = BPrepo;
            _UserManager = UserManager;
            _SignInManager = SignInManager;
            _RoleManager = RoleManager;
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetRoles()
        {
            //var result = JsonConvert.SerializeObject(await _AuthRepo.GetRoles());
            var roles = _RoleManager.Roles.ToList();
            var rolesArr = JsonConvert.SerializeObject(roles.Select(t => t.Name).ToArray());
            return Ok(rolesArr);

        }
        [HttpGet("GetAssignedRoleComponent")]
        public IActionResult GetAssignedRoleComponent(string Role)
        {
            string[] cmpStr = new string[] { "" };
            string[] RoleStr = new string[] { "" };
            if (!string.IsNullOrEmpty(Role))
                RoleStr = Role.Split(',');

            var AssignedComponents = _AuthRepo.GetAssignedRoleComponent(RoleStr);
            if(!string.IsNullOrEmpty(AssignedComponents))
            cmpStr = AssignedComponents.Split(',');
            var cmpArr = JsonConvert.SerializeObject(cmpStr.ToArray());
            return Ok(cmpArr);

        }

        [HttpPost("SetRolesAuth")]
        public IActionResult SetRolesAuth(RoleAuthVM obj)
        {

            var thisObj = new OSS_ROLES_AUTH { 
            U_Role=obj.role,
            U_Component = string.Join(",", obj.component.ToArray())
            };

            var result = _AuthRepo.SetRolesAuth(thisObj);
            return Ok();

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto UserDto)
        {

            UserDto.UserName = UserDto.UserName.ToLower();
            var ExistingUser = await _UserManager.FindByNameAsync(UserDto.UserName);
            var UserToCreate = new AspNetUsers
            {
                UserName = UserDto.UserName,
                //Role = UserDto.Role,
                CardCode = UserDto.CardCode,
                Email = UserDto.email,
                PhoneNumber = UserDto.mobile,


            };
            if (ExistingUser != null)
            {

                var Roles = await _UserManager.GetRolesAsync(ExistingUser);
                dynamic Roleresult1 = null;
                if (Roles.Count > 0)
                {
                    var RemoveRoles = await _UserManager.RemoveFromRolesAsync(ExistingUser, Roles);
                }
                foreach (var role in UserDto.Role)
                {
                    Roleresult1 = await _UserManager.AddToRoleAsync(ExistingUser, role);
                }
                ExistingUser.Email = UserDto.email;
                ExistingUser.PhoneNumber = UserDto.mobile;
                ExistingUser.CardCode = UserDto.CardCode;

                var UpdateResult = await _UserManager.UpdateAsync(ExistingUser);
                if (!UpdateResult.Succeeded)
                {
                    return BadRequest(UpdateResult.Errors.Select(t => t.Description).FirstOrDefault());
                }
                return Ok(UpdateResult);
            }


            //var createdUser = await _AuthRepo.Register(UserToCreate, UserDto.PassWord);
            var result = await _UserManager.CreateAsync(UserToCreate, UserDto.PassWord);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(t => t.Description).FirstOrDefault());
            }
            foreach (var role in UserDto.Role)
            {
                var Roleresult = await _UserManager.AddToRoleAsync(UserToCreate, role);
                if (!Roleresult.Succeeded)
                {
                    return BadRequest(Roleresult.Errors.Select(t => t.Description).FirstOrDefault());
                }
            }

            return Ok(result);

        }

        [HttpPost("createrole")]
        public async Task<IActionResult> CreateRole(RoleVM Role)
        {
            if (ModelState.IsValid)
            {
                if (await _RoleManager.RoleExistsAsync(Role.Name))
                    return BadRequest("Role is already exist.");

                Role.Name = Role.Name.ToUpper();
                var IdentityRole = new IdentityRole
                {
                    Name = Role.Name
                };

                var result = await _RoleManager.CreateAsync(IdentityRole);
                if (result.Succeeded)
                    return Ok(result);

            }
            return BadRequest("Error occurred while adding role");

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginVM UserDTO)
        {
            //var UserFromRepo = await _AuthRepo.Login(UserDTO.UserName.ToLower(), UserDTO.PassWord);
            var User = await _UserManager.FindByNameAsync(UserDTO.UserName);
            if (User != null)
            {
                var result = await _SignInManager.CheckPasswordSignInAsync(User, UserDTO.PassWord, false);

                if (result.Succeeded)
                {

                    return Ok(new
                    {
                        token = GenerateJwtToken(User).Result,
                        userid = UserDTO.UserName.ToLower(),
                        userrelated_CardCode = User.CardCode
                    });
                }
            }

            return Unauthorized();

        }
        public async Task<string> GenerateJwtToken(AspNetUsers User)
        {
            var claims = new List<Claim>{
                    new Claim(ClaimTypes.NameIdentifier,User.Id.ToString()),
                    new Claim(ClaimTypes.Name, User.UserName)

                };

            var Roles = await _UserManager.GetRolesAsync(User);

            foreach (var role in Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(60),
                SigningCredentials = cred
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }



        [HttpGet("getbplist")]
        public async Task<IActionResult> GetBPList()
        {
            var BPList = await _BPrepo.BPList();
            return Ok(BPList);
        }

        [HttpGet("getUserslist")]
        public async Task<IActionResult> GetUsersList()
        {
            //var UserList = await _AuthRepo.GetUserList();
            var users = _UserManager.Users.ToList();
            var UserList = JsonConvert.SerializeObject(users.Select(t => t.UserName).ToArray());
            return Ok(UserList);
        }

        [HttpGet("GetUserFromDB")]
        public async Task<IActionResult> GetUserFromDB(string UserName = "")
        {
            var UserVM = new UserVM();
            if (string.IsNullOrEmpty(UserName))
            {
                return Ok(UserVM);
            }
            var user = await _UserManager.FindByNameAsync(UserName);

            if (user != null)
            {
                var Roles = await _UserManager.GetRolesAsync(user);
                UserVM = new UserVM
                {
                    cardCode = user.CardCode,
                    email = user.Email,
                    mobile = user.PhoneNumber,
                    role = Roles.ToList(),
                    UserName = user.UserName

                };

            }
            return Ok(UserVM);



        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ChangePasswordViewModel UserDto)
        {
            var User = await _UserManager.FindByNameAsync(UserDto.UserName);
            var result = await _UserManager.ChangePasswordAsync(User, UserDto.CurrentPassWord, UserDto.NewPassWord);
            if (result.Succeeded)
            {
                await _SignInManager.RefreshSignInAsync(User);
                return StatusCode(200);
            }
            else
            {

                return BadRequest(result.Errors.Select(t => t.Description).FirstOrDefault());
            }
        }

    }
}