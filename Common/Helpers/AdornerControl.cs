using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;

namespace Common.Helpers
{
    public class AdornerControl : ContentControl
    {
        #region Dependency properties

        public FrameworkElement AdorningContent
        {
            get { return (FrameworkElement)GetValue(AdorningContentProperty); }
            set { SetValue(AdorningContentProperty, value); }
        }

        public static readonly DependencyProperty AdorningContentProperty =
            DependencyProperty.Register("AdorningContent", typeof(FrameworkElement), typeof(AdornerControl));

        public bool IsAdorning
        {
            get { return (bool)GetValue(IsAdorningProperty); }
            set { SetValue(IsAdorningProperty, value); }
        }

        public static readonly DependencyProperty IsAdorningProperty =
            DependencyProperty.Register("IsAdorning", typeof(bool), typeof(AdornerControl));

        #endregion

        #region Private variables

        private FrameworkElementAdorner _adorner = null;
        private AdornerLayer _adornerLayer = null;

        #endregion

        #region Protected methods

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == AdornerControl.IsAdorningProperty)
            {
                if (e.NewValue != null && e.NewValue is bool)
                {
                    UpdateAdorningContent();
                }
            }
            else if (e.Property == AdornerControl.AdorningContentProperty)
            {
                if (e.NewValue != null)
                {
                    UpdateAdorningContent();
                }
            }

            base.OnPropertyChanged(e);
        }

        #endregion

        #region Private methods

        private void HideAdornerContent()
        {
            if (_adornerLayer != null && _adorner != null)
            {
                _adornerLayer.Remove(_adorner);
                _adorner.DisconnectChild();

                _adorner = null;
                _adornerLayer = null;
            }
        }
        private void ShowAdornerContent()
        {
            if (AdorningContent != null)
            {
                if (_adornerLayer == null)
                    _adornerLayer = AdornerLayer.GetAdornerLayer(this);

                if (_adornerLayer != null)
                {
                    _adorner = new FrameworkElementAdorner(AdorningContent, this, 0, 0, 0, 0);

                    _adornerLayer.Add(_adorner);

                    AdorningContent.DataContext = DataContext;
                }
            }
        }
        private void UpdateAdorningContent()
        {
            if (IsAdorning)
            {
                ShowAdornerContent();
            }
            else
            {
                HideAdornerContent();
            }
        }

        #endregion
    }
}