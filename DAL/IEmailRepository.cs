using A91WEBAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A91WEBAPI.DAL
{
    public interface IEmailRepository
    {
        bool SaveEmailLog(long singleActCode, string CardCode, DateTime StartDate, DateTime CloseDate, string VotersEmail, string User, int prevactvty);
        Boolean Save(Email eml);
    }
}
