using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using EmployeeService.Contracts;
using EmployeeService.Services;
using Infrastructure.Contract;
using Infrastructure.Repositories;
using Unity;
using Unity.Wcf;

namespace EmployeeService.DIConfig
{
    public class DiServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var container = new UnityContainer();

            container.RegisterType<IEmployeeService, Service1>();

            container.RegisterType<IEmployeeRepository, EmployeeRepository>();

            return new UnityServiceHost(container, serviceType, baseAddresses);
        }
    }
}
