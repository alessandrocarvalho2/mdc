using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Infrastructure;
using Volvo.Ecash.Application.Service;

namespace Volvo.Ecash.Application.DependencyInjection
{
    public static class ConfigurationService
    {
        public static void ConfigureService(IServiceCollection services, IConfiguration configuration)
        {            
            services.AddScoped<IUserService, UserService>();           

            services.AddDbContext<AuthContext>(options => options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]));
        }
    }
}
