using Sitecore.Services.Infrastructure.Sitecore.DependencyInjection;
using System.Reflection;
using Sitecore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Sitecore.Support.EmailCampaign.Server.DependencyInjection
{
  internal class CustomServiceConfigurator : IServicesConfigurator
  {
    public void Configure(IServiceCollection serviceCollection)
    {
      Assembly[] assemblies = new Assembly[] { GetType().Assembly };
      serviceCollection.AddWebApiControllers(assemblies);
    }
  }
}