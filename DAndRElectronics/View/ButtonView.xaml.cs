using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DAndRElectronics.View
{
    /// <summary>
    /// Interaction logic for ButtonView.xaml
    /// </summary>
    public partial class ButtonView : UserControl
    {
        public ButtonView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
            "SelectedItems", typeof(IList), typeof(ButtonView), new PropertyMetadata(default(IList)));

        public IList SelectedItems
        {
            get { return (IList) GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        private void KeyCombo_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
           // KeyCombo.IsDropDownOpen = true;
        }
    }
}
