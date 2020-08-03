using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace A91WEBAPI.DTOs
{
    public class UserForRegisterDto
    {
       
        public string UserName { get; set; }
        public string PassWord { get; set; }        
        public List<string> Role { get; set; }
        public string CardCode { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
    }

    public class ChangePasswordViewModel
    {
        public string UserName { get; set; }
        public string CurrentPassWord { get; set; }
        public string NewPassWord { get; set; }

    }

    public class LoginVM
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "you must specify password between 6 to 12 characters")]
        public string PassWord { get; set; }
    }

    public class OCRDVM
    {
        internal string email { get; set; }
        internal string telephone { get; set; }

        public string cardCode { get; set; }
        public string cardName { get; set; }
    }

    public class oclgvm
    {
        public string status;
        public string stage;
        public string enddate;
        public DateTime createdate;
        public string[] userid;
        public double pre_avg;
        public double post_avg;

        public double mpre_avg;
        public double mpost_avg;

        [Required]
        public int clgcode { get; set; }
        [Required]
        public string cardcode { get; set; }
        public string cardname { get; set; }
        public string recontact { get; set; }
        public string closedate { get; set; }
        public string opentime { get; set; }
        public string time { get; set; }
        public string notes { get; set; }
        public int prediscussionrate { get; set; }
        public int postdiscussionrate { get; set; }
        public string prediscussionncommnets { get; set; }
        public string postdiscussioncommnets { get; set; }
        public bool ispreedit { get; set; }
        public bool ispostedit { get; set; }
        public string U_Time { get; internal set; }
        public string CntctTime { get; internal set; }
        public dynamic VoteLines { get; set; }
    }

    public class UserVM { 
        public string UserName { get; set; }
        public List<string>   role { get; set; }
        public string cardCode{ get; set; }
        public string mobile{ get; set; }
        public string email { get; set; }
        public string password{ get; set; }
        public string cpassword { get; set; }
}

    public class voting
    { 
    public int StageID { get; set; }
    public string Company { get; set; }
    public string Type { get; set; }
    public string Stage { get; set; }
    public decimal? PreAvgRate { get; set; }
    public decimal? PostAvgRate { get; set; }
    public decimal? VOTERAvgRATE { get; set; }
    public decimal? VOTINGMANAGERAvgRATE { get; set; }
    public string Status { get; set; }
    public DateTime? StartDate{ get; set; }
    public DateTime? CloseDate { get; set; }
    //public List<votinglines> votinglines { get; set; }
    }
    public class votinglines
    {
        public int StageID { get; set; }
        public string Stage { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }
        public int PreRate { get; set; }
        public string PreComments { get; set; }
        public int PostRate { get; set; }
        public string PostComments { get; set; }
        
    }

    public class votingVM
    {
        public int StageID { get; set; }
        public string Company { get; set; }
        
        public string Stage { get; set; }
        public decimal? Pre{ get; set; }
        public decimal? Post{ get; set; }
        public decimal? Team { get; set; }
        public decimal? Partner { get; set; }
        public string Status { get; set; }
       /* public DateTime? StartDate { get; set; }
        public DateTime? CloseDate { get; set; }*/
        public List<votinglinesVM> votinglinesVM { get; set; }
    }
    public class votinglinesVM
    {
        
        public string Stage { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }
        public int Pre{ get; set; }
        public string Comments { get; set; }
        public int Post{ get; set; }
        public string Comments_ { get; set; }

    }
    public class RoleVM
    { 
    public string Name { get; set; }
    }

    public class RoleAuthVM
    {
        public string role { get; set; }
        public List<string> component { get; set; }
    }

}