using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A91WEBAPI.DTOs
{
    public class MISUploadVM
    {
        public string Type { get; set; }
        public List<MISUploadLines> Lines { get; set; }
    }

    public class MISUploadLines
    {
        public int FY_Year { get; set; }
        public int Period { get; set; }
        public string Card_Code { get; set; }
        public string GL_Code { get; set; }
        public decimal Amt { get; set; }


    }
}
