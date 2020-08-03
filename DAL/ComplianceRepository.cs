using A91WEBAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A91WEBAPI.DAL
{
    public class ComplianceRepository : IComplianceRepository
    {
        private readonly APIDataContext _context;
        public ComplianceRepository(APIDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<dynamic>> GetComplianceList(string CardCode)
        {
            try
            {
                var list = await (from OCLG in _context.OCLG.Where(t => t.CardCode == CardCode && t.Action == "T" && t.CntctType == 8)
                                  join OCRD in _context.OCRD on OCLG.CardCode equals OCRD.CardCode
                                  select new
                                  { 
                                      clgcode = OCLG.ClgCode,
                                      activity = OCLG.Action,
                                      type = OCLG.CntctType,
                                      subject = OCLG.CntctSbjct,
                                      assignesto = OCLG.AttendUser,
                                      cntctcode = OCLG.CntctCode,
                                      cardcode = OCLG.CardCode,
                                      cardname = OCRD.CardName,
                                      details = OCLG.Details,
                                      notes = OCLG.Notes,
                                      status = OCLG.status,
                                      priority = OCLG.Priority,
                                      startdate = OCLG.Recontact,
                                      enddate = OCLG.endDate,
                                      atcentry = OCLG.AtcEntry,
                                      atc = OCLG.AtcEntry > 0 ? _context.ATC1.Where(t => t.AbsEntry == OCLG.AtcEntry).ToList() : null                
                                  }).ToListAsync<dynamic>();

                return list;
            }
            catch (Exception E)
            {
                var t = E.Message;
            }
            return null;
        }

        public async Task<IEnumerable<dynamic>> GetComplianceSubjectList()
        {
            var result = await _context.OCLS.Where(t=>t.Type==8).Select(t => new { code = t.Code, name = t.Name }).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<dynamic>> GetComplianceTypeList()
        {
            var result = await _context.OCLT.Where(t=>t.Code==8).Select(t => new { code = t.Code, name = t.Name }).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<dynamic>> GetSAPUserList()
        {
            var result = await _context.OUSR.Where(t => t.USERID > 5 || t.USERID==1).Select(t => new { code = t.USERID, name = t.U_NAME }).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<dynamic>> GetBPContactPerson(string CardCode)
        {
            var Result = await _context.OCPR.Where(t => t.CardCode == CardCode).Select(t => new { code = t.CntctCode, name = t.Name }).ToListAsync();
            return Result;
        }

        public async Task<IEnumerable<dynamic>> GetComplienceStatusList()
        {
            var Roles = await _context.OCLA.Select(t => new { code = t.statusID, name = t.name }).ToListAsync();
            return Roles;
        }

        public ATC1 GetSingleAttachment(int AbsEntry, int Line)
        {
            return  _context.ATC1.Where(t => t.AbsEntry == AbsEntry && t.Line == Line).FirstOrDefault();
        }

    }
}
