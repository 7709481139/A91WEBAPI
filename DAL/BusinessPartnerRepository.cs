using A91WEBAPI.DTOs;
using A91WEBAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A91WEBAPI.DAL
{
    public class BusinessPartnerRepository : IBusinessPartnerRepository
    {
        private readonly APIDataContext _context;
        public BusinessPartnerRepository(APIDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OCRDVM>> BPList()
        {
            var Roles = await _context.OCRD.Select(t => new OCRDVM { cardCode = t.CardCode, cardName = t.CardName}).ToListAsync();
            return Roles;
        }

        public async Task<IEnumerable<dynamic>> ClientList()
        {
            var ListData = await _context.OCRD.Select(t => new  { cardcode = t.CardCode, cardname = t.CardName , email =t.E_Mail,
                telephone = t.Phone1,
                sector =t.Notes,
                currency = t.Currency,
                Type = (t.CardType == "C" ? "Portfolio" : "Opportunity")

            }).ToListAsync();
            return ListData;
        }

        public async Task<IEnumerable<dynamic>> CurrList()
        {
            var list = await _context.OCRN.Select(t => new { currcode = t.CurrCode, currname = t.CurrName }).ToListAsync();
            return list;
        }
        public async Task<IEnumerable<dynamic>> CountryList()
        {
            var list = await _context.OCRY.Select(t => new  { code = t.Code, name = t.Name }).ToListAsync();
            return list;
        }
        public async Task<IEnumerable<dynamic>> StateList(string CountryCode)
        {
            var list = await _context.OCST.Where(t=>t.Country == CountryCode).Select(t => new { code = t.Code, name = t.Name }).ToListAsync();
            return list;
        }

        public string GetNewCardCodeForLead(string Prefix)
        {
            string[] CardCode;
            Int64 MaxNo = 0;
            string CC = "", CN = "";
            CardCode = (_context.OCRD.Where(t => (t.CardType == "L" || t.CardType=="C") && ((t.CardCode.StartsWith("L") || (t.CardCode.StartsWith("L"))))).Select(t => t.CardCode)).ToArray();
            if (CardCode == null)
            {
                return Prefix + "00001";
            }
            else
            {
                for (int i = 0; i < CardCode.Length; i++)
                {

                    string MaxCode = CardCode[i].Remove(0, 1);
                    if (!string.IsNullOrEmpty(MaxCode))
                    {
                        try
                        {
                            Int64 tempMax = Convert.ToInt64(MaxCode);
                            if (tempMax > MaxNo)
                            {
                                MaxNo = tempMax;
                            }
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                    }

                }
                CN = MaxNo.ToString();
                CC = Prefix + (((CN.Length == 4 ? "0" : CN.Length == 3 ? "00" : CN.Length == 2 ? "000" : CN.Length == 1 ? "0000" : ""))) + Convert.ToString((Convert.ToInt64(CN) + 1));
            }
            return CC;
        }

        public async Task<dynamic> GetSelectedBPDetails(string CardCode)
        {
            var BP = await (from  OCRD in _context.OCRD.Where(t=>t.CardCode == CardCode)

                              select new
                              {
                                  cardcode = OCRD.CardCode,
                                  cardname = OCRD.CardName,
                                  email = OCRD.E_Mail,
                                  website = OCRD.IntrntSite,
                                  telephone = OCRD.Phone1,
                                  sector = OCRD.Notes,
                                  currency = OCRD.Currency,
                                  notes=OCRD.Notes,
                                  free_text= OCRD.Free_Text,
                                  pan = _context.CRD7.Where(t=>t.CardCode == CardCode  && (t.Address == "" || t.Address == null)).Select(t=>t.TaxId0).FirstOrDefault(),
                                  addressDef = _context.CRD1.Where(t=>t.CardCode == OCRD.CardCode  && t.Address == OCRD.ShipToDef).FirstOrDefault(),
                                  cnDef = _context.OCPR.Where(t=>t.CardCode == OCRD.CardCode && t.Name == OCRD.CntctPrsn).FirstOrDefault(),
                                  cnSec = _context.OCPR.Where(t => t.CardCode == OCRD.CardCode && t.Name != OCRD.CntctPrsn).FirstOrDefault(),
                                  Type=(OCRD.CardType=="C"?"Portfolio": "Opportunity")
                              }).FirstOrDefaultAsync<dynamic>();
            return BP;
        }
    }
}
