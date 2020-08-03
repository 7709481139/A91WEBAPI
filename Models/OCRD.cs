using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace A91WEBAPI.Models
{
    public class OCPR
    {
        [Key]
        public int CntctCode { get; set; }
        public string CardCode { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }
        public string Tel1 { get; set; }
        public string Cellolar { get; set; }
        public string E_MailL { get; set; }
        public string Notes1 { get; set; }
        public string Notes2 { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

    public class OCRD
    {
        [Key]
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string CardType { get; set; }
        public short? GroupCode { get; set; }
        public string Free_Text { get; set; }
        public string Cellular { get; set; }
        public string Phone1 { get; set; }
        public string E_Mail { get; set; }
        public string frozenFor { get; set; }
        public string validFor { get; set; }
        public string Currency { get; set; }
        public string Notes { get; set; }
        public string IntrntSite { get; set; }
        public string ShipToDef { get; set; }
        public string CntctPrsn { get; set; }
    }

    public class OCLG
    {
        [Key]
        public int ClgCode { get; set; }

        public string CardCode { get; set; }

        public string Notes { get; set; }

        public DateTime CntctDate { get; set; }

        public int CntctTime { get; set; }

        public DateTime Recontact { get; set; }

        public string Closed { get; set; }

        public DateTime CloseDate { get; set; }

        public string ContactPer { get; set; }

        public string Tel { get; set; }

        public string Fax { get; set; }

        public short CntctSbjct { get; set; }

        public string Transfered { get; set; }

        public string DocType { get; set; }

        public string DocNum { get; set; }

        public string DocEntry { get; set; }

        public string Attachment { get; set; }

        public string DataSource { get; set; }

        public short? AttendUser { get; set; }

        public int? CntctCode { get; set; }

        public short UserSign { get; set; }

        public int? SlpCode { get; set; }

        public string Action { get; set; }

        public string Details { get; set; }

        public short CntctType { get; set; }

        public short Location { get; set; }

        public int? BeginTime { get; set; }

        public decimal Duration { get; set; }

        public string DurType { get; set; }

        public int? ENDTime { get; set; }

        public string Priority { get; set; }

        public string Reminder { get; set; }

        public decimal RemQty { get; set; }

        public string RemType { get; set; }

        public int? OprId { get; set; }

        public short OprLine { get; set; }

        public DateTime RemDate { get; set; }

        public short RemTime { get; set; }

        public string RemSented { get; set; }

        public short Instance { get; set; }

        public DateTime endDate { get; set; }

        public int? status { get; set; }

        public string personal { get; set; }

        public string inactive { get; set; }

        public string tentative { get; set; }

        public string street { get; set; }

        public string city { get; set; }

        public string country { get; set; }

        public string state { get; set; }

        public string room { get; set; }

        public string parentType { get; set; }

        public int? parentId { get; set; }

        public int? prevActvty { get; set; }

        public int? AtcEntry { get; set; }

        public string RecurPat { get; set; }

        public string EndType { get; set; }

        public DateTime SeStartDat { get; set; }

        public DateTime SeEndDat { get; set; }

        public int? MaxOccur { get; set; }

        public int? Interval { get; set; }

        public string Sunday { get; set; }

        public string Monday { get; set; }

        public string Tuesday { get; set; }

        public string Wednesday { get; set; }

        public string Thursday { get; set; }

        public string Friday { get; set; }

        public string Saturday { get; set; }

        public string SubOption { get; set; }

        public int? DayInMonth { get; set; }

        public int? Month { get; set; }

        public int? DayOfWeek { get; set; }

        public int? Week { get; set; }

        public int? SeriesNum { get; set; }

        public DateTime OrigDate { get; set; }

        public string IsRemoved { get; set; }

        public DateTime LastRemind { get; set; }

        public short? AssignedBy { get; set; }

        public string AddrName { get; set; }

        public string AddrType { get; set; }

        public int? AttendEmpl { get; set; }

        public DateTime NextDate { get; set; }

        public short NextTime { get; set; }

        public int? OwnerCode { get; set; }

        public int? AttendReci { get; set; }

        public int? ActType { get; set; }

        public string LaborItem { get; set; }

        public string ResCode { get; set; }

        public string FIPROJECT { get; set; }

        public DateTime UpdateDate { get; set; }

        public int? LogInstanc { get; set; }

        public short? UserSign2 { get; set; }

        public string DPPStatus { get; set; }

        public DateTime CreateDate { get; set; }

        public string EncryptIV { get; set; }
        public string U_OpenTime { get; set; }
        public string U_Time { get; set; }
    }
    public class OCLT
    {
        [Key]
        public short Code { get; set; }
        public string Name { get; set; }
        public short UserSign { get; set; }
        public string Active { get; set; }

    }
    public class OCLS
    {
        [Key]
        public short Code { get; set; }
        public string Name { get; set; }
        public short Type { get; set; }
        public string DataSource { get; set; }
        public short UserSign { get; set; }
        public string Active { get; set; }
    }

    //country
    public class OCRY
    {
        [Key]
        public string Code { get; set; }

        public string Name { get; set; }
    }

    //state
    public class OCST
    {
        [Key]
        public string Code { get; set; }
        public string Country { get; set; }
        public string Name { get; set; }
    }

    //currency
    public class OCRN
    {
        [Key]
        public string CurrCode { get; set; }
        public string CurrName { get; set; }
    }

    public class OUSR
    {
        [Key]
        public Int16 USERID { get; set; }
        public string U_NAME { get; set; }
    }

    public class OCLA
    {
        [Key]
        public int statusID { get; set; }
        public string name { get; set; }
    }
    public class ATC1
    {
        [Key]
        public int AbsEntry { get; set; }
        public int Line { get; set; }
        public string trgtPath { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }

    }
    public class CRD1
    {
        [Key]
        public string CardCode { get; set; }
        public string Address { get; set; }

        public string AdresType { get; set; }
        public string Street { get; set; }
        public string Block { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public int LineNum { get; set; }
        public string Building { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string StreetNo { get; set; }
        public string GSTRegnNo { get; set; }
        public int GSTType { get; set; }

    }
    public class CRD7
    {
        [Key]
        public String CardCode { get; set; }
        public String Address { get; set; }
        public String AddrType { get; set; }
        public String TaxId0 { get; set; }
    }

}