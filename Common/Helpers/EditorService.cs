using System;
using System.Windows;
using Common.Services;

namespace Common.Helpers
{
    public class EditorService: IEditorService
    {
        private HelperWindow _helperWindow;

        private HelperWindow EditorWindow => _helperWindow;
               

        public void SetContent(object uiElement, string title)
        {
            CreateHelperWindow();
            if (!(uiElement is FrameworkElement view))
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
        }

        #endregion
       
        private void EditorWindowUnLoaded(object sender, RoutedEventArgs e)
        {
            EditorWindow.Unloaded -= EditorWindowUnLoaded;
            _helperWindow = null;
        }

        private void CreateHelperWindow()
        {
            if (_helperWindow != null)
            {
                return;
            }

            var owner = Application.Current.MainWindow;
            _helperWindow = new HelperWindow {  Height = 640, Width = 400 };
            _helperWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            _helperWindow.Left = owner.Left + owner.Width;
            _helperWindow.Top = owner.Top;
            _helperWindow.Owner = owner;
            _helperWindow.Unloaded += EditorWindowUnLoaded;
        }
    }
}