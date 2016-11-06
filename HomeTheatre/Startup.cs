using AutoMapper;
using HomeTheatre.CompositionRoot.App_Start;
using HomeTheatre.CompositionRoot.Hubs;
using HomeTheatre.Core;
using HomeTheatre.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HomeTheatre.Startup))]
namespace HomeTheatre
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => new UnityHubActivator(UnityConfig.GetConfiguredContainer()));
            var config = new MapperConfiguration(cfg => cfg.AddProfile<DomainToDtoMapping>());
            config.CreateMapper();

            MvcApplication.MapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<DomainToDtoMapping>());
            MvcApplication.Mapper = MvcApplication.MapperConfiguration.CreateMapper();

            app.MapSignalR();
        }
    }
}
