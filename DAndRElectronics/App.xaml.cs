using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DAndRElectronics.Helpers;
using DAndRElectronics.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DAndRElectronics
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ServiceDirectory.Instance.AddSingleton<IStateService>(new StateService());
            ServiceDirectory.Instance.AddSingleton<IEditorService>(new EditorService());
           
            base.OnStartup(e);
        }
    }

    
}
