using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LPS.Printing.WPF
{
    /// <summary>
    /// TemplateDesignWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TemplateDesignWindow : Window
    {
        public TemplateDesignWindow()
        {
            InitializeComponent();

        }

        public class Test : INotifyPropertyChanged
        {
            private string _title;
            private string _name;
            private string _description;
            private double _width;
            private double _height;
            private bool _isEnabled;
            private Color _stroke;
            private Color _fill;
            private double _strokeThickness;

            [DisplayName("标题")]
            public string Title { get { return _title; } set { _title = value; RaisePropertyChanged("Title"); } }
            [DisplayName("标题")]
            public string Name { get { return _name; } set { _name = value; RaisePropertyChanged("Name"); } }
            [Category("布局")]
            [DisplayName("宽度")]
            public double Width { get { return _width; } set { _width = value; RaisePropertyChanged("Width"); } }
            [Category("布局")]
            [DisplayName("高度")]
            public double Height { get { return _height; } set { _height = value; RaisePropertyChanged("Height"); } }

            [Category("笔刷")]
            [DisplayName("填充色")]
            public Color Fill { get { return _fill; } set { _fill = value; RaisePropertyChanged("Fill"); } }
            [Category("笔刷")]
            [DisplayName("边框颜色")]
            public Color Stroke { get { return _stroke; } set { _stroke = value; RaisePropertyChanged("Stroke"); } }
            [Category("笔刷")]
            [DisplayName("边框宽度")]
            public double StrokeThickness { get { return _strokeThickness; } set { _strokeThickness = value; RaisePropertyChanged("StrokeThickness"); } }

            [Category("公共")]
            [DisplayName("是否可用")]
            public bool IsEnabled { get { return _isEnabled; } set { _isEnabled = value; RaisePropertyChanged("IsEnabled"); } }

            public event PropertyChangedEventHandler PropertyChanged;

            private void RaisePropertyChanged(string propertyName)
            {
                if (null != PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PropertyGrid.Browse(new Test(), new string[] { "Title", "Width", "Fill", "Stroke", "StrokeThickness", "Height", "IsEnabled", "Name" });
        }
    }
}
