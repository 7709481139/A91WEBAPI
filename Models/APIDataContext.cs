
using A91WEBAPI.DTOs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace A91WEBAPI.Models
{
    public class APIDataContext : IdentityDbContext<AspNetUsers>//<ApplicationUser, CustomRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public APIDataContext(DbContextOptions<APIDataContext> options) : base(options)
        {

        }
        public DbSet<AspNetUsers> AspNetUsers { get; set; }
        //public DbSet<AspNetRoles> AspNetRoles { get; set; }

        //public DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public DbSet<OCRD> OCRD { get; set; }
        public DbSet<OCRY> OCRY { get; set; }
        public DbSet<OCST> OCST { get; set; }
        public DbSet<OCLG> OCLG { get; set; }
        public DbSet<OCRN> OCRN { get; set; }
        public DbSet<OCLS> OCLS { get; set; }

        public DbSet<OCLT> OCLT { get; set; }
        public DbSet<OUSR> OUSR { get; set; }
        public DbSet<OCLA> OCLA { get; set; }
        public DbSet<OCPR> OCPR { get; set; }
        public DbSet<ATC1> ATC1 { get; set; }
        public DbSet<CRD1> CRD1 { get; set; }
        public DbSet<CRD7> CRD7 { get; set; }
        public DbSet<OSS_MIS_Monthly> OSS_MIS_Monthly { get; set; }
        public DbSet<OSS_MIS_Quaterly> OSS_MIS_Quaterly { get; set; }
        public DbSet<OSS_MIS_Yearly> OSS_MIS_Yearly { get; set; }
        public DbSet<OSS_VotingResults> OSS_VotingResults { get; set; }
        public DbSet<OSS_ROLES_AUTH> OSS_ROLES_AUTH { get; set; }
        public DbSet<EmailLog> EmailLog { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OSS_VotingResults>()
                .HasKey(c => new { c.VotingID, c.UserId });

            modelBuilder.Entity<OSS_MIS_Monthly>()
                .HasKey(c => new { c.FY_Year, c.Period, c.Card_Code, c.GL_Code });
            modelBuilder.Entity<OSS_MIS_Quaterly>()
                .HasKey(c => new { c.FY_Year, c.Period, c.Card_Code, c.GL_Code });
            modelBuilder.Entity<OSS_MIS_Yearly>()
                .HasKey(c => new { c.FY_Year, c.Period, c.Card_Code, c.GL_Code });
            modelBuilder.Entity<OSS_ROLES_AUTH>().ToTable("@OSS_ROLES_AUTH");

            modelBuilder.Entity<EmailLog>().ToTable("@OSS_EMLG");
            modelBuilder.Entity<voting>().HasNoKey();
            //modelBuilder.Query<voting>();
            base.OnModelCreating(modelBuilder);
        }
        public List<T> ExecSQL<T>(string query)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                Database.OpenConnection();

                List<T> list = new List<T>();
                using (var result = command.ExecuteReader())
                {
                    T obj = default(T);
                    while (result.Read())
                    {
                        obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            if (!object.Equals(result[prop.Name], DBNull.Value))
                            {
                                prop.SetValue(obj, result[prop.Name], null);
                            }
                        }
                        list.Add(obj);
                    }
                }
                Database.CloseConnection();
                return list;
            }
        }

    }


}