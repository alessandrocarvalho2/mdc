using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volvo.Ecash.Domain.Entities;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Api.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    public static class JwtSecurityExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="tokenConfigurations"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwtSecurity(
    this IServiceCollection services,
    //SigningConfigurations signingConfigurations,
    TokenConfigurations tokenConfigurations)
        {
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                // Validates the signature of a received token
                paramsValidation.ValidateIssuerSigningKey = true;

                // Checks whether a received token is still valid
                paramsValidation.ValidateLifetime = true;

                // Tolerance time for the expiration of a token 
                // (used in case of time synchronization problems between different computers involved in the communication process)
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            // Enables the use of the token as a way to authorize access to resources for this project
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });

            return services;
        }
    }
}
