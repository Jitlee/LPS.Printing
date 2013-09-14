using LPS.Printing.WPF.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LPS.Printing.WPF
{
    internal class LineAdorner : ElementAdorner
    {
        private readonly _Line _line;
        private readonly static SolidColorBrush _brush = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x7A, 0xCC));
        private readonly static Pen _pen = new Pen(_brush, 2d);
        private const double HANDLE_SIZE = 5d;
        private const double ELEMENT_MIN_SIZE = 10d;
        private Point _originPoint;
        public Action<_Line, Point, Point> LineMovingAction { get; set; }
        public Func<_Line, Point, Point, Point> LineMovedAction { get; set; }
        public LineAdorner(_Line line)
            : base(line)
        {
            _line = line;
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            drawingContext.DrawEllipse(_brush, null, new Point(_line.Line.X1, _line.Line.Y1), HANDLE_SIZE, HANDLE_SIZE);
            drawingContext.DrawEllipse(_brush, null, new Point(_line.Line.X2, _line.Line.Y2), HANDLE_SIZE, HANDLE_SIZE);
        }

        protected override void OnPreviewMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            //base.OnPreviewMouseLeftButtonDown(e);

            this.CaptureMouse();

            _originPoint = e.GetPosition(_line);

            if (IsCloseTo(_originPoint, new Point(_line.Line.X1, _line.Line.Y1)))
            {

                this.PreviewMouseMove -= Point1_PreviewMouseMove;
                this.PreviewMouseLeftButtonUp -= Point1_PreviewMouseLeftButtonUp;

                this.PreviewMouseMove += Point1_PreviewMouseMove;
                this.PreviewMouseLeftButtonUp += Point1_PreviewMouseLeftButtonUp;
            }
            else if (IsCloseTo(_originPoint, new Point(_line.Line.X2, _line.Line.Y2)))
            {

                this.PreviewMouseMove -= Point1_PreviewMouseMove;
                this.PreviewMouseLeftButtonUp -= Point1_PreviewMouseLeftButtonUp;

                this.PreviewMouseMove += Point2_PreviewMouseMove;
                this.PreviewMouseLeftButtonUp += Point2_PreviewMouseLeftButtonUp;
            }
            else
            {
                this.PreviewMouseLeftButtonUp -= Move_MouseLeftButtonUp;
                this.PreviewMouseMove -= Move_PreviewMouseMove;

                this.PreviewMouseLeftButtonUp += Move_MouseLeftButtonUp;
                this.PreviewMouseMove += Move_PreviewMouseMove;
            }

            e.Handled = true;
        }

        private void Point1_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(_line);
            if ((_line.Line.X2 - point.X) * (_line.Line.X2 - point.X) + (_line.Line.Y2 - point.Y) * (_line.Line.Y2 - point.Y) > ELEMENT_MIN_SIZE * ELEMENT_MIN_SIZE)
            {
                _line.Line.X1 = point.X;
                _line.Line.Y1 = point.Y;

                UpdatePoint();

                OnLineMoving(new Point(_line.Line.X1, _line.Line.Y1), new Point(_line.Line.X2, _line.Line.Y2));
            }
        }

        private void Point1_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.PreviewMouseMove -= Point1_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp -= Point1_PreviewMouseLeftButtonUp;
            this.ReleaseMouseCapture();

            Point point = e.GetPosition(_line);
            if ((_line.Line.X2 - point.X) * (_line.Line.X2 - point.X) + (_line.Line.Y2 - point.Y) * (_line.Line.Y2 - point.Y) > ELEMENT_MIN_SIZE * ELEMENT_MIN_SIZE)
            {
                point = OnLineMoved(new Point(_line.Line.X1, _line.Line.Y1), new Point(_line.Line.X2, _line.Line.Y2));
                _line.Line.X1 = point.X;
                _line.Line.Y1 = point.Y;
                UpdatePoint();
            }
        }

        private void Point2_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(_line);
            if ((_line.Line.X1 - point.X) * (_line.Line.X1 - point.X) + (_line.Line.Y1 - point.Y) * (_line.Line.Y1 - point.Y) > ELEMENT_MIN_SIZE * ELEMENT_MIN_SIZE)
            {
                _line.Line.X2 = point.X;
                _line.Line.Y2 = point.Y;
                UpdatePoint();

                OnLineMoving(new Point(_line.Line.X2, _line.Line.Y2), new Point(_line.Line.X1, _line.Line.Y1));
            }
        }

        private void Point2_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.PreviewMouseMove -= Point2_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp -= Point2_PreviewMouseLeftButtonUp;
            this.ReleaseMouseCapture();

            Point point = e.GetPosition(_line);
            if ((_line.Line.X1 - point.X) * (_line.Line.X1 - point.X) + (_line.Line.Y1 - point.Y) * (_line.Line.Y1 - point.Y) > ELEMENT_MIN_SIZE * ELEMENT_MIN_SIZE)
            {
                point = OnLineMoved(new Point(_line.Line.X2, _line.Line.Y2), new Point(_line.Line.X1, _line.Line.Y1));
                _line.Line.X2 = point.X;
                _line.Line.Y2 = point.Y;
                UpdatePoint();
            }
        }

        private void UpdatePoint()
        {
            double left = double.IsNaN((left = Canvas.GetLeft(_line))) ? 0d : left;
            double top = double.IsNaN((top = Canvas.GetTop(_line))) ? 0d : top;

            Rect rect = new Rect(new Point(_line.Line.X1 + left, _line.Line.Y1 + top), new Point(_line.Line.X2 + left, _line.Line.Y2 + top));
            _line.Left = rect.X;
            _line.Top = rect.Y;
            _line.Width = rect.Width;
            _line.Height = rect.Height;
            if (_line.Width < _line.StrokeThickness)
            {
                _line.Width = _line.StrokeThickness;
            }
            if (_line.Height < _line.StrokeThickness)
            {
                _line.Height = _line.StrokeThickness;
            }
            _line.Line.X1 += left - rect.X;
            _line.Line.Y1 += top - rect.Y;
            _line.Line.X2 += left - rect.X;
            _line.Line.Y2 += top - rect.Y;
        }

        private void Move_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(_line);
            double offsetX = point.X - _originPoint.X;
            double offsetY = point.Y - _originPoint.Y;
            OffsetMove(offsetX, offsetY);
            OnOffset(OffsetType.Move, offsetX, offsetY);
        }

        //private void OffsetMove(double offsetX, double offsetY)
        //{
        //    double left = double.IsNaN((left = Canvas.GetLeft(_line))) ? 0d : left;
        //    double top = double.IsNaN((top = Canvas.GetTop(_line))) ? 0d : top;
        //    double targetLeft = left + offsetX;
        //    double targetTop = top + offsetY;
        //    _line.Left = targetLeft;
        //    _line.Top = targetTop;
        //}

        private void OnLineMoving(Point a, Point b)
        {
            if (null != LineMovingAction)
            {
                LineMovingAction(_line, a, b);
            }
        }

        private Point OnLineMoved(Point a, Point b)
        {
            if (null != LineMovedAction)
            {
                return LineMovedAction(_line, a, b);
            }
            return a;
        }

        private void Move_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
            this.PreviewMouseLeftButtonUp -= Move_MouseLeftButtonUp;
            this.PreviewMouseMove -= Move_PreviewMouseMove;
        }

        //private void OnOffset(OffsetType type, double offsetX, double offsetY)
        //{
        //    if (null != OffsetAction)
        //    {
        //        OffsetAction(this, type, offsetX, offsetY);
        //    }
        //}
    }
}
