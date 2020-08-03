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
using System.IO;
using System.Net.Http.Headers;

namespace A91WEBAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ComplieanceController : ControllerBase
    {
        private readonly IComplianceRepository _CRepo;

        public ComplieanceController(IComplianceRepository CRepo)
        {
            _CRepo = CRepo;

        }


        [HttpGet("getcompliancelist/{id}")]
        public async Task<IActionResult> getComplianceList(string id)
        {
            var list = await _CRepo.GetComplianceList(id);
            return Ok(list);
        }

        [HttpGet("getclientcontactperson/{id}")]
        public async Task<IActionResult> getClientContactPerson(string id)
        {
            var list = await _CRepo.GetBPContactPerson(id);
            return Ok(list);
        }


        [HttpGet("getsubjectlist")]
        public async Task<IActionResult> getSubjectList()
        {
            var list = await _CRepo.GetComplianceSubjectList();
            return Ok(list);
        }

        [HttpGet("gettypelist")]
        public async Task<IActionResult> gettypelist()
        {
            var list = await _CRepo.GetComplianceTypeList();
            return Ok(list);
        }

        [HttpGet("getsapuserlist")]
        public async Task<IActionResult> getSapUsersList()
        {
            var list = await _CRepo.GetSAPUserList();
            return Ok(list);
        }

        [HttpGet("getstatuslist")]
        public async Task<IActionResult> GetComplienceStatusList()
        {
            var list = await _CRepo.GetComplienceStatusList();
            return Ok(list);
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> UploadFiles()
        {
            try
            {
                var file = Request.Form.Files[0];
                string folderName = "Upload";
                string webRootPath = "H:";
                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
                return Ok();
            }
            catch (System.Exception ex)
            {
            }
            return Ok();
        }

        [HttpPost("CreateSingleActivity")]
        public async Task<IActionResult> CreateSingleActivity(singlecomplianceviewmodel VM)
        {
            string strMessage = "incorrect inputs provided.";
            bool IsCreate = true;
            try
            {

                int IsAttchInserted = -1;
                SAPEntity.Instance.InitializeCompany();
                if (SAPEntity.Company.Connected)
                {
                    ActivitiesService oActSrv = (ActivitiesService)SAPEntity.Company.GetCompanyService().GetBusinessService(ServiceTypes.ActivitiesService);
                    Activity oAct = (Activity)oActSrv.GetDataInterface(ActivitiesServiceDataInterfaces.asActivity);
                    ActivityParams oParams;
                    if (VM.clgcode > 0)
                    {
                        IsCreate = false;
                        oParams = (ActivityParams)oActSrv.GetDataInterface(ActivitiesServiceDataInterfaces.asActivityParams);
                        oParams.ActivityCode = VM.clgcode;
                        oAct = oActSrv.GetActivity(oParams);

                    }
                    oAct.CardCode = VM.cardcode;
                    if (VM.cntctcode > 0)
                        oAct.ContactPersonCode = VM.cntctcode;
                   
                    oAct.Activity = BoActivities.cn_Task;
                    oAct.ActivityType = VM.type;
                    oAct.Subject = VM.subject;
                    if (VM.assignesto.HasValue && VM.assignesto.Value > 0)
                        oAct.HandledBy = VM.assignesto.Value;
                    oAct.Status = VM.status;
                    oAct.Priority = VM.prcode == "0" ? BoMsgPriorities.pr_Low : VM.prcode == "1" ? BoMsgPriorities.pr_Normal : BoMsgPriorities.pr_High;
                    oAct.Details = VM.details;
                    oAct.Notes = VM.notes;
                    if (!string.IsNullOrEmpty(VM.str_startdate))
                    {
                        DateTime StartDate = DateTime.ParseExact(VM.str_startdate, "dd/MM/yyyy", null);
                        DateTime CloseDate = DateTime.ParseExact(VM.str_enddate, "dd/MM/yyyy", null);
                        oAct.ActivityDate = StartDate;
                        oAct.Duration = (CloseDate - StartDate).TotalDays;
                        oAct.DurationType = BoDurations.du_Days;
                    }
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

                    if (IsCreate)
                    {
                        oAct.UserFields.Item("U_AddedBy").Value = User.Identity.Name;
                        oParams = oActSrv.AddActivity(oAct);
                        long singleActCode = oParams.ActivityCode;
                        if (singleActCode > 0)
                            return StatusCode(201);

                        else
                            strMessage = SAPEntity.Company.GetLastErrorDescription();
                    }
                    else
                    {
                        oAct.UserFields.Item("U_UpdatedBy").Value = User.Identity.Name;
                        oActSrv.UpdateActivity(oAct);
                        strMessage = SAPEntity.Company.GetLastErrorDescription();
                        if (!string.IsNullOrWhiteSpace(strMessage))
                            return BadRequest(strMessage);
                        else
                            return StatusCode(201);
                    }
                }
            }
            catch (Exception E)
            {
                var tt = E.Message;
            }

            return BadRequest(strMessage);
        }

        [HttpGet("DownloadAttachment")]
        public  /*async Task<FileStream> */  ActionResult DownloadAttachment()
        {

            ATCVM VM2 = new ATCVM();
            VM2.absEntry = 6;
            VM2.line = 1;
            var attFile = _CRepo.GetSingleAttachment(VM2.absEntry, VM2.line);
            //string FilePath = attFile.trgtPath + "\\" + attFile.FileName;
            //var file = Path.Combine(Path.Combine(FilePath, "attachments"), attFile.FileName);
            //return new FileStream(file, FileMode.Open, FileAccess.Read);
            string FilePath = attFile.trgtPath + "\\" + attFile.FileName + "." + attFile.FileExt;
            return File(FilePath, "." + attFile.FileExt, attFile.FileName + "." + attFile.FileExt);
        }

        [HttpPost("DownloadFile")]
        public async Task<FileStream> DownloadFile(ATCVM VM2)
        {
            var attFile = _CRepo.GetSingleAttachment(VM2.absEntry, VM2.line);

            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            currentDirectory = attFile.trgtPath;
            var file = Path.Combine(Path.Combine(currentDirectory), attFile.FileName);
            return new FileStream(file, FileMode.Open, FileAccess.Read);
        }
    }
}
