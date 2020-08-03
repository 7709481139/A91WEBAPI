using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using A91WEBAPI.DAL;
using A91WEBAPI.DTOs;
using A91WEBAPI.Global;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace A91WEBAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly IBusinessPartnerRepository _BPrepo;
        private readonly IVotingRepository _VotingRepo;
        public MeetingController(IBusinessPartnerRepository BPrepo, IVotingRepository VotingRepo)
        {
            _BPrepo = BPrepo;
            _VotingRepo = VotingRepo;
        }
        [HttpPost("CreateBoardMeeting")]
        public async Task<IActionResult> CreateBoardMeeting(MeetingVM VM)
        {

            int IsAttchInserted = -1;
            string strMessage = "incorrect inputs provided.";
            SAPEntity.Instance.InitializeCompany();
            if (SAPEntity.Company.Connected)
            {
                ActivitiesService oActSrv = (ActivitiesService)SAPEntity.Company.GetCompanyService().GetBusinessService(ServiceTypes.ActivitiesService);
                Activity oAct = (Activity)oActSrv.GetDataInterface(ActivitiesServiceDataInterfaces.asActivity);
                ActivityParams oParams;
                oAct.CardCode = VM.cardcode;
                oAct.Activity = BoActivities.cn_Task;
                oAct.ActivityType = 2;

                if (VM.cntctcode > 0)
                    oAct.ContactPersonCode = VM.cntctcode;

                DateTime StartDate = DateTime.ParseExact(VM.str_startdate, "dd/MM/yyyy", null);
                DateTime CloseDate = DateTime.ParseExact(VM.str_enddate, "dd/MM/yyyy", null);

                oAct.ActivityDate = StartDate;
                oAct.Duration = (CloseDate - StartDate).TotalDays;
                oAct.DurationType = BoDurations.du_Days;
                if (!String.IsNullOrEmpty(VM.time))
                    oAct.UserFields.Item("U_Time").Value = VM.time;
                if (VM.assignesto.HasValue && VM.assignesto.Value > 0)
                    oAct.HandledBy = VM.assignesto.Value;
                oAct.Notes = VM.notes;
                oAct.Details = VM.details;

                if (VM.files != null && VM.files.Count() > 0)
                {
                    SAPbobsCOM.Attachments2 SAPAttachment = (SAPbobsCOM.Attachments2)SAPEntity.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);
                    foreach (var FL in VM.files)
                    {

                        if (!string.IsNullOrEmpty(FL))
                        {
                            SAPAttachment.Lines.SetCurrentLine((SAPAttachment.Lines.Count) - 1);
                            string FileName = System.IO.Path.GetFileName(FL);
                            string FileExt = System.IO.Path.GetExtension(FileName);
                            FileExt = FileExt.Replace('.', ' ').Trim();
                            SAPAttachment.Lines.SourcePath = @"H:\Upload";
                            SAPAttachment.Lines.FileName = FileName;
                            SAPAttachment.Lines.FileExtension = Path.GetExtension(FileExt);
                            SAPAttachment.Lines.Add();
                        }
                    }

                    if (VM.files.Count() > 0)
                    {
                        IsAttchInserted = SAPAttachment.Add();
                        if (IsAttchInserted == 0)
                        {
                            string ObjCode;
                            SAPEntity.Company.GetNewObjectCode(out ObjCode);
                            oAct.AttachmentEntry = Convert.ToInt32(ObjCode);
                        }
                        else
                        {
                            strMessage = SAPEntity.Company.GetLastErrorDescription();
                        }
                    }
                }


                oParams = oActSrv.AddActivity(oAct);
                long singleActCode = oParams.ActivityCode;
                if (singleActCode > 0)
                    return StatusCode(201);
            }

            return BadRequest("incorrect inputs provided.");
        }

        [HttpPost("submitWeeklyMeeting")]
        public async Task<IActionResult> submitWeeklyMeeting(MeetingVM VM)
        {

            int IsAttchInserted = -1;
            string strMessage = "incorrect inputs provided.";
            SAPEntity.Instance.InitializeCompany();
            if (SAPEntity.Company.Connected)
            {
                ActivitiesService oActSrv = (ActivitiesService)SAPEntity.Company.GetCompanyService().GetBusinessService(ServiceTypes.ActivitiesService);
                Activity oAct = (Activity)oActSrv.GetDataInterface(ActivitiesServiceDataInterfaces.asActivity);
                ActivityParams oParams;
                oAct.CardCode = VM.cardcode;
                oAct.Activity = BoActivities.cn_Task;
                oAct.ActivityType = 4;

                if (VM.cntctcode > 0)
                    oAct.ContactPersonCode = VM.cntctcode;

                DateTime StartDate = DateTime.ParseExact(VM.str_startdate, "dd/MM/yyyy", null);
                DateTime CloseDate = DateTime.ParseExact(VM.str_enddate, "dd/MM/yyyy", null);

                oAct.ActivityDate = StartDate;
                oAct.Duration = (CloseDate - StartDate).TotalDays;
                oAct.DurationType = BoDurations.du_Days;
                if (!String.IsNullOrEmpty(VM.time))
                    oAct.UserFields.Item("U_Time").Value = VM.time;
                if (VM.assignesto.HasValue && VM.assignesto.Value > 0)
                    oAct.HandledBy = VM.assignesto.Value;
                oAct.Notes = VM.notes;
                oAct.Details = VM.details;

                if (VM.files != null && VM.files.Count() > 0)
                {
                    SAPbobsCOM.Attachments2 SAPAttachment = (SAPbobsCOM.Attachments2)SAPEntity.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);
                    foreach (var FL in VM.files)
                    {

                        if (!string.IsNullOrEmpty(FL))
                        {
                            SAPAttachment.Lines.SetCurrentLine((SAPAttachment.Lines.Count) - 1);
                            string FileName = System.IO.Path.GetFileName(FL);
                            string FileExt = System.IO.Path.GetExtension(FileName);
                            FileExt = FileExt.Replace('.', ' ').Trim();
                            SAPAttachment.Lines.SourcePath = @"H:\Upload";
                            SAPAttachment.Lines.FileName = FileName;
                            SAPAttachment.Lines.FileExtension = Path.GetExtension(FileExt);
                            SAPAttachment.Lines.Add();
                        }
                    }

                    if (VM.files.Count() > 0)
                    {
                        IsAttchInserted = SAPAttachment.Add();
                        if (IsAttchInserted == 0)
                        {
                            string ObjCode;
                            SAPEntity.Company.GetNewObjectCode(out ObjCode);
                            oAct.AttachmentEntry = Convert.ToInt32(ObjCode);
                        }
                        else
                        {
                            strMessage = SAPEntity.Company.GetLastErrorDescription();
                        }
                    }
                }


                oParams = oActSrv.AddActivity(oAct);
                long singleActCode = oParams.ActivityCode;
                if (singleActCode > 0)
                    return StatusCode(201);
            }

            return BadRequest("incorrect inputs provided.");
        }


        [HttpGet("getMeetingData/{id}")]
        public async Task<IActionResult> getMeetingData(int id)
        {
            var Activitylist = await _VotingRepo.getMeetingData(id);
            return Ok(Activitylist);
        }
        
    }
}
