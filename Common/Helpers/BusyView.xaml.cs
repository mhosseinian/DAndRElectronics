
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Common.Helpers
{
    /// <summary>
    /// Interaction logic for BusyView.xaml
    /// </summary>
    public partial class BusyView : UserControl
    {
        #region Dependency properties

        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register("CancelCommand", typeof(ICommand), typeof(BusyView));

        #endregion

        #region Private variables

        private readonly DispatcherTimer _timer;

        #endregion

        #region Constructor

        public BusyView()
        {
            InitializeComponent();

            _timer = new DispatcherTimer(DispatcherPriority.ContextIdle, Dispatcher);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 180);
        }

        #endregion

        #region Private methods

        private void Start()
        {
            _timer.Tick += Timer_HandleAnimationTick;
            _timer.Start();
        }

        private void Stop()
        {
            _timer.Stop();
            _timer.Tick -= Timer_HandleAnimationTick;
        }

        private void SetPosition(Ellipse ellipse, double offset, double posOffSet, double step)
        {
            ellipse.SetValue(Canvas.LeftProperty, 50.0 + Math.Sin(offset + posOffSet * step) * 50.0);

            ellipse.SetValue(Canvas.TopProperty, 50 + Math.Cos(offset + posOffSet * step) * 50.0);
        }

        #endregion

        #region Private event handlers

        private void Timer_HandleAnimationTick(object sender, EventArgs e)
        {
            SpinnerRotate.Angle = (SpinnerRotate.Angle + 36) % 360;
        }

        private void Canvas_HandleLoaded(object sender, RoutedEventArgs e)
        {
            const double OFFSET = Math.PI;
            const double STEP = Math.PI * 2 / 10.0;

            SetPosition(C0, OFFSET, 0.0, STEP);
            SetPosition(C1, OFFSET, 1.0, STEP);
            SetPosition(C2, OFFSET, 2.0, STEP);
            SetPosition(C3, OFFSET, 3.0, STEP);
            SetPosition(C4, OFFSET, 4.0, STEP);
            SetPosition(C5, OFFSET, 5.0, STEP);
            SetPosition(C6, OFFSET, 6.0, STEP);
            SetPosition(C7, OFFSET, 7.0, STEP);
            SetPosition(C8, OFFSET, 8.0, STEP);
        }

        private void Canvas_HandleUnloaded(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void BusyView_HandleVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool isVisible = (bool)e.NewValue;

            if (isVisible)
            {
                Start();
            }
            else
            {
                Stop();
            }
        }

        #endregion
    }
}
