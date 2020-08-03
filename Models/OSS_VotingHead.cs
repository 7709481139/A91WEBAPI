using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A91WEBAPI.Models
{
    public class OSS_VotingHead
    {
        [Key]
        public int VotingID { get; set; }

        public string CardCode { get; set; }

        public DateTime VotingStartDtTS { get; set; }

        public DateTime VotingEndDtTS { get; set; }

        public int MeetingID { get; set; }

        public string Agenda { get; set; }

        public string VotType { get; set; }

        public DateTime CreatedDateTS { get; set; }

        public DateTime UpdateDateTS { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

    }
    public class OSS_VotingResults
    {
        [Key]
        [Column(Order = 0)]
        public int VotingID { get; set; }
        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; }
        public string Reason { get; set; }
        public int prediscussionrate { get; set; }
        public int postdiscussionrate { get; set; }
        public string prediscussionncommnets { get; set; }
        public string postdiscussioncommnets { get; set; }
        public string CreatedBy { get; set; }
        
        public DateTime CreatedDateTS { get; set; }
        public DateTime? UpdateDateTS { get; set; }

    }
}
