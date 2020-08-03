using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SAPbobsCOM;
using Microsoft.Extensions.Configuration;
namespace A91WEBAPI.Global
{
    public class SAPEntity
    {
        public static SAPbobsCOM.Company Company;
        // static holder for instance, need to use lambda to construct since constructor private
        private static readonly Lazy<SAPEntity> _instance
            = new Lazy<SAPEntity>(() => new SAPEntity());
        //public IConfiguration Configuration { get; }
        // private to prevent direct instantiation.
        private SAPEntity()
        {
            InitializeCompany();
        }
      

        // accessor for instance
        public static SAPEntity Instance
        {
            get
            {
                return _instance.Value;
            }
        }
        public void InitializeCompany()
        {
            bool isCompanyConnected = true;
            if (Company == null)
            {
                isCompanyConnected = false;
            }
            else
            {
                if (!Company.Connected)
                {
                    isCompanyConnected = false;
                }

            }
            if (!isCompanyConnected)
            {

                /* Company = new SAPbobsCOM.Company();
                 Company.language = SAPbobsCOM.BoSuppLangs.ln_English;
                 Company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2016;
                 Company.Server = "WIN-8VO0VFHMQE0";
                 Company.DbUserName = "sa";
                 Company.DbPassword = "B1Admin";
                 Company.CompanyDB = "A91_Emerging_Fund";
                 Company.UserName = "manager";
                 Company.Password = "Pass@123";*/
                IConfigurationRoot Configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

                SqlConnectionStringBuilder ConnBuilder = new SqlConnectionStringBuilder(Configuration.GetConnectionString("DefaultConnection"));
                Company = new SAPbobsCOM.Company();
                Company.Server =Configuration.GetSection("SAPConnectionString:datasource").Value;
                Company.SLDServer = Configuration.GetSection("SAPConnectionString:SLDServer").Value;
                Company.language = SAPbobsCOM.BoSuppLangs.ln_English;
                Company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2016;
                Company.DbUserName = ConnBuilder.UserID;
                Company.DbPassword = ConnBuilder.Password;
                Company.CompanyDB = ConnBuilder.InitialCatalog;
                Company.UserName = Configuration.GetSection("SAPConnectionString:B1ManagerUName").Value;
                Company.Password = Configuration.GetSection("SAPConnectionString:B1ManagerPwd").Value;


                int k = Company.Connect();
                var a = Company.GetLastErrorDescription();
                if (!string.IsNullOrEmpty(a))
                {
                }
            }
        }

        internal void InitializeCompany(string p)
        {
            InitializeCompany();
        }

    }
}
