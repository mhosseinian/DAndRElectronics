﻿using System;
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

namespace PatternEditor.ViewModels
{
    /// <summary>
    /// Interaction logic for DeviceView.xaml
    /// </summary>
    public partial class DeviceView : UserControl
    {
        public DeviceView()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
            {
                return;
            }
            DropDownPopup.IsOpen = true;
        }



        private void ColorButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
            {
                return;
            }
            var color = ((SolidColorBrush)button.Background).Color;

            Device.Fill = button.Background;
            DropDownPopup.IsOpen = false;
        }



        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
        }

        private void MainButton_OnLostFocus(object sender, RoutedEventArgs e)
        {
        }
    }
}
