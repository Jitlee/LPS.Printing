using LPS.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace LPS.Printing.WPF.Controls
{
    public class _Ellipse : _Control
    {
        private readonly Ellipse _ellipse = new Ellipse() { StrokeThickness = 1d, Stroke = Brushes.Black, Fill = Brushes.White };

        #region 属性

        public Brush Fill
        {
            get { return (Brush)_ellipse.GetValue(Shape.FillProperty); }
            set { _ellipse.SetValue(Shape.FillProperty, value); }
        }

        public Brush Stroke
        {
            get { return (Brush)_ellipse.GetValue(Shape.StrokeProperty); }
            set { _ellipse.SetValue(Shape.StrokeProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double)_ellipse.GetValue(Shape.StrokeThicknessProperty); }
            set { _ellipse.SetValue(Shape.StrokeThicknessProperty, value); }
        }

        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)_ellipse.GetValue(Shape.StrokeDashArrayProperty); }
            set { _ellipse.SetValue(Shape.StrokeDashArrayProperty, value); }
        }

        #endregion

        public _Ellipse()
        {
            this.Content = _ellipse;
        }

        public override IEnumerable<LPS.Controls.Property> GetProperties()
        {
            yield return new Property("公共", "Fill", "填充颜色", "设置圆形背景颜色");
            yield return new Property("公共", "Stroke", "边框颜色", "设置圆形边框颜色");
            yield return new Property("公共", "StrokeThickness", "边框大小", "设置圆形边框大小");
            yield return new Property("公共", "StrokeDashArray", "线条类型", "设置圆形线条类型", base.StrokeDashArraySource);
            yield return new Property("布局", "Width", "宽度", "设置圆形宽度");
            yield return new Property("布局", "Height", "高度", "设置圆形高度");
            yield return new Property("布局", "Left", "左", "设置圆形 X 轴位置");
            yield return new Property("布局", "Top", "上", "设置圆形 Y 轴位置");
        }

        public override string ToString()
        {
            return "圆形";
        }
    }
}
