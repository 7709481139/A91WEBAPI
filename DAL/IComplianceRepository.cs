using A91WEBAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A91WEBAPI.DAL
{
    public interface IComplianceRepository
    {
        Task<IEnumerable<dynamic>> GetComplianceList(string CardCode);
        Task<IEnumerable<dynamic>> GetComplianceSubjectList();
        Task<IEnumerable<dynamic>> GetSAPUserList();
        Task<IEnumerable<dynamic>> GetComplienceStatusList();
        Task<IEnumerable<dynamic>> GetBPContactPerson(string CardCode);
        ATC1 GetSingleAttachment(int AbsEntry, int Line);
        Task<IEnumerable<dynamic>> GetComplianceTypeList();
    }
}
