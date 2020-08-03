using A91WEBAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A91WEBAPI.DAL
{
    public interface IVotingRepository
    {
        Task<IEnumerable<oclgvm>> ActivitiList(string userid, DateTime TodayDate);
        Task<dynamic> Activity(int id);
        Task<bool> PostSingleVote(singlevote VM);
        Task<List<votingVM>> GetVotingResult(string username, bool IsAdmin,string fromdate, string todate, string status);
        Task<IEnumerable<dynamic>> getMeetingData(int CntctType);
       
    }
}
