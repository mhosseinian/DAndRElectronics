﻿using System;
using System.Windows;
using Common.Services;

namespace Common.Helpers
{
    public class EditorService: IEditorService
    {
        private HelperWindow _helperWindow;
        private Action _windowClosedAction;
        private bool _isModal;
        private double _height = 640;
        private double _width = 400;

        private HelperWindow EditorWindow => _helperWindow;
               

        public void SetContent(object uiElement, string title, bool isModal = false)
        {
            _isModal = isModal;
            CreateHelperWindow();
            if (!(uiElement is FrameworkElement view))
            {
                throw new ArgumentException("UiElement is null");
            }

            EditorWindow.SetContent(view);
            EditorWindow.Title = title;
            if (_isModal)
            {
                EditorWindow.ShowDialog();
            }
            else
            {
                EditorWindow.Show();
            }
            
        }

        public void SetContent(object uiElement, string title, Action windowClosed, bool isModal = false)
        {
            _windowClosedAction = windowClosed;
            SetContent(uiElement, title, isModal);
        }

        public void SetContentWithSize(object uiElement, string title, Action windowClosed, double width, double height,
            bool isModal = false)
        {
            _height = height;
            _width = width;
            _windowClosedAction = windowClosed;
            SetContent(uiElement, title, isModal);
        }

        public void SetWidthAndHeight(double width, double height)
        {
            if (_helperWindow == null)
            {
                return;
            }

            _width = width;
            _height = height;
            _helperWindow.Width = width;
            _helperWindow.Height = height;
        }

        #region Contructors

        public EditorService()
        {
        }

        #endregion
       
        private void EditorWindowUnLoaded(object sender, RoutedEventArgs e)
        {
            EditorWindow.Unloaded -= EditorWindowUnLoaded;
            _windowClosedAction?.Invoke();
            _helperWindow = null;
        }

        private void CreateHelperWindow()
        {
            if (_helperWindow != null)
            {
                return;
            }

            var owner = Application.Current.MainWindow;
            _helperWindow = new HelperWindow {  Height = _height, Width = _width };
            _helperWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            _helperWindow.Left = owner.Left + owner.Width;
            _helperWindow.Top = owner.Top;
            _helperWindow.Owner = owner;
            _helperWindow.Unloaded += EditorWindowUnLoaded;
        }
    }
}