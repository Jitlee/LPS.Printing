using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Diagnostics;

namespace LPS.Printing.WPF
{
    public class AddLineCanvas : Canvas
    {
        private Point _originPoint;
        private Point _targetOriginPoint;
        private const double ELEMENT_MIN_SIZE = 10d;

        private Canvas _containerCanvas;
        private Line _line;

        public Action<UIElement> AddedAction { get; set; }

        public AddLineCanvas()
        {
            Background = Brushes.Transparent;
            this.Visibility = Visibility.Collapsed;
        }

        public void BeginAdd(Canvas containerCanvas)
        {
            _containerCanvas = containerCanvas;
            this.Visibility = Visibility.Visible;
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.CaptureMouse();

            _targetOriginPoint = e.GetPosition(_containerCanvas);
            _originPoint = e.GetPosition(this);

            this.PreviewMouseMove -= Canvas_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp -= Canvas_PreviewMouseLeftButtonUp;

            _line = new Line()
            {
                Stroke = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x7A, 0xCC)),
                StrokeThickness = 1,
                X1 = _originPoint.X,
                Y1 = _originPoint.Y,
            };
            this.Children.Add(_line);

            this.PreviewMouseMove += Canvas_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp += Canvas_PreviewMouseLeftButtonUp;
        }

        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(this);
            _line.X2 = point.X;
            _line.Y2 = point.Y;
        }

        private void Canvas_PreviewMouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            this.ReleaseMouseCapture();
            this.PreviewMouseMove -= Canvas_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp -= Canvas_PreviewMouseLeftButtonUp;
            Point point = e.GetPosition(_containerCanvas);
            this.Visibility = Visibility.Collapsed;
            this.Children.Remove(_line);
            _line.X1 = _targetOriginPoint.X;
            _line.Y2 = _targetOriginPoint.Y;
            _line.X2 = point.X;
            _line.Y2 = point.Y;
            _line.Stroke = Brushes.Black;
            _containerCanvas.Children.Add(_line);
            if (null != AddedAction)
            {
                AddedAction(_line);
            }
        }
    }
}
