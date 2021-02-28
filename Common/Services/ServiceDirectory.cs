using System;
using Common.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Services
{
    public class ServiceDirectory : ServiceCollection
    {
        public static ServiceDirectory _instance;

        public static ServiceDirectory Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance ??= new ServiceDirectory();
                }

                _instance = new ServiceDirectory();
                var logService = new LogService();
                _instance.AddSingleton<ILogService>(logService);
                _instance.AddSingleton<IEditorService>(new EditorService());
                logService.Info($"{System.AppDomain.CurrentDomain.FriendlyName} started");
                return _instance ??= new ServiceDirectory();
            }
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