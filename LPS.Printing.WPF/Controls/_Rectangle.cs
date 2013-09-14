using LPS.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LPS.Printing.WPF.Controls
{
    public class _Rectangle : _Control
    {
        private readonly Rectangle _rectangle = new Rectangle() { StrokeThickness = 1d, Stroke = Brushes.Black, Fill = Brushes.White };

        #region 属性

        public Brush Fill
        {
            get { return (Brush)_rectangle.GetValue(Shape.FillProperty); }
            set { _rectangle.SetValue(Shape.FillProperty, value); }
        }

        public Brush Stroke
        {
            get { return (Brush)_rectangle.GetValue(Shape.StrokeProperty); }
            set { _rectangle.SetValue(Shape.StrokeProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double)_rectangle.GetValue(Shape.StrokeThicknessProperty); }
            set { _rectangle.SetValue(Shape.StrokeThicknessProperty, value); }
        }

        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)_rectangle.GetValue(Shape.StrokeDashArrayProperty); }
            set { _rectangle.SetValue(Shape.StrokeDashArrayProperty, value); }
        }

        public double Radius
        {
            get { return (double)_rectangle.GetValue(Rectangle.RadiusXProperty); }
            set { _rectangle.SetValue(Rectangle.RadiusXProperty, value); _rectangle.SetValue(Rectangle.RadiusYProperty, value); }
        }

        #endregion

        public _Rectangle()
        {
            this.Content = _rectangle;
        }

        public override IEnumerable<LPS.Controls.Property> GetProperties()
        {
            yield return new Property("公共", "Fill", "填充颜色", "设置矩形背景颜色");
            yield return new Property("公共", "Stroke", "边框颜色", "设置矩形边框颜色");
            yield return new Property("公共", "StrokeThickness", "边框大小", "设置矩形边框大小");
            yield return new Property("公共", "StrokeDashArray", "线条类型", "设置矩形线条类型", base.StrokeDashArraySource);
            yield return new Property("公共", "Radius", "圆角", "设置矩形圆角");
            yield return new Property("布局", "Width", "宽度", "设置矩形宽度");
            yield return new Property("布局", "Height", "高度", "设置矩形高度");
            yield return new Property("布局", "Left", "左", "设置矩形 X 轴位置");
            yield return new Property("布局", "Top", "上", "设置矩形 Y 轴位置");
        }

        public override string ToString()
        {
            return "矩形";
        }
    }
}
