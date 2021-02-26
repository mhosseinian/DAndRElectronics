using System;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Services
{
    public class ServiceDirectory : ServiceCollection
    {
        public static ServiceDirectory _instance;

        public static ServiceDirectory Instance
        {
            get { return _instance ??= new ServiceDirectory(); }
        }

        private IServiceProvider _serviceProvider;

        public IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    _serviceProvider = this.BuildServiceProvider();
                }

                return _serviceProvider;
            }
        }

        public T GetService<T>() where T : class
        {
            return ServiceProvider.GetService(typeof(T)) as T;
        }

    }
}