using GamesLoan.Api.Filters;
using GamesLoan.Application.IoC.Services;
using GamesLoan.Domain;
using GamesLoan.Infrastructure.DBConfiguration;
using GamesLoan.Infrastructure.IoC;
using GamesLoan.Infrastructure.IoC.ORMs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GamesLoan.Api
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
            services.AddCors();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Games Loan API",
                    Description = "This API is used with a purpose to manager the loans of your games to another people.",
                    Version = "v1"
                });
                c.OperationFilter<AddAuthHeaderOperationFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Token. Exemplo: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            #region ConfAuth
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(bearerOptions =>
                {
                    bearerOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Constants.Secret)),
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = Constants.Issuer,
                        ValidAudiences = Constants.Audiences
                    };

                    bearerOptions.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context => { return Task.CompletedTask; },
                        OnTokenValidated = context => { return Task.CompletedTask; }
                    };
                });

            // Ativa o uso do token como forma de autorizar o acesso a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
            #endregion

            services.ApplicationServicesIoC();
            services.InfrastructureORM<EntityFrameworkIoC>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationContext dataContext)
        {
            app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            ExecuteDbMigrations(dataContext);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Games Loan v1");
                c.RoutePrefix = string.Empty;
                c.DocExpansion(DocExpansion.None);
            });
        }

        private void ExecuteDbMigrations(ApplicationContext dataContext, int tentatives = 1)
        {
            Console.WriteLine($"Attempting to Migrate Database. Tentative: {tentatives}");
            var count = tentatives;
            try
            {
                dataContext.Database.Migrate();
            }
            catch (Exception)
            {
                Thread.Sleep(3000);
                count += 1;
                if (count > 10)
                    throw;
                ExecuteDbMigrations(dataContext, count);
            }
        }
    }
}
