using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A91WEBAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using A91WEBAPI.DAL;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using A91WEBAPI.Helpers;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace A91WEBAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<APIDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            ///services.AddIdentityCore<AspNetUsers, IdentityRole>().AddEntityFrameworkStores<APIDataContext>();
            ///
            services.AddIdentity<AspNetUsers, IdentityRole>(options =>
            {
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/";
            }).AddEntityFrameworkStores<APIDataContext>()
                    .AddDefaultTokenProviders();


            

            services.AddControllers().AddNewtonsoftJson( opt => {
                opt.SerializerSettings.ReferenceLoopHandling =
                Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            
            services.AddAutoMapper(typeof(Startup));
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCors();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IBusinessPartnerRepository, BusinessPartnerRepository>();
            services.AddScoped<IVotingRepository, VotingRepository>();
            services.AddScoped<IComplianceRepository, ComplianceRepository>();
            services.AddScoped<IMISRepository, MISRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                    option =>
                    {
                        option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration
                            .GetSection("AppSettings:Token").Value)),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                  {
                      builder.Run(async context =>
                      {
                          context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                          var error = context.Features.Get<IExceptionHandlerFeature>();
                          if (error != null)
                          {
                              context.Response.AddApplicationError(error.Error.Message);
                              await context.Response.WriteAsync(error.Error.Message);
                          }
                      });
                  });
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "Fallback");
            });




        }
    }
}
