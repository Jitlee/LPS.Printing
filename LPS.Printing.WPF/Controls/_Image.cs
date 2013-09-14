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

namespace LPS.Printing.WPF.Controls
{
    public class _Image : _Control
    {
        private readonly Border _border = new Border() { BorderThickness = new Thickness(1d), BorderBrush = Brushes.Black, Background= Brushes.Transparent };
        private readonly Image _image = new Image()
        {
            Margin = new Thickness(2),
        };

        #region 属性
        [ImageAttribute]
        public byte[] ImageSource
        {
            get
            {
                BitmapImage bs = (BitmapImage)_image.GetValue(Image.SourceProperty);
                if (null != bs && (bs.StreamSource is MemoryStream))
                {
                    using(MemoryStream stream =  (bs.StreamSource as MemoryStream))
                    {
                        return stream.ToArray();
                    }
                }
                return null;
            }
            set
            {
                _image.SetValue(Image.SourceProperty, ImageBufferConverter.Instance.Convert(value));
            }
        }

        public Stretch ImageStretch
        {
            get { return (Stretch)_image.GetValue(Image.StretchProperty); }
            set { _image.SetValue(Image.StretchProperty, value); }
        }

        public new Thickness Padding
        {
            get { return (Thickness)_image.GetValue(Image.MarginProperty); }
            set { _image.SetValue(Image.MarginProperty, value); }
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

        #endregion

        public _Image()
        {
            _border.Child = _image;
            this.Content = _border;
        }

        public override IEnumerable<LPS.Controls.Property> GetProperties()
        {
            yield return new Property("公共", "ImageSource", "图片", "设置图片");
            yield return new Property("公共", "ImageStretch", "填充方式", "设置图片填充方式",
                new Dictionary<object, object>()
                {
                    { Stretch.Uniform, "适合" },
                    { Stretch.Fill, "铺满" },
                    { Stretch.UniformToFill, "剪裁填充" },
                    { Stretch.None, "原始大小" },
                });
            yield return new Property("公共", "Background", "背景颜色", "设置图片背景颜色");
            yield return new Property("公共", "BorderBrush", "边框颜色", "设置图片边框颜色");
            yield return new Property("公共", "BorderThickness", "边框大小", "设置图片边框大小");
            yield return new Property("公共", "CornerRadius", "圆角", "设置图片圆角大小");
            yield return new Property("布局", "Width", "宽度", "设置图片宽度");
            yield return new Property("布局", "Height", "高度", "设置图片高度");
            yield return new Property("布局", "Left", "左", "设置图片 X 轴位置");
            yield return new Property("布局", "Top", "上", "设置图片 Y 轴位置");
            yield return new Property("布局", "Padding", "图片边距", "设置标签 Y 轴位置");
            yield return new Property("数据", "Binding", "绑定字段", "设置图片是否加粗", base.ItemsSource);
        }

        public override string ToString()
        {
            return "图片";
        }
    }
}
