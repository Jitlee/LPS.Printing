using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Diagnostics;
using LPS.Printing.WPF.Controls;

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
        private const double ELEMENT_MIN_SIZE = 10d;

        private _Control _addBaseControl;

        public Action<_Control> LineAddCompletedAction { get; set; }

        public AddElementCanvas()
        {
            Background = Brushes.Transparent;
            this.Visibility = Visibility.Collapsed;
            this.Cursor = Cursors.Cross;
        }

        public void BeginAdd(_Control addBaseControl)
        {
            _addBaseControl = addBaseControl;
            this.Visibility = Visibility.Visible;
        }

        public void EndAdd()
        {
            this.Visibility = Visibility.Collapsed;
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.CaptureMouse();

            _originPoint = e.GetPosition(this);

            this.PreviewMouseMove -= Canvas_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp -= Canvas_PreviewMouseLeftButtonUp;

            _rectangle.Width = 0;
            _rectangle.Height = 0;
            Canvas.SetLeft(_rectangle, _originPoint.X);
            Canvas.SetTop(_rectangle, _originPoint.Y);

            this.Children.Add(_rectangle);
            if (null != _addBaseControl)
            {
                this.Children.Add(_addBaseControl);
            }

            this.PreviewMouseMove += Canvas_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp += Canvas_PreviewMouseLeftButtonUp;
        }

        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(this);
            Rect rect = new Rect(point, _originPoint);
            if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            {
                rect.Width = rect.Height = Math.Min(rect.Width, rect.Height);
            }
            Canvas.SetLeft(_rectangle, rect.X);
            Canvas.SetTop(_rectangle, rect.Y);
            _rectangle.SetValue(WidthProperty, rect.Width);
            _rectangle.SetValue(HeightProperty, rect.Height);

            if (null != _addBaseControl)
            {
                Canvas.SetLeft(_addBaseControl, rect.X);
                Canvas.SetTop(_addBaseControl, rect.Y);
                _addBaseControl.SetValue(WidthProperty, rect.Width);
                _addBaseControl.SetValue(HeightProperty, rect.Height);
            }
        }

        private void Canvas_PreviewMouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            this.ReleaseMouseCapture();
            this.PreviewMouseMove -= Canvas_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp -= Canvas_PreviewMouseLeftButtonUp;
            this.Visibility = Visibility.Collapsed;

            this.Children.Remove(_rectangle);

            if (null != _addBaseControl)
            {
                this.Children.Remove(_addBaseControl);

                if (_addBaseControl.Width > ELEMENT_MIN_SIZE && _addBaseControl.Height > ELEMENT_MIN_SIZE)
                {
                    if (null != LineAddCompletedAction)
                    {
                        LineAddCompletedAction(_addBaseControl);
                    }
                }
            }

        }
    }
}
