using A91WEBAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A91WEBAPI.DAL
{
    public interface IMISRepository
    {
       bool PostMISData(MISUploadVM VM);
    }
}
