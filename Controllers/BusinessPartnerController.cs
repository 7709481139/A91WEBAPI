using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A91WEBAPI.DAL;
using A91WEBAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SAPbobsCOM;
using A91WEBAPI.Global;

using Microsoft.AspNetCore.Mvc;
using System.Globalization;


namespace A91WEBAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessPartnerController : ControllerBase
    {
        private readonly IBusinessPartnerRepository _BPrepo;
        public BusinessPartnerController(IBusinessPartnerRepository BPrepo)
        {
            _BPrepo = BPrepo;
        }

        [HttpGet("getclientlist")]
        public async Task<IActionResult> GetClientList()
        {
            var list = await _BPrepo.ClientList();
            return Ok(list);
        }

        [HttpGet("getcurrlist")]
        public async Task<IActionResult> GetCurrList()
        {
            var list = await _BPrepo.CurrList();
            return Ok(list);
        }

        [HttpGet("getcountrylist")]
        public async Task<IActionResult> GetCountryList()
        {
            var list = await _BPrepo.CountryList();
            return Ok(list);
        }

        [HttpGet("getstatelist/{id}")]
        public async Task<IActionResult> GetStateList(string id)
        {
            

            var list = await _BPrepo.StateList(id);
            return Ok(list);
        }

        [HttpPost("postbpmaster")]
        public async Task<IActionResult> PostBPMaster(BusinessPartnerVM VM)
        {
            bool IsCreate = true, IsRecordFound = false;

            SAPEntity.Instance.InitializeCompany();
            if (SAPEntity.Company.Connected)
            {
                SAPbobsCOM.BusinessPartners SAPBusinessPartner = (SAPbobsCOM.BusinessPartners)SAPEntity.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);

                if (!string.IsNullOrEmpty(VM.cardcode) && SAPBusinessPartner.GetByKey(VM.cardcode))
                {
                    IsCreate = false;
                }
                else
                {
                    SAPBusinessPartner.CardCode = _BPrepo.GetNewCardCodeForLead("L");
                    SAPBusinessPartner.CardType = BoCardTypes.cLid;
                }

                if (VM.Type == "Portfolio")
                { SAPBusinessPartner.CardType = BoCardTypes.cCustomer; }

                SAPBusinessPartner.CardName = VM.cardname;
                SAPBusinessPartner.EmailAddress = VM.email;
                SAPBusinessPartner.Website = VM.website;
                SAPBusinessPartner.Phone1 = VM.telephone;
                SAPBusinessPartner.Notes = VM.sector;
                SAPBusinessPartner.Currency = VM.currency;
                SAPBusinessPartner.FreeText = VM.Notes;
                //adress
                for (var i = 0; i <= SAPBusinessPartner.Addresses.Count - 1; i++)
                {
                    IsRecordFound = false;
                    SAPBusinessPartner.Addresses.SetCurrentLine(i);
                    if (SAPBusinessPartner.Addresses.AddressName == VM.temp_address && SAPBusinessPartner.Addresses.AddressType == BoAddressType.bo_ShipTo
                        && SAPBusinessPartner.Addresses.RowNum == Convert.ToInt32(VM.lineNum) && SAPBusinessPartner.Addresses.BPCode == SAPBusinessPartner.CardCode)
                    {
                        IsRecordFound = true;
                        break;
                    }
                }
                if (!IsRecordFound)
                {

                    SAPBusinessPartner.Addresses.SetCurrentLine(SAPBusinessPartner.Addresses.Count - 1);
                    if (!string.IsNullOrEmpty(SAPBusinessPartner.Addresses.AddressName))
                        SAPBusinessPartner.Addresses.Add();
                    SAPBusinessPartner.Addresses.SetCurrentLine(SAPBusinessPartner.Addresses.Count - 1);
                }
                SAPBusinessPartner.Addresses.SetCurrentLine(SAPBusinessPartner.Addresses.Count - 1);
                SAPBusinessPartner.Addresses.AddressType = BoAddressType.bo_ShipTo;
                SAPBusinessPartner.Addresses.AddressName = VM.adressname;
                SAPBusinessPartner.Addresses.Street = VM.street;
                SAPBusinessPartner.Addresses.AddressName2 = VM.landmark;
                SAPBusinessPartner.Addresses.Country = VM.country;
                SAPBusinessPartner.Addresses.State = VM.state;
                SAPBusinessPartner.Addresses.City = VM.city;
                SAPBusinessPartner.Addresses.ZipCode = VM.zipcode;

                //GST
                if (!string.IsNullOrEmpty(VM.gstin))
                {
                    SAPBusinessPartner.Addresses.GstType = BoGSTRegnTypeEnum.gstRegularTDSISD;
                    SAPBusinessPartner.Addresses.GSTIN = VM.gstin;
                }




                //Contact person

                if (!string.IsNullOrEmpty(VM.pfname))
                {
                    IsRecordFound = false;
                    for (var i = 0; i <= SAPBusinessPartner.ContactEmployees.Count - 1; i++)
                    {
                        SAPBusinessPartner.ContactEmployees.SetCurrentLine(i);
                        if (SAPBusinessPartner.ContactEmployees.InternalCode == Convert.ToInt32(VM.pcntctCode))
                        {
                            IsRecordFound = true;
                            SAPBusinessPartner.ContactEmployees.SetCurrentLine(i);
                            break;
                        }
                    }
                    if (!IsRecordFound)
                    {

                        SAPBusinessPartner.ContactEmployees.SetCurrentLine(SAPBusinessPartner.ContactEmployees.Count - 1);
                        if (!string.IsNullOrEmpty(SAPBusinessPartner.ContactEmployees.Name))
                            SAPBusinessPartner.ContactEmployees.Add();
                        SAPBusinessPartner.ContactEmployees.SetCurrentLine(SAPBusinessPartner.ContactEmployees.Count - 1);
                    }
                    SAPBusinessPartner.ContactEmployees.Name = VM.pfname + " " + VM.plname;
                    SAPBusinessPartner.ContactEmployees.FirstName = VM.pfname;
                    SAPBusinessPartner.ContactEmployees.LastName = VM.plname;
                    SAPBusinessPartner.ContactEmployees.MobilePhone = VM.pmobile;
                    SAPBusinessPartner.ContactEmployees.E_Mail = VM.pemail;
                    SAPBusinessPartner.ContactEmployees.Phone1 = VM.ptel;
                    SAPBusinessPartner.ContactEmployees.Position = VM.pposition;
                }

                if (!string.IsNullOrEmpty(VM.sfname))
                {
                    IsRecordFound = false;
                    for (var i = 0; i <= SAPBusinessPartner.ContactEmployees.Count - 1; i++)
                    {
                        SAPBusinessPartner.ContactEmployees.SetCurrentLine(i);
                        if (SAPBusinessPartner.ContactEmployees.InternalCode == Convert.ToInt32(VM.scntctCode))
                        {
                            IsRecordFound = true;
                            SAPBusinessPartner.ContactEmployees.SetCurrentLine(i);
                            break;
                        }
                    }
                    if (!IsRecordFound)
                    {

                        SAPBusinessPartner.ContactEmployees.SetCurrentLine(SAPBusinessPartner.ContactEmployees.Count - 1);
                        if (!string.IsNullOrEmpty(SAPBusinessPartner.ContactEmployees.Name))
                            SAPBusinessPartner.ContactEmployees.Add();
                        SAPBusinessPartner.ContactEmployees.SetCurrentLine(SAPBusinessPartner.ContactEmployees.Count - 1);
                    }
                    SAPBusinessPartner.ContactEmployees.Title = "Mr/Mrs";
                    SAPBusinessPartner.ContactEmployees.Name = VM.sfname + " " + VM.slname;
                    SAPBusinessPartner.ContactEmployees.FirstName = VM.sfname;
                    SAPBusinessPartner.ContactEmployees.LastName = VM.slname;
                    SAPBusinessPartner.ContactEmployees.MobilePhone = VM.smobile;
                    SAPBusinessPartner.ContactEmployees.E_Mail = VM.semail;
                    SAPBusinessPartner.ContactEmployees.Phone1 = VM.stel;
                    SAPBusinessPartner.ContactEmployees.Position = VM.sposition;
                }


                if (!string.IsNullOrEmpty(VM.pan))
                {
                    IsRecordFound = false;
                    for (var i = 0; i <= SAPBusinessPartner.FiscalTaxID.Count - 1; i++)
                    {
                        SAPBusinessPartner.FiscalTaxID.SetCurrentLine(i);
                        if (SAPBusinessPartner.FiscalTaxID.BPCode == SAPBusinessPartner.CardCode && SAPBusinessPartner.FiscalTaxID.Address == SAPBusinessPartner.Address)
                        {
                            IsRecordFound = true;
                            break;
                        }
                    }

                    if (!IsRecordFound)
                    {
                        SAPBusinessPartner.FiscalTaxID.SetCurrentLine(SAPBusinessPartner.FiscalTaxID.Count - 1);
                        if (!string.IsNullOrEmpty(SAPBusinessPartner.FiscalTaxID.BPCode))
                            SAPBusinessPartner.FiscalTaxID.Add();
                        SAPBusinessPartner.FiscalTaxID.SetCurrentLine(SAPBusinessPartner.FiscalTaxID.Count - 1);
                    }

                    SAPBusinessPartner.FiscalTaxID.TaxId0 = VM.pan;

                }

                if (IsCreate)
                {

                    if (SAPBusinessPartner.Add() == 0)
                    {
                        string ObjCode;
                        SAPEntity.Company.GetNewObjectCode(out ObjCode);
                        return StatusCode(201);
                    }
                    else
                    {
                        string Message = SAPEntity.Company.GetLastErrorDescription();
                        return BadRequest(Message);
                    }
                }
                else
                {
                    if (SAPBusinessPartner.Update() == 0)
                    {
                        string ObjCode;
                        SAPEntity.Company.GetNewObjectCode(out ObjCode);
                        return StatusCode(201);
                    }
                    else
                    {
                        string Message = SAPEntity.Company.GetLastErrorDescription();
                        return BadRequest(Message);
                    }
                }

            }

            return BadRequest("not able to connect sap.");
        }

        [HttpGet("getSelectedBP/{id}")]
        public async Task<IActionResult> getSelectedBP(string id)
        {
            var BP = await _BPrepo.GetSelectedBPDetails(id);
            return Ok(BP);
        }
    }
}