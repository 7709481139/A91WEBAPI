using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A91WEBAPI.DAL;
using A91WEBAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SAPbobsCOM;
using A91WEBAPI.Global;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using A91WEBAPI.Models;
using Newtonsoft.Json;

namespace A91WEBAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    
    [Route("api/[controller]")]
    [ApiController]
    public class VotingController : ControllerBase
    {
        private readonly IBusinessPartnerRepository _BPrepo;
        private readonly IEmailRepository _EmailRepo;
        
        private readonly IVotingRepository _VotingRepo;
        private UserManager<AspNetUsers> _UserManager;
        private RoleManager<IdentityRole> _RoleManager;
        public VotingController(IEmailRepository EmailRepo,RoleManager<IdentityRole> RoleManager, UserManager<AspNetUsers> UserManager, IBusinessPartnerRepository BPrepo, IVotingRepository VotingRepo)
        {
            _BPrepo = BPrepo;
            _VotingRepo = VotingRepo;
            _UserManager = UserManager;
            _RoleManager = RoleManager;
            _EmailRepo = EmailRepo;
        }

        [HttpGet("getbplist")]        
        public async Task<IActionResult> GetBPList()
        {
            

            var BPList = await _BPrepo.BPList();
            return Ok(BPList);
        }

        [HttpPost("createvotingheader")]
        public async Task<IActionResult> CreateVotingHeader(VotingHeaderVM VH)
        {
            SAPEntity.Instance.InitializeCompany();
            if (SAPEntity.Company.Connected)
            {
                ActivitiesService oActSrv = (ActivitiesService)SAPEntity.Company.GetCompanyService().GetBusinessService(ServiceTypes.ActivitiesService);
                Activity oAct = (Activity)oActSrv.GetDataInterface(ActivitiesServiceDataInterfaces.asActivity);
                ActivityParams oParams;
                oAct.CardCode = VH.cardcode;
                oAct.Activity = BoActivities.cn_Task;
                oAct.ActivityType = 1;
                oAct.PreviousActivity = VH.prevactvty;
                //oAct.Subject = VH.subject;
                DateTime StartDate = DateTime.ParseExact(VH.str_startdate, "dd/MM/yyyy", null).Date;
                DateTime CloseDate = DateTime.ParseExact(VH.str_enddate, "dd/MM/yyyy", null).Date;



                oAct.ActivityDate = StartDate.Date;
                //oAct.EndDuedate = CloseDate;
                oAct.Duration = (CloseDate - StartDate).TotalDays;
                oAct.DurationType = BoDurations.du_Days;
                if (!String.IsNullOrEmpty(VH.time))
                    oAct.UserFields.Item("U_Time").Value = VH.time;
                if (!String.IsNullOrEmpty(VH.opentime))
                    oAct.UserFields.Item("U_OpenTime").Value = VH.opentime;

                oAct.UserFields.Item("U_AddedBy").Value = User.Identity.Name;

                oAct.Notes = VH.remark;
                oParams = oActSrv.AddActivity(oAct);
                long singleActCode = oParams.ActivityCode;
                if (singleActCode > 0)
                {
                    //-----Email Notification
                    var VOTERS = await _UserManager.GetUsersInRoleAsync("VOTER");
                    string[] VOTERStr = VOTERS.Select(t => t.Email).Distinct().ToArray();
                    string voteremails = String.Join(",", VOTERStr);
                    var VOTINGMANAGERS = await _UserManager.GetUsersInRoleAsync("VOTINGMANAGER");
                    VOTERStr = VOTINGMANAGERS.Select(t => t.Email).Distinct().ToArray();
                    voteremails = voteremails + "," + String.Join(",", VOTERStr);                    
                    _EmailRepo.SaveEmailLog(singleActCode, oAct.CardCode, StartDate.Date, CloseDate, voteremails, User.Identity.Name, VH.prevactvty);
                    //-----Email Notification

                    return StatusCode(201);
                }
            }
            else {
                return BadRequest("SAP connection failed.");
            }

            return BadRequest("incorrect inputs provided.");
        }

        [HttpGet("getactivitis/{id}")]
        public async Task<IActionResult> GetActivityList(string id)
        {
            DateTime TodayDate = DateTime.Now.Date;
            var Activitylist = await _VotingRepo.ActivitiList(id, TodayDate);
            return Ok(Activitylist);
        }

        [HttpGet("getactivity/{id}")]
        public async Task<IActionResult> GetActivity(int id)
        {
            var Activitylist = await _VotingRepo.Activity(id);
            return Ok(Activitylist);
        }

        [HttpPost("postsinglevoting")]
        public async Task<IActionResult> postsinglevoting(singlevote VM)
        {

            if (await _VotingRepo.PostSingleVote(VM))
                return StatusCode(201);
            else
                return BadRequest("incorrect inputs provided.");


        }

        [HttpGet("getvotingresult")]
        public async Task<IActionResult> GetVotingResult(string fromdate,string todate,string status)
        {
            var ExistingUser = await _UserManager.FindByNameAsync(User.Identity.Name);
            var Roles = await _UserManager.GetRolesAsync(ExistingUser);

            bool IsAdmin = false;
            foreach (var role in Roles)
            {
                if (role.ToUpper() == "ADMIN" || role.ToUpper() == "VOTINGMANAGER")
                { IsAdmin = true; }
            }

            var Activitylist = await _VotingRepo.GetVotingResult(User.Identity.Name,IsAdmin,  fromdate,  todate,  status);
            return Ok(Activitylist);
        }
    }
}