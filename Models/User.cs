using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A91WEBAPI.Models
{
    public class AspNetUsers : IdentityUser
    {
        //[Column(TypeName="nvarchar(30)")]
        public string CardCode { get; set; }

    }

    //public class AspNetUserRoles:IdentityUserRole<string>
    //{
    //    //[Key]
    //    //public int UserId { get; set; }
    //    //public string RoleId { get; set; }
    //    ////public string IsA91 { get; set; }
    //}

    //public class AspNetRoles: IdentityRole
    //{  
    //}
    
    public class OSS_ROLES_AUTH
    {[Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string U_Method { get; set; }
        public string U_Role { get; set; }
        public string U_Component { get; set; }
    }
}