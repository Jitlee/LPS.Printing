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
    public class AddElementCanvas : Canvas
    {
        private readonly Rectangle _rectangle = new Rectangle()
        {
            Stroke = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x7A, 0xCC)),
            StrokeThickness = 1d,
            StrokeDashArray = new DoubleCollection(new double[]{ 5d, 5d }),
            IsHitTestVisible = false,
        };

        private Point _originPoint;
        private Point _targetOriginPoint;
        private const double ELEMENT_MIN_SIZE = 10d;

        private Canvas _containerCanvas;
        private UIElement _addUIElement;

        public Action<UIElement> AddedAction { get; set; }

        public AddElementCanvas()
        {
            Background = Brushes.Transparent;
            this.Visibility = Visibility.Collapsed;
        }

        public void BeginAdd(Canvas containerCanvas, UIElement addUIElement)
        {
            _containerCanvas = containerCanvas;
            _addUIElement = addUIElement;
            this.Visibility = Visibility.Visible;
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.CaptureMouse();

            _targetOriginPoint = e.GetPosition(_containerCanvas);
            _originPoint = e.GetPosition(this);

            this.PreviewMouseMove -= Canvas_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp -= Canvas_PreviewMouseLeftButtonUp;

            _rectangle.Width = 0;
            _rectangle.Height = 0;
            Canvas.SetLeft(_rectangle, _originPoint.X);
            Canvas.SetTop(_rectangle, _originPoint.Y);

            this.Children.Add(_rectangle);
            if (null != _addUIElement)
            {
                this.Children.Add(_addUIElement);
            }

            this.PreviewMouseMove += Canvas_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp += Canvas_PreviewMouseLeftButtonUp;
        }

        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(this);
            Rect rect = new Rect(point, _originPoint);
            Canvas.SetLeft(_rectangle, rect.X);
            Canvas.SetTop(_rectangle, rect.Y);
            _rectangle.SetValue(WidthProperty, rect.Width);
            _rectangle.SetValue(HeightProperty, rect.Height);

            if (null != _addUIElement)
            {
                Canvas.SetLeft(_addUIElement, rect.X);
                Canvas.SetTop(_addUIElement, rect.Y);
                _addUIElement.SetValue(WidthProperty, rect.Width);
                _addUIElement.SetValue(HeightProperty, rect.Height);
            }
        }

        private void Canvas_PreviewMouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            this.ReleaseMouseCapture();
            this.PreviewMouseMove -= Canvas_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp -= Canvas_PreviewMouseLeftButtonUp;
            this.Children.Remove(_rectangle);
            Point point = e.GetPosition(_containerCanvas);
            this.Visibility = Visibility.Collapsed;

            if (null != _addUIElement)
            {
                this.Children.Remove(_addUIElement);

                Rect rect = new Rect(_targetOriginPoint, point);
                Canvas.SetLeft(_addUIElement, rect.X);
                Canvas.SetTop(_addUIElement, rect.Y);
                _addUIElement.SetValue(WidthProperty, rect.Width);
                _addUIElement.SetValue(HeightProperty, rect.Height);

                if (rect.Width > ELEMENT_MIN_SIZE && rect.Height > ELEMENT_MIN_SIZE)
                {
                    _containerCanvas.Children.Add(_addUIElement);

                    if (null != AddedAction)
                    {
                        AddedAction(_addUIElement);
                    }
                }
            }
        }
    }
}
