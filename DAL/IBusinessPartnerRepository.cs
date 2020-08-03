using A91WEBAPI.DTOs;
using A91WEBAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A91WEBAPI.DAL
{
    public interface IBusinessPartnerRepository
    {
        Task<IEnumerable<OCRDVM>> BPList();
        Task<IEnumerable<dynamic>> ClientList();
        Task<IEnumerable<dynamic>> CurrList();
        Task<IEnumerable<dynamic>> CountryList();
        Task<IEnumerable<dynamic>> StateList(string CountryCode);
        string GetNewCardCodeForLead(string Prefix);
        Task<dynamic> GetSelectedBPDetails(string CardCode);
    }
}
