using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace A91WEBAPI.Models
{
    public class Email
    {
        public Email()
        {
            //To = new List<string>();
            CC = new List<string>();
            BCC = new List<string>();            
        }
        public Email(string pCode,string pFrom, string pTo, List<string> pCC, List<string> pBCC, String pSubject, String pBody,
            Boolean pIsHTML)
        {
            
            From = pFrom;
            To = pTo;
            CC = pCC;
            BCC = pBCC;
            Subject = pSubject;
            Body = pBody;
            IsBodyHTML = pIsHTML;
            Code = pCode;


        }
        public string From { get; set; }
        public string Code { get; set; }
        public string To { get; set; }
        public List<string> CC { get; set; }
        public List<string> BCC { get; set; }
        public String Subject { get; set; }
        public String Body { get; set; }
        public Boolean IsBodyHTML { get; set; }
       
        public bool IsQueue { get; set; }
        public bool IsSent { get; set; }
        public String User { get; set; }
        public String FailedReason { get; set; }

    
    }
    public class EmailLog
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public int? U_Attempt { get; set; }
        public string U_EmlFrm { get; set; }
        public string U_BCC { get; set; }
        public string U_CC { get; set; }
        public string U_EmlTO { get; set; }
        public string U_Subject { get; set; }
        public string U_IsQ { get; set; }
        public string U_IsSent { get; set; }
        public string U_IsHTML { get; set; }
        public string U_Body { get; set; }
        public string U_CrtdBy { get; set; }
        public DateTime? U_CrtdDt { get; set; }
        public string U_FailedReason1 { get; set; }
        public string U_FailedReason2 { get; set; }
    }
}
