using System;
using System.Collections;
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
