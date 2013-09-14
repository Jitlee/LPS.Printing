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
    public class AddLineCanvas : Canvas
    {
        private const double ELEMENT_MIN_SIZE = 10d;

        private _Line _addLine;

        public Action<_Line> LineAddCompletedAction { get; set; }
        public Action<_Line, Point> LinePoint1AddingAction { get; set; }
        public Func<_Line, Point, Point> LinePoint1AddedAction { get; set; }
        public Action<_Line, Point, Point> LinePoint2AddingAction { get; set; }
        public Func<_Line, Point, Point, Point> LinePoint2AddedAction { get; set; }

        private readonly Line _line = new Line()
        {
            Stroke = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x7A, 0xCC)),
            StrokeThickness = 1,
        };

        public AddLineCanvas()
        {
            Background = Brushes.Transparent;
            this.Visibility = Visibility.Collapsed;
            this.Children.Add(_line);
            this.Cursor = Cursors.Cross;
        }

        public void EndAdd()
        {
            this.Visibility = Visibility.Collapsed;

            this.PreviewMouseMove -= AddLinePoint1Canvas_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp -= AddLinePoint1Canvas_PreviewMouseLeftButtonUp;
            this.PreviewMouseMove -= AddLinePoint2Canvas_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp -= AddLinePoint2Canvas_PreviewMouseLeftButtonUp;
        }

        public void BeginAdd(_Line line)
        {
            _addLine = line;
            this.Visibility = Visibility.Visible;

            this.PreviewMouseMove -= AddLinePoint1Canvas_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp -= AddLinePoint1Canvas_PreviewMouseLeftButtonUp;
            this.PreviewMouseMove -= AddLinePoint2Canvas_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp -= AddLinePoint2Canvas_PreviewMouseLeftButtonUp;

            this.PreviewMouseMove += AddLinePoint1Canvas_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp += AddLinePoint1Canvas_PreviewMouseLeftButtonUp;
        }

        void AddLinePoint1Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(this);
            if (null != LinePoint1AddingAction)
            {
                LinePoint1AddingAction(_addLine, point);
            }
        }

        void AddLinePoint1Canvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(this);
            point = OnLinePoint1Added(point);
            _line.X1 = _line.X2 = point.X;
            _line.Y1 = _line.Y2 = point.Y;

            this.PreviewMouseMove -= AddLinePoint1Canvas_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp -= AddLinePoint1Canvas_PreviewMouseLeftButtonUp;

            this.PreviewMouseMove += AddLinePoint2Canvas_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp += AddLinePoint2Canvas_PreviewMouseLeftButtonUp;
        }

        void AddLinePoint2Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(this);
            if (null != LinePoint2AddingAction)
            {
                _line.X2 = point.X;
                _line.Y2 = point.Y;
                LinePoint2AddingAction(_addLine, point, new Point(_line.X1, _line.Y1));
            }
        }

        void AddLinePoint2Canvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(this);
            point = OnLinePoint2Added(point);
            _line.X2 = point.X;
            _line.Y2 = point.Y;

            if (null != LineAddCompletedAction)
            {
                Rect rect = new Rect(new Point(_line.X1, _line.Y1), point);
                _addLine.Left = rect.X;
                _addLine.Top = rect.Y;
                _addLine.Width = rect.Width;
                _addLine.Height = rect.Height;

                if (_addLine.Width <= _addLine.Line.StrokeThickness)
                {
                    _addLine.Width = _addLine.Line.StrokeThickness;
                }
                if (_addLine.Height <= _addLine.Line.StrokeThickness)
                {
                    _addLine.Height = _addLine.Line.StrokeThickness;
                }

                _addLine.Line.X1 = _line.X1 - rect.X;
                _addLine.Line.Y1 = _line.Y1 - rect.Y;
                _addLine.Line.X2 = _line.X2 - rect.X;
                _addLine.Line.Y2 = _line.Y2 - rect.Y;
                LineAddCompletedAction(_addLine);
            }

            _line.X1 = _line.X2 = _line.Y1 = _line.Y2 = 0d;
            EndAdd();
        }

        private Point OnLinePoint1Added(Point point)
        {
            if (null != LinePoint1AddedAction)
            {
                return LinePoint1AddedAction(_addLine, point);
            }
            return point;
        }

        private Point OnLinePoint2Added(Point point)
        {
            if (null != LinePoint2AddedAction)
            {
                return LinePoint2AddedAction(_addLine, point, new Point(_line.X1, _line.Y1));
            }
            return point;
        }
    }
}
