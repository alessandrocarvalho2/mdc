using Microsoft.Extensions.DependencyInjection;
using Volvo.Ecash.Infrastructure.Repository;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Application.DependencyInjection
{
    public static class ConfigurationRepository
    {


        public static void ConfigureRepository(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();

        }
    }
}
