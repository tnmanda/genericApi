using Api.Business.Implementation;
using Api.Business.Interface;
using Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace Api.Service.Services
{
    public class ServiceRegistration
    {
        public static void RegisterServices(IServiceCollection services, IConfigurationRoot config)
        {
            // Cors
            ConfigureCors(services);

            //MVC
            services.AddMvc()
               .AddJsonOptions(options =>
               {
                   options.SerializerSettings.Formatting = Formatting.Indented;
               });

            // auth
            JwtAuthorization(services, config);

            // db
            AddContexts(services, config);

        }

        public static void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder => { builder.AllowAnyOrigin(); });
                options.AddPolicy("AllowAllMethods", builder => { builder.AllowAnyMethod(); });
                options.AddPolicy("AllowAllHeaders", builder => { builder.AllowAnyHeader(); });
            });
        }

        protected static void JwtAuthorization(IServiceCollection services, IConfigurationRoot config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                string TokenKey = config["TokenValues:key"];
                string TokenIssuer = config["TokenValues:issuer"];
                string TokenAudience = config["TokenValues:audience"];

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = TokenIssuer,
                    ValidAudience = TokenAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenKey))
                };
            });
        }

        protected static void AddContexts(IServiceCollection services, IConfigurationRoot config)
        {
            //DB
            services.AddDbContext<GenContext>(options => options.UseSqlServer(config.GetConnectionString("MiMOSA_Conn")));

            //Objects
            services.AddScoped<IGenericRepository, GenericRepository>();


        }

    }
}
