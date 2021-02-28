using System.Windows;
using System.Windows.Controls;
using Common.Services;

namespace PatternBuilderLib.ViewModels
{
    /// <summary>
    /// Interaction logic for CyclesManageView.xaml
    /// </summary>
    public partial class CyclesManageView : UserControl
    {
        public CyclesManageView()
        {
            InitializeComponent();
        }

        private void PreviewClicked(object sender, RoutedEventArgs e)
        {
            var service = ServiceDirectory.Instance.GetService<IEditorService>();
            var view = new Previewer{DataContext = this.DataContext};
            service.SetContent(view, "Preview", OnPreviewWindowClosed, false);
            service.SetWidthAndHeight(600, 400);
            PreviewButton.IsEnabled = false;
        }

        private void OnPreviewWindowClosed()
        {
            PreviewButton.IsEnabled = true;
            if (!(DataContext is CyclesManageViewModel vm))
            {
                return;
            }

            vm.IsPreview = false;
        }
    }
}
