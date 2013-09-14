using LPS.Printing.WPF.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LPS.Printing.WPF
{
    public class AlignCanvas : Canvas
    {
        private static Point _emptyPoint = new Point(double.NaN, double.NaN);
        private const double AUTO_ALIGN_SIZE = 8d;
        public AutoAlignLine MeasureAlign(_Control source, _Control target)
        {
            if (Math.Abs(source.Left + source.Width / 2d - target.Left - target.Width / 2d) <= AUTO_ALIGN_SIZE)
            {
                return new AutoAlignLine(
                    target.Left + target.Width / 2d,
                    target.Top - target.Height * 0.1d,
                    target.Left + target.Width / 2d,
                    target.Top + target.Height * 1.1d,
                    AlignType.HorizontalCenter);
            }
            else if (Math.Abs(source.Top + source.Height / 2d - target.Top - target.Height / 2d) <= AUTO_ALIGN_SIZE)
            {
                return new AutoAlignLine(
                    target.Left - target.Width * 0.1,
                    target.Top + target.Height / 2d,
                    target.Left + target.Width * 1.1,
                    target.Top + target.Height / 2d,
                    AlignType.VerticalCenter);
            }
            else if (Math.Abs(source.Top - target.Top) <= AUTO_ALIGN_SIZE)
            {
                return new AutoAlignLine(
                    target.Left - target.Width * 0.1,
                    target.Top,
                    target.Left + target.Width * 1.1,
                    target.Top,
                    AlignType.Top);
            }
            else if (Math.Abs(source.Left - target.Left) <= AUTO_ALIGN_SIZE)
            {
                return new AutoAlignLine(
                    target.Left,
                    target.Top - target.Height * 0.1d,
                    target.Left,
                    target.Top + target.Height * 1.1,
                    AlignType.Left);
            }
            else if (Math.Abs(source.Left + source.Width - target.Left - target.Width) <= AUTO_ALIGN_SIZE)
            {
                return new AutoAlignLine(
                    target.Left + target.Width,
                    target.Top - target.Height * 0.1d,
                    target.Left + target.Width,
                    target.Top + target.Height * 1.1,
                    AlignType.Right);
            }
            else if (Math.Abs(source.Top + source.Height - target.Top - target.Height) <= AUTO_ALIGN_SIZE)
            {
                return new AutoAlignLine(
                    target.Left - target.Width * 0.1,
                    target.Top + target.Height,
                    target.Left + target.Width * 1.1,
                    target.Top + target.Height,
                    AlignType.Bottom);
            }
            else
            {
                return AutoAlignLine.Empty;
            }
        }

        private static DoubleCollection dashArray = new DoubleCollection(new double[]{ 3, 3 });
        public void DrawAlignLines(List<AutoAlignLine> alignLines)
        {
            this.Children.Clear();
            foreach (AutoAlignLine alignLine in alignLines)
            {
                this.Children.Add(new Line()
                    {
                        X1 = alignLine.X1,
                        Y1 = alignLine.Y1,
                        X2 = alignLine.X2,
                        Y2 = alignLine.Y2,
                        StrokeDashArray = dashArray,
                        Stroke = Brushes.Red,
                        StrokeThickness = 1d,
                    });
            }
        }

        public void Clear()
        {
            this.Children.Clear();
        }

        public Point MeasurePoint(Point a, Point a1, Point b1)
        {
            if (DistancePointToPoint(a, a1) <= AUTO_ALIGN_SIZE)
            {
                return a1;
            }
            if (DistancePointToPoint(a, b1) <= AUTO_ALIGN_SIZE)
            {
                return b1;
            }
            else
            {
                Point point = new Point();
                if (b1.X == a1.X)
                {
                    point.X = b1.X;
                    point.Y = a.Y;
                }
                else if (b1.Y == a1.Y)
                {
                    point.X = a.X;
                    point.Y = a1.Y;
                }
                else
                {
                    double k = (b1.Y - a1.Y) / (b1.X - a1.X);
                    point.X = ((k * b1.X + a.X / k + a.Y - b1.Y) / (1 / k + k));
                    point.Y = (-1 / k * (point.X - a.X) + a.Y);
                }
                if (new Rect(a1, b1).Contains(point) && DistancePointToPoint(point, a) <= AUTO_ALIGN_SIZE)
                {
                    return point;
                }
                else if(Math.Abs(a.X - a1.X) <= AUTO_ALIGN_SIZE)
                {
                    a.X = a1.X;
                    return a;
                }
                else if (Math.Abs(a.Y - a1.Y) <= AUTO_ALIGN_SIZE)
                {
                    a.Y = a1.Y;
                    return a;
                }
                else if (Math.Abs(a.X - b1.X) <= AUTO_ALIGN_SIZE)
                {
                    a.X = b1.X;
                    return a;
                }
                else if (Math.Abs(a.Y - b1.Y) <= AUTO_ALIGN_SIZE)
                {
                    a.Y = b1.Y;
                    return a;
                }
                return _emptyPoint;
            }
        }

        public Point MeasurePoint(Point a, Point b, Point a1, Point b1)
        {
            a = Orthogonal(a, b);
            a = MeasurePoint(a, a1, b1);
            return Orthogonal(a, b);
        }

        public Point Orthogonal(Point a, Point b)
        {
            if (Math.Abs(a.X - b.X) <= AUTO_ALIGN_SIZE)
            {
                a.X = b.X;
            }
            if (Math.Abs(a.Y - b.Y) <= AUTO_ALIGN_SIZE)
            {
                a.Y = b.Y;
            }
            return a;
        }

        private double DistancePointToPoint(Point a, Point b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        private double DistancePointToLine(Point a, Point a1, Point b1)
        {
            Vector cb = b1 - a1;
            Vector ab = a - a1;
            double d = cb * ab;
            if (d < 0) return DistancePointToPoint(a, a1);
            double e = cb * cb;
            if (d > e) return DistancePointToPoint(a, a1);
            d = d / e;
            Point f = a1 + d * cb;
            return DistancePointToPoint(a1, f);
        }

        public void DrawAlignPoints(List<Point> points)
        {
            this.Children.Clear();
            foreach (Point point in points)
            {
                {
                    Ellipse ellipse = new Ellipse()
                    {
                        Height = 10d,
                        Width = 10d,
                        Stroke = Brushes.Red,
                        StrokeThickness = 1d,
                    };
                    Canvas.SetLeft(ellipse, point.X - 5d);
                    Canvas.SetTop(ellipse, point.Y - 5d);
                    this.Children.Add(ellipse);
                }
                {
                    Ellipse ellipse = new Ellipse()
                    {
                        Height = 6d,
                        Width = 6d,
                        Fill = Brushes.Red,
                    };
                    Canvas.SetLeft(ellipse, point.X - 3d);
                    Canvas.SetTop(ellipse, point.Y - 3d);
                    this.Children.Add(ellipse);
                }
            }
        }
    }

    public struct AutoAlignLine
    {
        private double _x1;
        private double _y1;
        private double _x2;
        private double _y2;
        private AlignType _alignType;
        public double X1 { get { return _x1; } set { _x1 = value; } }
        public double Y1 { get { return _y1; } set { _y1 = value; } }
        public double X2 { get { return _x2; } set { _x2 = value; } }
        public double Y2 { get { return _y2; } set { _y2 = value; } }
        public AlignType AlignType { get { return _alignType; } set { _alignType = value; } }
        private bool _isEmpty;
        public bool IsEmpty { get { return _isEmpty; } }

        public static AutoAlignLine Empty = new AutoAlignLine() { _isEmpty= true, };

        public AutoAlignLine(double x1, double y1, double x2, double y2, AlignType alignType)
        {
            _x1 = x1;
            _y1 = y1;
            _x2 = x2;
            _y2 = y2;
            _alignType = alignType;
            _isEmpty = false;
        }
    }

    public enum AlignType
    {
        Left,
        Top,
        Right,
        Bottom,
        HorizontalCenter,
        VerticalCenter,
    }
}
