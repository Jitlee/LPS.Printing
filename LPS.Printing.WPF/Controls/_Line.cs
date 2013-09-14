using LPS.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LPS.Printing.WPF.Controls
{
    public class _Line : _Control
    {
        private readonly Line _line = new Line() { X2 = 1d, Y2 = 1d, Stroke = Brushes.Black, StrokeThickness = 1d, };

        public Line Line { get { return _line; } }

        public Brush Stroke
        {
            get { return (Brush)_line.GetValue(Shape.StrokeProperty); }
            set { _line.SetValue(Shape.StrokeProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double)_line.GetValue(Shape.StrokeThicknessProperty); }
            set
            {
                _line.SetValue(Shape.StrokeThicknessProperty, value);
                if (this.Width <= _line.StrokeThickness)
                {
                    this.Width = _line.StrokeThickness;
                }
                if (this.Height <= _line.StrokeThickness)
                {
                    this.Height = _line.StrokeThickness;
                }
            }
        }

        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)_line.GetValue(Shape.StrokeDashArrayProperty); }
            set { _line.SetValue(Shape.StrokeDashArrayProperty, value); }
        }

        public _Line()
        {
            this.Content = _line;
        }

        public override IEnumerable<LPS.Controls.Property> GetProperties()
        {
            yield return new Property("公共", "Stroke", "颜色", "设置直线颜色");
            yield return new Property("公共", "StrokeThickness", "直线粗细", "设置直线粗细");
            yield return new Property("公共", "StrokeDashArray", "线条类型", "设置直线线条类型", base.StrokeDashArraySource);
            yield return new Property("布局", "Width", "宽度", "设置直线宽度", false);
            yield return new Property("布局", "Height", "高度", "设置直线高度", false);
            yield return new Property("布局", "Left", "左", "设置直线 X 轴位置");
            yield return new Property("布局", "Top", "上", "设置直线 Y 轴位置");
        }

        public override string ToString()
        {
            return "直线";
        }
    }
}
