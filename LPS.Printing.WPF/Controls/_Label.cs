using LPS.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LPS.Printing.WPF.Controls
{
    public class _Label : _Control
    {
        private readonly Border _border = new Border() { Background = Brushes.Transparent };
        private readonly TextBlock _textBlock = new TextBlock()
        {
            TextWrapping = TextWrapping.Wrap,
            Text = "文字",
            VerticalAlignment = VerticalAlignment.Center,
        };

        #region 属性

        public string Text
        {
            get { return (string)_textBlock.GetValue(TextBlock.TextProperty); }
            set { _textBlock.SetValue(TextBlock.TextProperty, value); }
        }

        public new Thickness Padding
        {
            get { return (Thickness)_textBlock.GetValue(TextBlock.MarginProperty); }
            set { _textBlock.SetValue(TextBlock.MarginProperty, value); }
        }

        public new Brush Background
        {
            get { return (Brush)_border.GetValue(Border.BackgroundProperty); }
            set { _border.SetValue(Border.BackgroundProperty, value); }
        }

        public new Brush BorderBrush
        {
            get { return (Brush)_border.GetValue(Border.BorderBrushProperty); }
            set { _border.SetValue(Border.BorderBrushProperty, value); }
        }

        public new Thickness BorderThickness
        {
            get { return (Thickness)_border.GetValue(Border.BorderThicknessProperty); }
            set { _border.SetValue(Border.BorderThicknessProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)_border.GetValue(Border.CornerRadiusProperty); }
            set { _border.SetValue(Border.CornerRadiusProperty, value); }
        }

        public new HorizontalAlignment HorizontalContentAlignment
        {
            get { return (HorizontalAlignment)_textBlock.GetValue(TextBlock.HorizontalAlignmentProperty); }
            set 
            {
                _textBlock.SetValue(TextBlock.HorizontalAlignmentProperty, value);
                switch (value)
                {
                    case HorizontalAlignment.Left:
                        _textBlock.TextAlignment = TextAlignment.Left;
                        break;
                    case HorizontalAlignment.Center:
                        _textBlock.TextAlignment = TextAlignment.Center;
                        break;
                    case HorizontalAlignment.Right:
                        _textBlock.TextAlignment = TextAlignment.Right;
                        break;
                    case HorizontalAlignment.Stretch:
                        _textBlock.TextAlignment = TextAlignment.Justify;
                        break;
                }
            }
        }

        public new VerticalAlignment VerticalContentAlignment
        {
            get { return (VerticalAlignment)_textBlock.GetValue(TextBlock.VerticalAlignmentProperty); }
            set { _textBlock.SetValue(TextBlock.VerticalAlignmentProperty, value); }
        }

        #endregion

        public _Label()
        {
            _border.Child = _textBlock;
            this.Content = _border;
        }

        public override IEnumerable<Property> GetProperties()
        {
            yield return new Property("公共", "Text", "文本", "设置标签文本");
            yield return new Property("公共", "Background", "背景颜色", "设置标签背景颜色");
            yield return new Property("公共", "BorderBrush", "边框颜色", "设置标签边框颜色");
            yield return new Property("公共", "BorderThickness", "边框大小", "设置标签边框大小");
            yield return new Property("公共", "CornerRadius", "圆角", "设置标签圆角大小");
            yield return new Property("布局", "Width", "宽度", "设置标签宽度");
            yield return new Property("布局", "Height", "高度", "设置标签高度");
            yield return new Property("布局", "Left", "左", "设置标签 X 轴位置");
            yield return new Property("布局", "Top", "上", "设置标签 Y 轴位置");
            yield return new Property("布局", "Padding", "文字边距", "设置标签 Y 轴位置");
            yield return new Property("布局", "HorizontalContentAlignment", "水平方向", "设置标签内文字",
                new Dictionary<object, object>()
                {
                    { HorizontalAlignment.Left, "左" },
                    { HorizontalAlignment.Center, "居中" },
                    { HorizontalAlignment.Right, "右" },
                    { HorizontalAlignment.Stretch, "平铺" },
                });
            yield return new Property("布局", "VerticalContentAlignment", "垂直方向", "设置标签内文字",
                new Dictionary<object, object>()
                {
                    { VerticalAlignment.Top, "上" },
                    { VerticalAlignment.Center, "居中" },
                    { VerticalAlignment.Bottom, "下" },
                    { VerticalAlignment.Stretch, "平铺" },
                });
            yield return new Property("字体", "Foreground", "字体颜色", "设置标签字体颜色");
            yield return new Property("字体", "FontFamily", "字体", "设置标签字体");
            yield return new Property("字体", "FontSize", "字体", "设置标签字体");
            yield return new Property("字体", "FontWeight", "加粗", "设置标签是否加粗");
            yield return new Property("字体", "FontStyle", "斜体", "设置标签是否加粗");
            yield return new Property("数据", "Binding", "绑定字段", "设置标签是否加粗", base.ItemsSource);
        }

        public override string ToString()
        {
            return "标签";
        }
    }
}
