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

    public class VotingRepository : IVotingRepository
    {
        private readonly APIDataContext _context;


        public VotingRepository(APIDataContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<oclgvm>> ActivitiList(string userid, DateTime TodayDate)
        {


            DateTime TodayDate2 = DateTime.Now;
            var activitis = await (from OCLG in _context.OCLG.Where(t => t.CntctType == 1 && (t.Recontact <= TodayDate && t.endDate >= TodayDate))

                                   join OCRD in _context.OCRD on OCLG.CardCode equals OCRD.CardCode
                                   join VT in _context.OSS_VotingResults.Where(t => t.UserId == userid) on OCLG.ClgCode equals VT.VotingID into GRP
                                   from G in GRP.DefaultIfEmpty()
                                   select new oclgvm
                                   {
                                       clgcode = OCLG.ClgCode,
                                       cardcode = OCLG.CardCode,
                                       cardname = OCRD.CardName,
                                       recontact = OCLG.Recontact.ToString("dd/MM/yyyy"),
                                       closedate = OCLG.endDate.ToString("dd/MM/yyyy"),
                                       opentime = OCLG.U_OpenTime,
                                       time = OCLG.U_Time,
                                       notes = OCLG.Notes,
                                       prediscussionrate = G == null ? 0 : G.prediscussionrate,
                                       postdiscussionrate = G == null ? 0 : G.postdiscussionrate,
                                       prediscussionncommnets = G == null ? "" : G.prediscussionncommnets,
                                       postdiscussioncommnets = G == null ? "" : G.postdiscussioncommnets,
                                       // IsPreEdit = TodayDate2 < (new DateTime(OCLG.endDate.Year, OCLG.endDate.Month, OCLG.endDate.Day, Convert.ToInt32(OCLG.U_Time), 0, 0)) ? true : false
                                   }).OrderByDescending(t => t.clgcode).ToListAsync();
            foreach (var a in activitis)
            {
                try
                {
                    DateTime CloseDate = DateTime.ParseExact(a.closedate + " " + "10:00", "dd/MM/yyyy HH:mm", null);
                    a.ispreedit = TodayDate2 < CloseDate ? true : false;

                    DateTime postCloseDate = DateTime.ParseExact(a.closedate + " " + "18:00", "dd/MM/yyyy HH:mm", null);
                    a.ispostedit = (TodayDate2 >= CloseDate && TodayDate2 <= postCloseDate) ? true : false;
                }
                catch (Exception e) { }

            }
            activitis = activitis.Where(t=>t.ispreedit==true || t.ispostedit==true).ToList();

            return activitis;

        }

        public async Task<dynamic> Activity(int id)
        {

            var activitis = await (from OCLG in _context.OCLG.Where(t => t.ClgCode == id)
                                   join OCRD in _context.OCRD on OCLG.CardCode equals OCRD.CardCode
                                   select new
                                   {
                                       clgcode = OCLG.ClgCode,
                                       cardcode = OCLG.CardCode,
                                       cardname = OCRD.CardName,
                                       recontact = OCLG.Recontact.ToString("dd/MM/yyyy"),
                                       closedate = OCLG.endDate.ToString("dd/MM/yyyy"),
                                       notes = OCLG.Notes
                                   }).FirstOrDefaultAsync();
            return activitis;

        }

        public async Task<bool> PostSingleVote(singlevote VM)
        {
            try
            {
                OSS_VotingResults OBJ = new OSS_VotingResults();

                OBJ = _context.OSS_VotingResults.Where(t => t.VotingID == VM.clgcode && t.UserId == VM.userid).FirstOrDefault();
                if (OBJ == null)
                {
                    OBJ = new OSS_VotingResults();
                    OBJ.VotingID = VM.clgcode;
                    OBJ.UserId = VM.userid;
                    OBJ.Reason = VM.notes;

                    OBJ.prediscussionrate = VM.prediscussionrate;
                    OBJ.postdiscussionrate = VM.postdiscussionrate;
                    OBJ.prediscussionncommnets = VM.prediscussionncommnets;
                    OBJ.postdiscussioncommnets = VM.postdiscussioncommnets;

                    OBJ.CreatedBy = VM.userid;
                    OBJ.CreatedDateTS = DateTime.Now;

                    await _context.OSS_VotingResults.AddAsync(OBJ);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    OBJ.prediscussionrate = VM.prediscussionrate;
                    OBJ.postdiscussionrate = VM.postdiscussionrate;
                    OBJ.prediscussionncommnets = VM.prediscussionncommnets;
                    OBJ.postdiscussioncommnets = VM.postdiscussioncommnets;


                    OBJ.UpdateDateTS = DateTime.Now;
                    _context.OSS_VotingResults.Update(OBJ);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception E)
            {

                return false;
            }
        }
        /*
        public async Task<List<oclgvm>> GetVotingResult1(string username, bool IsAdmin, string fromdate, string todate, string status)
        {
           
           

            List<int> VotingIdList = new List<int>();
            if (!IsAdmin)
            {
                VotingIdList = _context.OSS_VotingResults.Where(t => t.UserId.ToUpper() == username.ToUpper()).Select(t => t.VotingID).ToList();
            }

            var activities = await (from OCLG in _context.OCLG.Where(t => t.Action == "T" && t.CntctType == 8)
                                    join OCRD in _context.OCRD on OCLG.CardCode equals OCRD.CardCode                                    
                                    where (VotingIdList.Contains(OCLG.ClgCode) || IsAdmin == true)

                                    select new oclgvm
                                    {
                                        clgcode = OCLG.ClgCode,
                                        cardcode = OCLG.CardCode,
                                        cardname = OCRD.CardName,
                                        createdate = OCLG.CreateDate,
                                        recontact = OCLG.Recontact.ToString("dd/MM/yyyy"),
                                        CntctTime = OCLG.CntctTime.ToString("00:00"),
                                        enddate = OCLG.endDate.ToString("dd/MM/yyyy"),
                                        U_Time = OCLG.U_Time,
                                        status = "",
                                        userid = _context.OSS_VotingResults.Where(t => t.VotingID == OCLG.ClgCode).Select(t => t.UserId).ToArray(),
                                        pre_avg = _context.OSS_VotingResults.Where(t => t.VotingID == OCLG.ClgCode).Average(t => t.prediscussionrate),
                                        post_avg = _context.OSS_VotingResults.Where(t => t.VotingID == OCLG.ClgCode).Average(t => t.postdiscussionrate),

                                        mpre_avg = _context.OSS_VotingResults.Where(t => t.VotingID == OCLG.ClgCode).Average(t => t.prediscussionrate),
                                        mpost_avg = _context.OSS_VotingResults.Where(t => t.VotingID == OCLG.ClgCode).Average(t => t.postdiscussionrate),
                                        stage = _context.OCLS.Where(t => t.Code == OCLG.CntctSbjct).Select(t => t.Name).FirstOrDefault(),
                                        VoteLines = (
                                        from  OCLG2 in _context.OCLG.Where(t=>t.Action=="T" && t.CntctType==1 && t.prevActvty==OCLG.ClgCode)
                                        join OSS_VR in _context.OSS_VotingResults on OCLG2.ClgCode equals OSS_VR.VotingID
                                        where ((OSS_VR.UserId.ToUpper() == username.ToUpper()) || IsAdmin == true)
                                        select new
                                        {
                                            user = OSS_VR.UserId,
                                            pre_rate = OSS_VR.prediscussionrate,
                                            post_rate = OSS_VR.postdiscussionrate,
                                            pre_note = OSS_VR.prediscussionncommnets,
                                            post_note = OSS_VR.postdiscussioncommnets,                                           
                                            status =_context.OCLA.Where(t=>t.statusID==OCLG2.status).Select(t=>t.name).FirstOrDefault()
                                        }).ToList()

                                    }).ToListAsync<oclgvm>();


            DateTime? FromDate, ToDate;
            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
            {
                FromDate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", null).Date;
                ToDate = DateTime.ParseExact(todate, "dd/MM/yyyy", null).Date;
                if (activities.Count() > 0)
                {
                    activities = activities.Where(u => (u.createdate >= FromDate.Value && u.createdate <= ToDate.Value)).ToList<oclgvm>();
                }
            }
            foreach (var a in activities)
            {
                try
                {
                    DateTime StartDate = DateTime.ParseExact(a.recontact + " " + a.CntctTime, "dd/MM/yyyy HH:mm", null);
                    DateTime CloseDate = DateTime.ParseExact(a.enddate + " " + a.U_Time, "dd/MM/yyyy HH:mm", null);
                    a.status = (DateTime.Now <= CloseDate && DateTime.Now >= StartDate) ? "Open" : "Closed";
                }
                catch (Exception E)
                { }
            }
            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                activities = activities.Where(u => (u.status == status)).ToList<oclgvm>();
            }


            return activities;

        }
        */
        public async Task<List<votingVM>> GetVotingResult(string username, bool IsAdmin, string fromdate, string todate, string status)
        {
            List<voting> votingresult = new List<voting>();
            List<votinglines> votinglinesresult = new List<votinglines>();

            DateTime? FromDate, ToDate;
            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
            {
                FromDate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", null).Date;
                ToDate = DateTime.ParseExact(todate, "dd/MM/yyyy", null).Date;
                fromdate = FromDate.Value.ToString("yyyyMMdd");
                todate = ToDate.Value.ToString("yyyyMMdd");
            }
            else { fromdate = ""; todate = ""; }
            if (IsAdmin)
            { username = ""; }
            if (string.IsNullOrEmpty(status))
            { status = "All"; }

                string HQuery = string.Format(@"select * from(select T0.ClgCode StageID ,M.CardName 'Company',(select name from OCLT where Code=T0.CntctType) Type,
(select name from OCLS where Code=T0.CntctSbjct) 'Stage',
convert(decimal(4,2),(AVG(CONVERT(decimal(4,2), v.prediscussionrate)))) 'PreAvgRate',
convert(decimal(4,2),(AVG(CONVERT(decimal(4,2), v.postdiscussionrate)))) 'PostAvgRate',
(select  convert(decimal(4,2),(sum(CONVERT(decimal(4,2), vt1.prediscussionrate))) /count(V.VotingID))  from OSS_VotingResults vt1 
join AspNetUsers U1 on vt1.UserId=U1.UserName 
join AspNetUserRoles UR1 on UR1.UserId=U1.Id
join AspNetRoles R1 on R1.Id=UR1.RoleId
where V.VotingID=vt1.VotingID and R1.Name='VOTER') 'VOTERAvgRATE',
(select convert(decimal(4,2),(sum(CONVERT(decimal(4,2), vt2.prediscussionrate))) /count(V.VotingID)) from OSS_VotingResults vt2 
join AspNetUsers U2 on vt2.UserId=U2.UserName 
join AspNetUserRoles UR2 on UR2.UserId=U2.Id
join AspNetRoles R2 on R2.Id=UR2.RoleId
where V.VotingID=vt2.VotingID and R2.Name='VOTINGMANAGER') 'VOTINGMANAGERAvgRATE',
case when (T1.Recontact<=Convert(date, getdate())  and (T1.endDate>Convert(date, getdate())
or( replace(convert(varchar, getdate(), 108) ,':','')<=180000) and T1.endDate=Convert(date, getdate()))) then 'Open' else 'Close' end 'Status'
,T1.Recontact 'StartDate',T1.endDate 'CloseDate'
from OCRD M 
join OCLG T0 on M.CardCode=T0.CardCode and (T0.createdate between'{0}' and '{1}' or '{0}'='')
join OCLG T1 on T1.prevActvty=T0.ClgCode
join OSS_VotingResults V on T1.ClgCode=V.VotingID 
join AspNetUsers U on V.UserId=U.UserName 
join AspNetUserRoles UR on UR.UserId=U.Id
join AspNetRoles R on R.Id=UR.RoleId
group by M.CardCode,M.CardName,V.VotingID,T0.ClgCode,T0.CntctType,T0.CntctSbjct,T1.Recontact,T1.endDate
) A where (A.Status='{2}' or '{2}'='All') order by A.StageID,A.Stage

", fromdate, todate,  status);


            string LQuery = string.Format(@"select * from(select T0.ClgCode StageID,M.CardCode,M.CardName,(select name from OCLT where Code=T0.CntctType) Type,
(select name from OCLS where Code=T0.CntctSbjct) 'Stage',V.UserId,R.Name 'Role',v.prediscussionrate PreRate
,v.prediscussionncommnets PreComments,v.postdiscussionrate PostRate,V.postdiscussioncommnets PostComments
from OCRD M 
join OCLG T0 on M.CardCode=T0.CardCode and (T0.createdate between'{0}' and '{1}' or '{0}'='')
join OCLG T1 on T1.prevActvty=T0.ClgCode
join OSS_VotingResults V on T1.ClgCode=V.VotingID 
join AspNetUsers U on V.UserId=U.UserName 
join AspNetUserRoles UR on UR.UserId=U.Id
join AspNetRoles R on R.Id=UR.RoleId
) A order by A.StageID,A.Stage", fromdate, todate);




            try
            {
                votingresult = _context.ExecSQL<voting>(HQuery);
                votinglinesresult = _context.ExecSQL<votinglines>(LQuery);

                var result = (from VT in votingresult
                              select new votingVM
                              {
                                  StageID = VT.StageID,
                                  Company = VT.Company,
                                  Stage = VT.Stage,
                                  Pre= VT.PreAvgRate,
                                  Post= VT.PostAvgRate,
                                  Team = VT.VOTERAvgRATE,
                                  Partner = VT.VOTINGMANAGERAvgRATE,
                                  Status = VT.Status,
                                  votinglinesVM = (from VL in votinglinesresult
                                                   where VL.StageID == VT.StageID && VT.Status=="Close"
                                                   select new votinglinesVM
                                                   {   
                                                       Stage = VL.Stage,
                                                       UserId = VL.UserId,
                                                       Role = VL.Role,
                                                       Pre= VL.PreRate,
                                                       Comments = VL.PreComments,
                                                       Post = VL.PostRate,
                                                       Comments_ = VL.PostComments
                                                   }
                                                 ).ToList()
                              }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;


        }

        public async Task<IEnumerable<dynamic>> getMeetingData(int CntctType)
        {
            var ListData = await (from OCLG in _context.OCLG.Where(t => t.Action == "T" && t.CntctType == CntctType)
                                  join OCRD in _context.OCRD on OCLG.CardCode equals OCRD.CardCode
                                  join OCLA in _context.OCLA on OCLG.status equals OCLA.statusID
                                  select new
                                  {
                                      clgcode = OCLG.ClgCode,
                                      cardcode = OCLG.CardCode,
                                      cardname = OCRD.CardName,
                                      startdate = OCLG.Recontact.ToString("dd/MM/yyyy"),
                                      enddate = OCLG.endDate.ToString("dd/MM/yyyy"),
                                      time = OCLG.U_Time,
                                      details = OCLG.Details,
                                      prio = OCLG.Priority == "1" ? "Normal" : OCLG.Priority == "2" ? "High" : "Low",
                                      status = OCLA.name,
                                  }).ToListAsync<dynamic>();
            return ListData;
        }



    }
}
