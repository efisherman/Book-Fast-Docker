using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookFast.Files.Business.Data;
using BookFast.SeedWork;

namespace BookFast.Files.Data.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureStorageOptions>(configuration.GetSection("Data:Azure:Storage"));
            services.AddScoped<ISASTokenProvider, SASTokenProvider>();

            services.AddScoped<IFacilityProxy, FacilityProxy>();
        }
    }
}
