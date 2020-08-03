using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A91WEBAPI.DTOs;
using A91WEBAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace A91WEBAPI.DAL
{
    public class AuthRepository : IAuthRepository
    {
        private readonly APIDataContext _context;
        public AuthRepository(APIDataContext context)
        {
            _context = context;
        }
        public async Task<AspNetUsers> Login(string username, string password)
        {
            var user = await _context.AspNetUsers.FirstOrDefaultAsync(x => x.UserName == username);
            if (user == null)
                return null;

            //if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            //    return null;

            return user;
        }

        //public async Task<List<string>> GetRoles()
        //{
        //    var Roles = await _context.AspNetRoles.Select(t => t.Name).ToListAsync();
        //    return Roles;
        //}

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF7.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;

                }
            }
            return true;
        }

        public async Task<AspNetUsers> Register(AspNetUsers user, string password)
        {
            byte[] passwordhash, passwordsalt;
            CreatePasswordHash(password, out passwordhash, out passwordsalt);

            AspNetUsers TU = _context.AspNetUsers.Where(t => t.UserName == user.UserName).FirstOrDefault();
            if (TU != null)
            {
                //TU.PasswordHash = passwordhash;
                //TU.PasswordSalt = passwordsalt;
                //TU.Role = user.Role;
                TU.CardCode = user.CardCode;
                TU.Email = user.Email;
                TU.PhoneNumber = user.PhoneNumber;
                _context.AspNetUsers.Update(TU);
                await _context.SaveChangesAsync();
                return TU;
            }

            //user.PasswordHash = passwordhash;
            //user.PasswordSalt = passwordsalt;
            //user.temppass = password;
            await _context.AspNetUsers.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public bool SetRolesAuth(OSS_ROLES_AUTH ROLEAUTH)
        {

            try
            {
                OSS_ROLES_AUTH TU =  _context.OSS_ROLES_AUTH.Where(t => t.U_Role == ROLEAUTH.U_Role).FirstOrDefault();
                if (TU != null)
                {

                    TU.U_Role = ROLEAUTH.U_Role;
                    TU.U_Component = ROLEAUTH.U_Component;
                    _context.OSS_ROLES_AUTH.Update(TU);
                    _context.SaveChanges();

                    return true;
                }           


                string Query = "select   ISNULL(cast(max(cast(Code as int)+1)as nvarchar),'1') as Code from [@OSS_ROLES_AUTH]";
                //MaxCode = _context.Database.SqlQuery<int>(Query).SingleOrDefault();
                var MaxCodeStr =  _context.OSS_ROLES_AUTH.FromSqlRaw(Query).Select(t => t.Code).FirstOrDefault();

                ROLEAUTH.Code = MaxCodeStr;
                ROLEAUTH.Name = MaxCodeStr;
                //newRLAth.U_Role = ROLEAUTH.U_Role;
                //newRLAth.U_Component = ROLEAUTH.U_Component;
                ROLEAUTH.U_Method = "";
                 _context.OSS_ROLES_AUTH.Add(ROLEAUTH);
                 _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            { return false; }
        }
        public async Task<bool> ResetPassword(ChangePasswordViewModel user, string password)
        {
            byte[] passwordhash, passwordsalt;
            CreatePasswordHash(password, out passwordhash, out passwordsalt);

            AspNetUsers TU = _context.AspNetUsers.Where(t => t.UserName == user.UserName).FirstOrDefault();
            if (TU != null)
            {
                //TU.PasswordHash = passwordhash;
                //TU.PasswordSalt = passwordsalt;
                //TU.temppass = password;
                _context.AspNetUsers.Update(TU);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordhash, out byte[] passwordsalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordsalt = hmac.Key;
                passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF7.GetBytes(password));
            }
        }

        public async Task<bool> UserExist(string username)
        {
            if (await _context.AspNetUsers.AnyAsync(x => x.UserName == username))

                return true;
            return false;
        }

        public async Task<AspNetUsers> GetUser(string username)
        {
            var user = await _context.AspNetUsers.FirstOrDefaultAsync(x => x.UserName == username);
            return user;
        }

        public async Task<IEnumerable<dynamic>> GetUserList()
        {
            var user = await _context.AspNetUsers.Select(
                t => new
                {
                    UserName= t.UserName,
                    Role = "",//t.Role,
                    CardCode = t.CardCode,
                    email = t.Email,
                    mobile = t.PhoneNumber,
                    //temppass = t.temppass

                }).ToListAsync();
            return user;
        }
        

        public string GetAssignedRoleComponent(string[] role)
        {
            try
            {
                var components = _context.OSS_ROLES_AUTH.Where(t => role.Contains(t.U_Role)).Select(t => t.U_Component).FirstOrDefault();

                return components;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        

    }
}