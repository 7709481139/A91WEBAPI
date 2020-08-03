using A91WEBAPI.DTOs;
using A91WEBAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace A91WEBAPI.DAL
{
    public class EmailRepository : IEmailRepository
    {
        public readonly APIDataContext _context;
      
        public EmailRepository(APIDataContext context)
        {
            _context = context;
            
        }



        public string GetMaxCode()
        {
            
            string MaxCode = _context.EmailLog.FromSqlInterpolated($"select isnull(cast(max(cast(Code as int)+1)as nvarchar),1) as Code from [@OSS_EMLG]").Select(t => t.Code).FirstOrDefault();
           
            return MaxCode;
        }


        public bool SaveEmailLog(long singleActCode, string CardCode, DateTime StartDate, DateTime CloseDate, string VotersEmail, string User,int prevactvty)
        {

            IConfigurationRoot Configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();

            string ServerURL = Configuration.GetSection("ServerURL").Value; ;// System.Configuration.ConfigurationManager.AppSettings["ServerUrl"];
            string Messagebody = "";
            bool IsSaved = false;
            try
            {

                
                Email EmailMessage = new Email();
                string CardName = _context.OCRD.Where(t => t.CardCode == CardCode).Select(t => t.CardName).FirstOrDefault();
                int SubjectCode = _context.OCLG.Where(t => t.ClgCode == prevactvty).Select(t => t.CntctSbjct).FirstOrDefault();
                string SubjectName = _context.OCLS.Where(t => t.Code == SubjectCode).Select(t => t.Name).FirstOrDefault();
                EmailMessage.Subject = "Invitation for voting ("+ SubjectName + ") - "+CardName;

                string startdate = (StartDate).ToString("dd/MM/yyyy");
                string closedate = (CloseDate).ToString("dd/MM/yyyy");
                //string doctotal = string.Format("{0:0,0.00}", DocTotal);


                Messagebody = "Dear Voter,<br><br> Greetings !<br><br>A new voting is defined for the <b> " + CardName + "</b> by " + User +"."+
                                "<br>Voting will be open from :<b>" + startdate + "</b> to :<b>" + closedate + " 10:00 AM</b>." +
                                "<br>Please cast your vote by clicking on below link." +
                                "<br><br>" + ServerURL +
                                "<br><br><br>Regards<br>" + User;



                EmailMessage.User = "Admin";
                EmailMessage.IsBodyHTML = false;
                EmailMessage.Body = Messagebody;
                //var recipients = new List<string>();
                
                //string[] recipients = ApproverEmail.Split(';', ':', ',');
                string MaxCode = _context.EmailLog.FromSqlInterpolated($"select isnull(cast(max(cast(Code as int)+1)as nvarchar),1) as Code from [@OSS_EMLG]").Select(t => t.Code).FirstOrDefault();
                EmailMessage.Code = MaxCode;
                EmailMessage.To = VotersEmail;
                IsSaved = Save(EmailMessage);
                /*
                for (var i = 0; i < recipients.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(recipients[i]))
                    {
                        EmailMessage.To.Add(recipients[i]);
                    }
                }

                if (EmailMessage.To.Count > 0)
                {
                    IsSaved = Save(EmailMessage);
                }*/

            }
            catch (Exception Ex)
            {

                return false;
            }

            return true;
        }
        public Boolean Save(Email eml)
        {
            try
            {

               
                    EmailLog emlLog = new EmailLog();                    
                    emlLog.Code = eml.Code;
                    emlLog.Name = emlLog.Code;
                    emlLog.U_Attempt = 1;
                    emlLog.U_Subject = eml.Subject;
                    emlLog.U_Body = eml.Body;
                    emlLog.U_EmlFrm = "";
                    emlLog.U_EmlTO = eml.To;

                    if (eml.IsBodyHTML == true)
                    {
                        emlLog.U_IsHTML = "1";
                    }
                    else
                    {
                        emlLog.U_IsHTML = "0";
                    }
                    if (eml.IsBodyHTML == true)
                        emlLog.U_Subject = eml.Subject;
                    emlLog.U_CrtdBy = eml.User;
                    emlLog.U_CrtdDt = DateTime.Now;
                    if (eml.IsQueue == true)
                    {
                        emlLog.U_IsQ = "1";
                    }
                    else
                    {
                        emlLog.U_IsQ = "0";
                    }

                    if (eml.IsSent == true)
                    {
                        emlLog.U_IsSent = "1";
                    }
                    else
                    {
                        emlLog.U_IsSent = "0";
                    }

                    emlLog.U_FailedReason1 = eml.FailedReason;
                    _context.EmailLog.Add(emlLog);
                    _context.SaveChanges();
                    //return true;
                
                return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }
    }
}
