using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Common.Services;

namespace DAndRElectronics.View
{
    /// <summary>
    /// Interaction logic for TransparentButton.xaml
    /// </summary>
    public partial class TransparentButton : UserControl
    {
        public TransparentButton()
        {
            InitializeComponent();
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var buttonView = new ButtonView() { DataContext = this.DataContext };
            var editorService = ServiceDirectory.Instance.GetService<IEditorService>();
            editorService.SetContent(buttonView, "Editor");
        }

    }
}
