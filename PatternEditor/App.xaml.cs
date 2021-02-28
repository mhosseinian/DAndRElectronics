using System.Windows;
using Common.Helpers;
using Common.Services;
using Microsoft.Extensions.DependencyInjection;
using PatternBuilderLib;

namespace PatternEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Contructors

        public App()
        {
            ConfigureServices();
        }

        #endregion
        public static void ConfigureServices()
        {
            ServiceDirectory.Instance.AddSingleton<IButtonSelectionService>(new ButtonSelectionService());
        }
    }
}
