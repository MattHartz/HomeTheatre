using System;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Practices.Unity;

namespace HomeTheatre.CompositionRoot.App_Start
{
    public class UnityHubActivator : IHubActivator
    {
        private readonly IUnityContainer _container;

        public UnityHubActivator(IUnityContainer container)
        {
            _container = container;
        }

        public IHub Create(HubDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException("descriptor");
            }

            if (descriptor.HubType == null)
            {
                return null;
            }

            return (IHub)System.Web.Mvc.DependencyResolver.Current
                    .GetService(descriptor.HubType);
        }
    }
}
