using System.Windows;
using DAndRElectronics.ButtonViewModels;
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
            ServiceDirectory.Instance.AddSingleton<IButtonViewModelFactoryService>(new ButtonViewModelFactoryService());
           
            base.OnStartup(e);
        }
    }

    
}
