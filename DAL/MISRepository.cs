using A91WEBAPI.DTOs;
using A91WEBAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A91WEBAPI.DAL
{
    public class MISRepository : IMISRepository
    {
        private readonly APIDataContext _context;
        public MISRepository(APIDataContext context)
        {
            _context = context;
        }

        public bool PostMISData(MISUploadVM VM)
        {
            try
            {
                if (VM.Type == "Monthly")
                {

                    foreach (var LN in VM.Lines)
                    {
                        OSS_MIS_Monthly OBJ = new OSS_MIS_Monthly();
                        OBJ = _context.OSS_MIS_Monthly.Where(t => t.FY_Year == LN.FY_Year && t.Period == LN.Period && t.Card_Code == LN.Card_Code && t.GL_Code == LN.GL_Code).FirstOrDefault();

                        if (OBJ != null)
                            _context.Entry<OSS_MIS_Monthly>(OBJ).State = EntityState.Detached;

                        if (OBJ == null)
                        {
                            OSS_MIS_Monthly OBJ2 = new OSS_MIS_Monthly();
                            OBJ2.FY_Year = LN.FY_Year;
                            OBJ2.Period = LN.Period;
                            OBJ2.Card_Code = LN.Card_Code;
                            OBJ2.GL_Code = LN.GL_Code;
                            OBJ2.Amt = LN.Amt;
                            _context.OSS_MIS_Monthly.Add(OBJ2);
                            _context.SaveChanges();
                        }
                        else
                        {
                            OBJ.Amt = LN.Amt;
                            _context.OSS_MIS_Monthly.Update(OBJ);
                            _context.SaveChanges();
                        }

                    }
                }
                else if (VM.Type == "Quarterly")
                {
                    foreach (var LN in VM.Lines)
                    {
                        OSS_MIS_Quaterly OBJ = new OSS_MIS_Quaterly();
                        OBJ = _context.OSS_MIS_Quaterly.Where(t => t.FY_Year == LN.FY_Year && t.Period == LN.Period && t.Card_Code == LN.Card_Code && t.GL_Code == LN.GL_Code).FirstOrDefault();

                        if (OBJ != null)
                            _context.Entry<OSS_MIS_Quaterly>(OBJ).State = EntityState.Detached;

                        if (OBJ == null)
                        {
                            OSS_MIS_Quaterly OBJ2 = new OSS_MIS_Quaterly();
                            OBJ2.FY_Year = LN.FY_Year;
                            OBJ2.Period = LN.Period;
                            OBJ2.Card_Code = LN.Card_Code;
                            OBJ2.GL_Code = LN.GL_Code;
                            OBJ2.Amt = LN.Amt;
                            _context.OSS_MIS_Quaterly.Add(OBJ2);
                            _context.SaveChanges();
                        }
                        else
                        {
                            OBJ.Amt = LN.Amt;
                            _context.OSS_MIS_Quaterly.Update(OBJ);
                            _context.SaveChanges();
                        }

                    }
                }

                else if (VM.Type == "Yearly")
                {
                    foreach (var LN in VM.Lines)
                    {
                        OSS_MIS_Yearly OBJ = new OSS_MIS_Yearly();
                        OBJ = _context.OSS_MIS_Yearly.Where(t => t.FY_Year == LN.FY_Year && t.Period == LN.Period && t.Card_Code == LN.Card_Code && t.GL_Code == LN.GL_Code).FirstOrDefault();

                        if (OBJ != null)
                            _context.Entry<OSS_MIS_Yearly>(OBJ).State = EntityState.Detached;

                        if (OBJ == null)
                        {
                            OSS_MIS_Yearly OBJ2 = new OSS_MIS_Yearly();
                            OBJ2.FY_Year = LN.FY_Year;
                            OBJ2.Period = LN.Period;
                            OBJ2.Card_Code = LN.Card_Code;
                            OBJ2.GL_Code = LN.GL_Code;
                            OBJ2.Amt = LN.Amt;
                            _context.OSS_MIS_Yearly.Add(OBJ2);
                            _context.SaveChanges();
                        }
                        else
                        {
                            OBJ.Amt = LN.Amt;
                            _context.OSS_MIS_Yearly.Update(OBJ);
                            _context.SaveChanges();
                        }
                    }
                }

            }
            catch (Exception E)
            {

                return false;
            }
            return true;
        }
    }
}
