using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace A91WEBAPI.DTOs
{
    public class VotingHeaderVM
    {
        [Required]
        public string cardcode { get; set; }
        [Required]
        public string remark { get; set; }
        public string str_startdate { get; set; }

        public int prevactvty { get; set; }

        public int subject { get; set; }
        public string str_enddate { get; set; }
        public string time { get; set; }
        public string opentime { get; set; }
    }

    public class MeetingVM
    {
        public string cardcode { get; set; }
        public int clgcode { get; set; }
        public int cntctcode { get; set; }
        public int subject { get; set; }
        public int? assignesto { get; set; }
        public string details { get; set; }
        public string str_startdate { get; set; }
        public string str_enddate { get; set; }
        public string time { get; set; }
        public string prcode { get; set; }
        public int status { get; set; }
        public string notes { get; set; }
        public string[] files { get; set; }
    }

    public class ATCVM
    {
        public int absEntry { get; set; }
        public int line { get; set; }
    }

    public class singlecomplianceviewmodel
    {
        public string cardcode { get; set; }
        public int clgcode { get; set; }
        public int cntctcode { get; set; }
        public int type { get; set; }
        public int subject { get; set; }
        public int? assignesto { get; set; }
        public string details { get; set; }
        public string str_startdate { get; set; }
        public string str_enddate { get; set; }
        public string prcode { get; set; }
        public int status { get; set; }
        public string notes { get; set; }
        public string[] files { get; set; }
    }

    public class singlevote
    {
        public int clgcode { get; set; }
        public string cardcode { get; set; }
        public string notes { get; set; }
        public string recontact { get; set; }
        public string closedate { get; set; }
        public int prediscussionrate { get; set; }
        public int postdiscussionrate { get; set; }
        public string prediscussionncommnets { get; set; }
        public string postdiscussioncommnets { get; set; }
        public string userid { get; set; }
    }


    public class BusinessPartnerVM
    {
        

        public string cardcode { get; set; }
        public string cardname { get; set; }
        public string Type { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public string telephone { get; set; }
        public string sector { get; set; }
        public string currency { get; set; }
        public string Notes { get; set; }

        public string temp_address { get; set; }
        public string addressType { get; set; }
        public int? lineNum { get; set; }
        
        public string adressname { get; set; }
        public string street { get; set; }
        public string landmark { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }

        public int? pcntctCode { get; set; }
        public string pkeyname { get; set; }
        public string pfname { get; set; }
        public string plname { get; set; }
        public string pmobile { get; set; }
        public string pemail { get; set; }
        public string ptel { get; set; }
        public string pposition { get; set; }
        public string sfname { get; set; }


        public int? scntctCode { get; set; }
        public string skeyname { get; set; }
        public string slname { get; set; }
        public string smobile { get; set; }
        public string semail { get; set; }
        public string stel { get; set; }
        public string sposition { get; set; }
        public string pan { get; set; }
        public string gstin { get; set; }
    }


}
