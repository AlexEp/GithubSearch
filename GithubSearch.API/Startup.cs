using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using GithubSearch.API.Configs;
using GithubSearch.API.DB;
using GithubSearch.API.Identity;
using GithubSearch.DAL.Repository;
using GithubSearch.Shared.Interfaces;
using GithubSearch.Shared.Services;
using GitHubSearchEngine;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GithubSearch.API
{
    public class Startup
    {
        private string CORS_POLICY = "cors_policy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /* *** [App settings] *** */
            var appConfigSection = Configuration.GetSection("AppConfig");
            services.Configure<AppConfig>(appConfigSection);

            AppConfig appConfig = appConfigSection.Get<AppConfig>();


            // *** init DAL *** */
            services.AddTransient<IGithubRepositoriesRepository>(provider => new GithubRepositoriesRepository());


            /* *** [Authentication] *** */

            //DB Identity Context

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(appConfig.DB.IdentityConnection));

            services.AddIdentity<AppIdentityUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppIdentityDbContext>();

            /* *** [init Swagger] *** */
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GithubSearch API", Version = "v1" });
            });

            /* *** [Jwt Tokens] *** */

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.RequireHttpsMetadata = false; //No HTTPS for now
                       options.SaveToken = true;
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = false,
                           ValidateAudience = false,
                           //ValidateLifetime = true,
                           ValidateIssuerSigningKey = true,
                           //ValidIssuer = appConfig.JWT.Issuer,
                           //ValidAudience = appConfig.JWT.Audience,
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfig.JWT.SecretKey)),
                           ClockSkew = TimeSpan.Zero
                       };
                   });


            /* *** [ELSE] *** */
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
     
          
            services.AddCors(options =>
            {
                options.AddPolicy(name: CORS_POLICY,
                                  builder =>
                                  {
                                      builder.WithOrigins(appConfig.CORS.FrontEndURL)
                                        .AllowAnyMethod()
                                        .AllowAnyHeader()
                                        .AllowCredentials();
                                  });
            });

          
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new IocConfigModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }



            app.UseExceptionHandler("/error"); // Add this
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My First Swagger");
            });

            app.UseRouting();
            app.UseCors(CORS_POLICY);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Seed data
            IdentitySeedData.EnsurePopulatedAsync(app).Wait();
            //AppDataSeedData.EnsurePopulatedAsync(app).Wait();
        }

        public class IocConfigModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType<GitHubV3SearchEngine>().As<ISearchEngine>().InstancePerLifetimeScope();
                builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            }
        }
    }
}
