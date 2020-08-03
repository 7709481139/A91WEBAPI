using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A91WEBAPI.Models
{
    public class MIS
    {
    }

    public class OSS_MIS_Monthly
    {
        public int FY_Year { get; set; }
        public int Period { get; set; }
        public string Card_Code { get; set; }
        public string GL_Code { get; set; }
        public decimal Amt { get; set; }
    }

    public class OSS_MIS_Quaterly
    {
        public int FY_Year { get; set; }
        public int Period { get; set; }
        public string Card_Code { get; set; }
        public string GL_Code { get; set; }
        public decimal Amt { get; set; }
    }

    public class OSS_MIS_Yearly
    {
        public int FY_Year { get; set; }
        public int Period { get; set; }
        public string Card_Code { get; set; }
        public string GL_Code { get; set; }
        public decimal Amt { get; set; }
    }

}
