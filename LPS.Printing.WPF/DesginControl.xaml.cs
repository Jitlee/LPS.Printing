using LPS.Printing.WPF.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LPS.Printing.WPF
{
    /// <summary>
    /// DesginControl.xaml 的交互逻辑
    /// </summary>
    public partial class DesginControl : UserControl
    {
        public Action<List<_Control>> SelectionChangedAction { get; set; }
        public Action EndAddAction { get; set; }
        public DesginControl()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            DesginCanvas.SelectionChangedAction = OnSelectionChangedAction;
            DesginCanvas.ElementMovingAction = OnElementMoving;
            DesginCanvas.ElementMovedAction = OnElementMoved;
            DesginCanvas.LineMovingAction = OnLineMoving;
            DesginCanvas.LineMovedAction = OnLineMoved;
            AddLineCanvas.LinePoint1AddingAction = OnLinePoint1Adding;
            AddLineCanvas.LinePoint1AddedAction = OnLinePoint1Added;
            AddLineCanvas.LinePoint2AddingAction = OnLinePoint2Adding;
            AddLineCanvas.LinePoint2AddedAction = OnLinePoint2Added;

            AddElementCanvas.LineAddCompletedAction = OnAddElementCompletedAction;
            AddLineCanvas.LineAddCompletedAction = OnAddLineCompeledAction;
        }

        public void SetAutoAlign(bool align)
        {
            DesginCanvas.IsAutoAlign = align;
        }

        private void OnSelectionChangedAction(List<_Control> selectedList)
        {
            if (null != SelectionChangedAction)
            {
                SelectionChangedAction(selectedList);
            }

            if(null != EndAddAction)
            {
                EndAddAction();
            }
        }

        private void OnAddElementCompletedAction(_Control control)
        {
            DesginCanvas.AddNewControl(control);
        }

        private void OnAddLineCompeledAction(_Control control)
        {
            DesginCanvas.AddNewControl(control);
        }

        public void BeginAdd(Type controltype)
        {
            if (typeof(_Line).IsAssignableFrom(controltype))
            {
                AddLineCanvas.BeginAdd(new _Line());
            }
            else
            {
                AddElementCanvas.BeginAdd(Activator.CreateInstance(controltype, null) as _Control);
            }
            DesginCanvas.CleanSelection();
        }

        public void EndAdd()
        {
            AddElementCanvas.EndAdd();
            AddLineCanvas.EndAdd();
        }

        private void OnElementMoving(_Control element, Rect rect)
        {
            if (!DesginCanvas.IsAutoAlign)
            {
                return;
            }
            List<AutoAlignLine> alignLines = new List<AutoAlignLine>();
            foreach (UIElement ui in DesginCanvas.Children)
            {
                if (DesginCanvas.SelectedControls.Contains(ui as _Control))
                {
                    continue;
                }
                if (ui is _Control && !(ui is _Line))
                {
                    AutoAlignLine alignLine = AlignCanvas.MeasureAlign(element, ui as _Control);
                    if (!alignLine.IsEmpty)
                    {
                        alignLines.Add(alignLine);
                    }
                }
            }

            AlignCanvas.DrawAlignLines(alignLines);
        }

        private void OnElementMoved(_Control element, Rect rect)
        {
            AlignCanvas.Clear();

            if (!DesginCanvas.IsAutoAlign)
            {
                return;
            }

            foreach (UIElement ui in DesginCanvas.Children)
            {
                if (DesginCanvas.SelectedControls.Contains(ui as _Control))
                {
                    continue;
                }
                if (ui is _Control)
                {
                    AutoAlignLine alignLine = AlignCanvas.MeasureAlign(element, ui as _Control);
                    if (!alignLine.IsEmpty)
                    {
                        switch (alignLine.AlignType)
                        {
                            case AlignType.Left:
                                element.Left = alignLine.X1;
                                break;
                            case AlignType.Top:
                                element.Top = alignLine.Y1;
                                break;
                            case AlignType.Right:
                                element.Left = alignLine.X1 - element.Width;
                                break;
                            case AlignType.Bottom:
                                element.Top = alignLine.Y1 - element.Height;
                                break;
                            case AlignType.HorizontalCenter:
                                element.Left = alignLine.X1 - element.Width / 2d;
                                break;
                            case AlignType.VerticalCenter:
                                element.Top = alignLine.Y1 - element.Height / 2d;
                                break;
                        }
                        break;
                    }
                }
            }
        }

        public void OnLineMoving(_Line source, Point a, Point b)
        {
            if (!DesginCanvas.IsAutoAlign)
            {
                return;
            }
            a.X += source.Left;
            a.Y += source.Top;
            b.X += source.Left;
            b.Y += source.Top;
            LineAddingMode(source, a, b);
        }

        private void LineAddingMode(_Line source, Point a, Point b)
        {
            if (!DesginCanvas.IsAutoAlign)
            {
                return;
            }
            List<Point> points = new List<Point>();
            foreach (UIElement ui in DesginCanvas.Children)
            {
                if (DesginCanvas.SelectedControls.Contains(ui as _Control))
                {
                    continue;
                }
                if (ui is _Line)
                {
                    _Line line = ui as _Line;
                    Point point = AlignCanvas.MeasurePoint(a, b, new Point(line.Line.X1 + line.Left, line.Line.Y1 + line.Top), new Point(line.Line.X2 + line.Left, line.Line.Y2 + line.Top));
                    if (!double.IsNaN(point.X))
                    {
                        points.Add(point);
                    }
                }
                else if (ui is _Rectangle)
                {
                    _Control element = ui as _Control;
                    Point a1 = new Point(element.Left, element.Top);
                    Point b1 = new Point(element.Left + element.Width, element.Top);
                    Point c1 = new Point(element.Left + element.Width, element.Top + element.Height);
                    Point d1 = new Point(element.Left, element.Top + element.Height);
                    Point point = AlignCanvas.MeasurePoint(a, b, a1, b1);
                    if (!double.IsNaN(point.X))
                    {
                        points.Add(point);
                    }
                    else if (!double.IsNaN((point = AlignCanvas.MeasurePoint(a, b, b1, c1)).X))
                    {
                        points.Add(point);
                    }
                    else if (!double.IsNaN((point = AlignCanvas.MeasurePoint(a, b, c1, d1)).X))
                    {
                        points.Add(point);
                    }
                    else if (!double.IsNaN((point = AlignCanvas.MeasurePoint(a, b, d1, a1)).X))
                    {
                        points.Add(point);
                    }
                }
            }

            AlignCanvas.DrawAlignPoints(points);
        }

        private Point OnLineMoved(_Line source, Point point1, Point point2)
        {
            if (!DesginCanvas.IsAutoAlign)
            {
                return point1;
            }
            Point a = new Point(point1.X + source.Left, point1.Y + source.Top);
            Point b = new Point(point2.X + source.Left, point2.Y + source.Top);
            AlignCanvas.Clear();
            Point point = new Point(double.NaN, double.NaN);
            foreach (UIElement ui in DesginCanvas.Children)
            {
                if (DesginCanvas.SelectedControls.Contains(ui as _Control))
                {
                    continue;
                }
                if (ui is _Line)
                {
                    _Line line = ui as _Line;
                    point = AlignCanvas.MeasurePoint(a, b, new Point(line.Line.X1 + line.Left, line.Line.Y1 + line.Top), new Point(line.Line.X2 + line.Left, line.Line.Y2 + line.Top));
                    if (!double.IsNaN(point.X))
                    {
                        return new Point(point.X - source.Left, point.Y - source.Top);
                    }
                }
                else if (ui is _Rectangle)
                {
                    _Control element = ui as _Control;
                    Point a1 = new Point(element.Left, element.Top);
                    Point b1 = new Point(element.Left + element.Width, element.Top);
                    Point c1 = new Point(element.Left + element.Width, element.Top + element.Height);
                    Point d1 = new Point(element.Left, element.Top + element.Height);
                    point = AlignCanvas.MeasurePoint(a, b, a1, b1);
                    if (!double.IsNaN(point.X))
                    {
                        return new Point(point.X - source.Left, point.Y - source.Top);
                    }
                    else if (!double.IsNaN((point = AlignCanvas.MeasurePoint(a, b, b1, c1)).X))
                    {
                        return new Point(point.X - source.Left, point.Y - source.Top);
                    }
                    else if (!double.IsNaN((point = AlignCanvas.MeasurePoint(a, b, c1, d1)).X))
                    {
                        return new Point(point.X - source.Left, point.Y - source.Top);
                    }
                    else if (!double.IsNaN((point = AlignCanvas.MeasurePoint(a, b, d1, a1)).X))
                    {
                        return new Point(point.X - source.Left, point.Y - source.Top);
                    }
                }
            }
            
            return AlignCanvas.Orthogonal(point1, point2);
        }

        private void OnLinePoint1Adding(_Line source, Point a)
        {
            if (!DesginCanvas.IsAutoAlign)
            {
                return;
            }
            List<Point> points = new List<Point>();
            foreach (UIElement ui in DesginCanvas.Children)
            {
                if (DesginCanvas.SelectedControls.Contains(ui as _Control))
                {
                    continue;
                }
                if (ui is _Line)
                {
                    _Line line = ui as _Line;
                    Point point = AlignCanvas.MeasurePoint(a, new Point(line.Line.X1 + line.Left, line.Line.Y1 + line.Top), new Point(line.Line.X2 + line.Left, line.Line.Y2 + line.Top));
                    if (!double.IsNaN(point.X))
                    {
                        points.Add(point);
                    }
                }
                else if (ui is _Rectangle)
                {
                    _Control element = ui as _Control;
                    Point a1 = new Point(element.Left, element.Top);
                    Point b1 = new Point(element.Left + element.Width, element.Top);
                    Point c1 = new Point(element.Left + element.Width, element.Top + element.Height);
                    Point d1 = new Point(element.Left, element.Top + element.Height);
                    Point point = AlignCanvas.MeasurePoint(a, a1, b1);
                    if (!double.IsNaN(point.X))
                    {
                        points.Add(point);
                    }
                    else if (!double.IsNaN((point = AlignCanvas.MeasurePoint(a, b1, c1)).X))
                    {
                        points.Add(point);
                    }
                    else if (!double.IsNaN((point = AlignCanvas.MeasurePoint(a, c1, d1)).X))
                    {
                        points.Add(point);
                    }
                    else if (!double.IsNaN((point = AlignCanvas.MeasurePoint(a, d1, a1)).X))
                    {
                        points.Add(point);
                    }
                }
            }

            AlignCanvas.DrawAlignPoints(points);
        }

        private Point OnLinePoint1Added(_Line source, Point a)
        {
            if (!DesginCanvas.IsAutoAlign)
            {
                return a;
            }
            AlignCanvas.Clear();
            Point point = new Point(double.NaN, double.NaN);
            foreach (UIElement ui in DesginCanvas.Children)
            {
                if (DesginCanvas.SelectedControls.Contains(ui as _Control))
                {
                    continue;
                }
                if (ui is _Line)
                {
                    _Line line = ui as _Line;
                    point = AlignCanvas.MeasurePoint(a, new Point(line.Line.X1 + line.Left, line.Line.Y1 + line.Top), new Point(line.Line.X2 + line.Left, line.Line.Y2 + line.Top));
                    if (!double.IsNaN(point.X))
                    {
                        return new Point(point.X - source.Left, point.Y - source.Top);
                    }
                }
                else if (ui is _Rectangle)
                {
                    _Control element = ui as _Control;
                    Point a1 = new Point(element.Left, element.Top);
                    Point b1 = new Point(element.Left + element.Width, element.Top);
                    Point c1 = new Point(element.Left + element.Width, element.Top + element.Height);
                    Point d1 = new Point(element.Left, element.Top + element.Height);
                    point = AlignCanvas.MeasurePoint(a, a1, b1);
                    if (!double.IsNaN(point.X))
                    {
                        return point;
                    }
                    else if (!double.IsNaN((point = AlignCanvas.MeasurePoint(a, b1, c1)).X))
                    {
                        return point;
                    }
                    else if (!double.IsNaN((point = AlignCanvas.MeasurePoint(a, c1, d1)).X))
                    {
                        return point;
                    }
                    else if (!double.IsNaN((point = AlignCanvas.MeasurePoint(a, d1, a1)).X))
                    {
                        return point;
                    }
                }
            }
            return a;
        }

        private void OnLinePoint2Adding(_Line source, Point a, Point b)
        {
            if (!DesginCanvas.IsAutoAlign)
            {
                return;
            }
            LineAddingMode(source, a, b);
        }

        private Point OnLinePoint2Added(_Line source, Point a, Point b)
        {
            if (!DesginCanvas.IsAutoAlign)
            {
                return a;
            }
            AlignCanvas.Clear();
            Point point = new Point(double.NaN, double.NaN);
            foreach (UIElement ui in DesginCanvas.Children)
            {
                if (DesginCanvas.SelectedControls.Contains(ui as _Control))
                {
                    continue;
                }
                if (ui is _Line)
                {
                    _Line line = ui as _Line;
                    point = AlignCanvas.MeasurePoint(a, b, new Point(line.Line.X1 + line.Left, line.Line.Y1 + line.Top), new Point(line.Line.X2 + line.Left, line.Line.Y2 + line.Top));
                    if (!double.IsNaN(point.X))
                    {
                        return new Point(point.X - source.Left, point.Y - source.Top);
                    }
                }
                else if (ui is _Rectangle)
                {
                    _Control element = ui as _Control;
                    Point a1 = new Point(element.Left, element.Top);
                    Point b1 = new Point(element.Left + element.Width, element.Top);
                    Point c1 = new Point(element.Left + element.Width, element.Top + element.Height);
                    Point d1 = new Point(element.Left, element.Top + element.Height);
                    point = AlignCanvas.MeasurePoint(a, b, a1, b1);
                    if (!double.IsNaN(point.X))
                    {
                        return point;
                    }
                    else if (!double.IsNaN((point = AlignCanvas.MeasurePoint(a, b1, c1)).X))
                    {
                        return point;
                    }
                    else if (!double.IsNaN((point = AlignCanvas.MeasurePoint(a, c1, d1)).X))
                    {
                        return point;
                    }
                    else if (!double.IsNaN((point = AlignCanvas.MeasurePoint(a, d1, a1)).X))
                    {
                        return point;
                    }
                }
            }
            return AlignCanvas.Orthogonal(a, b);
        }

        public void SetZIndex(LayoutType type)
        {
            DesginCanvas.SetZIndex(type);
        }

        public void Delete()
        {
            DesginCanvas.Delete();
        }
    }
}
