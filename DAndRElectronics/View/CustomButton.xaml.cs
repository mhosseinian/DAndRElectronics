using System.Windows;
using System.Windows.Controls;
using Common.Services;
using DAndRElectronics.Services;

namespace DAndRElectronics.View
{
    /// <summary>
    /// Interaction logic for CustomButton.xaml
    /// </summary>
    public partial class CustomButton : UserControl
    {
        public CustomButton()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var buttonView = new ButtonView(){DataContext = this.DataContext};
            var editorService = ServiceDirectory.Instance.GetService<IEditorService>();
            editorService.SetContent(buttonView, "Editor");
        }

        private void OnDelete(object sender, RoutedEventArgs e)
        {
            var service = ServiceDirectory.Instance.GetService<IStateService>();
            service.OnDeleteButton(DataContext);
        }
    }
}
