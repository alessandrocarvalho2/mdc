using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.IO;
using Newtonsoft.Json;
using AutoMapper;
using System;
using Volvo.Ecash.Application.Service;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Infrastructure.Repository.Interface;
using Volvo.Ecash.Infrastructure.Repository;
using Volvo.Ecash.Infrastructure;
using Volvo.Ecash.Application.Utils.Security;
using Microsoft.IdentityModel.Tokens;
using Volvo.Ecash.Application.Utils;
using Volvo.Ecash.Infrastructure.AccessLogQueue;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Rewrite;
using Volvo.Ecash.Infrastructure.Context;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            services.AddCors();
            //services.AddApiVersioning();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Users & Logins
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            //Operations
            services.AddScoped<IOperationRepository, OperationRepository>();
            services.AddScoped<IOperationService, OperationService>();

            //Banks
            services.AddScoped<IService<Bank>, BankService>();
            services.AddScoped<IRepository<Bank>, BankRepository>();

            //BankAccounts
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();

            //DomainModel
            services.AddScoped<IDomainService, DomainService>();
            services.AddScoped<IDomainRepository, DomainRepository>();

            //Category
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository , CategoryRepository>();

            //AccountBalance
            services.AddScoped<IAccountBalanceService, AccountBalanceService>();
            services.AddScoped<IAccountBalanceRepository, AccountBalanceRepository>();

            //Transaction
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            //CashFlow
            services.AddScoped<ICashFlowService, CashFlowService>();
            services.AddScoped<ICashFlowRepository, CashFlowRepository>();

            //CashConsolidationReport
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IReportRepository, ReportRepository>();

            //LogTransactionClosed
            services.AddScoped<ILogTransactionClosedService, LogTransactionClosedService>();
            services.AddScoped<ILogTransactionClosedRepository, LogTransactionClosedRepository>();

            //Holiday
            services.AddScoped<IHolidayService, HolidayService>();
            services.AddScoped<IHolidayRepository, HolidayRepository>();

            //CashFlowDetailed
            services.AddScoped<ICashFlowDetailedService, CashFlowDetailedService>();
            services.AddScoped<ICashFlowDetailedRepository, CashFlowDetailedRepository>();

            //Conciliation
            services.AddScoped<IConciliationRepository, ConciliationRepository>();

            //Export
            services.AddScoped<IExportService, ExportService>();

            //ExcelUtils
            services.AddScoped<ExcelUtils, ExcelUtils>();

            services.AddDbContext<UserContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
            });
            services.AddDbContext<AuthContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
            });
            services.AddDbContext<BankContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
            });
            services.AddDbContext<UtilsContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
            });

            services.AddScoped<UserContext, UserContext>();
            services.AddScoped<BankContext, BankContext>();
            services.AddScoped<AuthContext, AuthContext>();
            services.AddScoped<UtilsContext, UtilsContext>();

            var signinConfigurations = new SigninConfigurations(Configuration);
            services.AddSingleton(signinConfigurations);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = signinConfigurations.Audience,
                ValidIssuer = signinConfigurations.Issuer,
                IssuerSigningKey = signinConfigurations.Key,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero,
                ValidateAudience = false,
                ValidateIssuer = false,
                AuthenticationType = "Bearer"
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.SaveToken = true;
                bearerOptions.TokenValidationParameters = tokenValidationParameters;
                bearerOptions.RequireHttpsMetadata = true;

                bearerOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                              .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                              .RequireAuthenticatedUser().Build());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Authentication API",
                        Version = "v1",
                        Description = "API resposible for user authentication",
                        Contact = new OpenApiContact
                        {
                            Name = "Volvo eCASH",
                            Email = ""
                        }
                    });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Adicione 'Bearer <token>'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                    new string[] { }
                    }
                });
                c.IgnoreObsoleteProperties();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfigurationBuilder(env);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            var option = new RewriteOptions();
            option.AddRedirect("^$", "api/ecash/swagger");
            app.UseRewriter(option);

            app.UseHttpsRedirection();

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API ECASH");
                c.RoutePrefix = "api/ecash/swagger";
            });

        }

        private static void ConfigurationBuilder(IWebHostEnvironment env)
        {

            new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
