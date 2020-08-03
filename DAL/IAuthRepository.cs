using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using A91WEBAPI.DTOs;
using A91WEBAPI.Models;

namespace A91WEBAPI.DAL
{
    public interface IAuthRepository
    {
        Task<AspNetUsers> Register(AspNetUsers user, string password);
        Task<AspNetUsers> Login(string username, string password);
        Task<bool> UserExist(string username);
        Task<AspNetUsers> GetUser(string username);
        //Task<List<string>> GetRoles();
        Task<IEnumerable<dynamic>> GetUserList();
         Task<bool> ResetPassword(ChangePasswordViewModel user, string password);
        string GetAssignedRoleComponent(string[] role);
        bool SetRolesAuth(OSS_ROLES_AUTH ROLEAUTH);
    }
}