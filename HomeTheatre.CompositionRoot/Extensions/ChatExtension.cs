using HomeTheatre.CompositionRoot.App_Start;
using HomeTheatre.CompositionRoot.Hubs;
using HomeTheatre.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Practices.Unity;
using System;

namespace HomeTheatre.CompositionRoot.Extensions
{
    public class ChatExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType<IRoomManager, RoomManager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IHubActivator, UnityHubActivator>(new ContainerControlledLifetimeManager());

            Container.RegisterType<RoomHub, RoomHub>(new ContainerControlledLifetimeManager());
            Container.RegisterType<RoomHub>(new InjectionFactory(_ => CreateMyHub(Container)));
        }

        private static object CreateMyHub(IUnityContainer p)
        {
            var myHub = new RoomHub(p.Resolve<IRoomManager>());

            return myHub;
        }
    }
}
