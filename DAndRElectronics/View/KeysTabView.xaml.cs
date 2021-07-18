using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DAndRElectronics.ButtonViewModels;

namespace DAndRElectronics.View
{
    /// <summary>
    /// Interaction logic for KeysTabView.xaml
    /// </summary>
    public partial class KeysTabView : UserControl
    {
        public KeysTabView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
            SizeChanged +=OnSizeChanged;

        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ActualWidth < 5 || Canvas1.Children.Count == 0)
            {
                return;
            }

            SetPositions();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as KeyboardViewModel;
            SetupGrid(GridKeys, vm.KeyButtons);
        }

        private Dictionary<int, int> _keyPositions = new Dictionary<int, int>
        {
            {1, 1}, {2, 1}, {3, 1}, {4, 1}, {5, 1}, {6, 1}, {7, 1}, {8, 1},
            {9, 2}, {10, 2}, {11, 2}, {12, 2}, {13, 2}, {14, 2}, {15, 2},
            {16, 3}, {17, 3}, {18, 3}, {19, 3}, {20, 3}, {21, 3}

        };

        private Dictionary<int, double> _rowStartPosition = new Dictionary<int, double>
        {
            {1, 20.0}, {2, 20.0}, {3, 80.0}
        };

        private Dictionary<int, int> _numKeysPerRow = new Dictionary<int, int>
        {
            {1, 8}, {2, 7}, {3, 6}
        };

        private void SetupGrid(Grid grid, IEnumerable<ButtonViewModel> buttons)
        {
            if (buttons == null)
            {
                return;
            }
            Canvas1.Children.Clear();
            foreach (var buttonVm in buttons)
            {
                var buttonView = new TransparentButton() { DataContext = buttonVm, ToolTip = buttonVm.DisplayButtonName};
                Canvas1.Children.Add(buttonView);
            }
            SetPositions();
        }

        private void SetPositions()
        {
            double keyWidth = ActualWidth / 9.911949685534591;
            var keyheight = ActualHeight/5.5;
            var firstRowTopPosition = keyheight * 0.1041666666666667;
            var leftPosition = keyWidth * 0.5660377358490566;
            var horDistance = keyWidth * 0.1366459627329193;

            var num = 1;
            foreach (FrameworkElement buttonView in Canvas1.Children)
            {
                var rowNumber = _keyPositions[num];
                var keyNumber = num - 1;
                var keyTopPosition = 0.0;
                var keyLeftPosition = 0.0;
                switch (rowNumber)
                {
                    case 1:
                        keyTopPosition = firstRowTopPosition;
                        
                        keyLeftPosition = leftPosition + (keyNumber) * (keyWidth + horDistance);
                        break;
                    case 2:
                        keyTopPosition = (firstRowTopPosition) + 2 * keyheight;
                        keyTopPosition *= 0.99919;
                        keyNumber -= _numKeysPerRow[rowNumber] +1;
                        if (keyNumber > 0)
                        {
                            keyNumber++;
                        }
                        keyLeftPosition = leftPosition + (keyNumber) * (keyWidth + horDistance);
                        break;
                    case 3:
                        keyTopPosition = (firstRowTopPosition) + 4 * keyheight;
                        keyTopPosition *= 0.97029;
                        keyNumber -= _numKeysPerRow[rowNumber] + _numKeysPerRow[rowNumber - 1];
                        keyLeftPosition = leftPosition + (keyNumber) * (keyWidth + horDistance);
                        break;
                    default:
                        break;
                }
                Canvas.SetLeft(buttonView, keyLeftPosition);
                Canvas.SetTop(buttonView, keyTopPosition);

                if (keyheight > 0)
                {
                    buttonView.Height = keyheight;
                    buttonView.Width = keyWidth;
                }
                
                num++;
            }
        }

        private void Canvas1_OnMouseMove(object sender, MouseEventArgs e)
        {
            //var pos = Mouse.GetPosition(Canvas1);
            //var keyPoint = Canvas1.PointToScreen(pos);
            //Label1.Content = $"X:{pos.X}  , Y:{pos.Y}  PIXEL: X:{keyPoint.X} Y: {keyPoint.Y}";
        }

    }
}
