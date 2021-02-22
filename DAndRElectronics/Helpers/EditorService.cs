using System;
using System.Windows;
using DAndRElectronics.Services;
using DAndRElectronics.View;

namespace DAndRElectronics.Helpers
{
    public class EditorService: IEditorService
    {
        private bool _isLoaded;
        private HelperWindow _helperWindow;

        private HelperWindow EditorWindow => _helperWindow;
               

        public void SetContent(object uiElement, string title)
        {
            CreateHelperWindow();
            var view = uiElement as FrameworkElement;
            if (view == null)
            {
                throw new ArgumentException("UiElement is null");
            }

            EditorWindow.SetContent(view);
            EditorWindow.Title = title;
            EditorWindow.Show();
        }

        #region Contructors

        public EditorService()
        {
            CreateHelperWindow();
        }

        #endregion
        private void EditorWindowLoaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;
        }
        private void EditorWindowUnLoaded(object sender, RoutedEventArgs e)
        {
            EditorWindow.Loaded -= EditorWindowLoaded;
            EditorWindow.Unloaded -= EditorWindowUnLoaded;
            _helperWindow = null;
            _isLoaded = false;
        }

        private void CreateHelperWindow()
        {
            if (_helperWindow != null)
            {
                return;
            }

            //_helperWindow = new HelperWindow { SizeToContent = SizeToContent.WidthAndHeight, Height = 640, Width = 1000 };
            _helperWindow = new HelperWindow {  Height = 640, Width = 1000 };
            _helperWindow.Loaded += EditorWindowLoaded;
            _helperWindow.Unloaded += EditorWindowUnLoaded;
        }
    }
}