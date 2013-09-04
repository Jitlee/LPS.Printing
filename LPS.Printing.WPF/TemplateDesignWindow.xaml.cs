using LPS.Controls;
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
        private Test _test = new Test() { OperationType = Test.OperationTypeEnum.Delete };
        public TemplateDesignWindow()
        {
            InitializeComponent();
            this.DataContext = _test;
        }

        public class Test : INotifyPropertyChanged
        {
            private string _title;
            private string _name;
            private string _description;
            private double _width;
            private double _height;
            private bool _isEnabled;
            private SolidColorBrush _stroke;
            private SolidColorBrush _fill;
            private Thickness _strokeThickness;
            public OperationTypeEnum _operationType;
            private byte[] _image;

            [DisplayName("标题")]
            public string Title { get { return _title; } set { _title = value; RaisePropertyChanged("Title"); } }
            [DisplayName("名称")]
            public string Name { get { return _name; } set { _name = value; RaisePropertyChanged("Name"); } }
            [Category("布局")]
            [DisplayName("宽度")]
            public double Width { get { return _width; } set { _width = value; RaisePropertyChanged("Width"); } }
            [Category("布局")]
            [DisplayName("高度")]
            public double Height { get { return _height; } set { _height = value; RaisePropertyChanged("Height"); } }

            [Category("笔刷")]
            [DisplayName("填充色")]
            public SolidColorBrush Fill { get { return _fill; } set { _fill = value; RaisePropertyChanged("Fill"); } }
            [Category("笔刷")]
            [DisplayName("边框颜色")]
            public SolidColorBrush Stroke { get { return _stroke; } set { _stroke = value; RaisePropertyChanged("Stroke"); } }
            [Category("笔刷")]
            [DisplayName("边框宽度")]
            public Thickness StrokeThickness { get { return _strokeThickness; } set { _strokeThickness = value; RaisePropertyChanged("StrokeThickness"); } }
            [Category("笔刷")]
            [DisplayName("背景图片")]
            [ImageAttribute("Images", "Images")]
            public byte[] Image { get { return _image; } set { _image = value; RaisePropertyChanged("Image"); } }

            [Category("公共")]
            [DisplayName("是否可用")]
            public bool IsEnabled { get { return _isEnabled; } set { _isEnabled = value; RaisePropertyChanged("IsEnabled"); } }

            [Category("布局")]
            [DisplayName("操作类型")]
            public OperationTypeEnum OperationType { get { return _operationType; } set { _operationType = value; RaisePropertyChanged("OperationType"); } }

            public event PropertyChangedEventHandler PropertyChanged;

            private void RaisePropertyChanged(string propertyName)
            {
                if (null != PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            public enum OperationTypeEnum
            {
                [Description("添加")]
                Add,
                [Description("更新")]
                Update,
                [Description("删除")]
                Delete,
                [Description("移除")]
                Remove,
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PropertyGrid.Browse(_test, new string[] { "Title", "Width", "Fill", "Stroke", "StrokeThickness", "Height", "IsEnabled", "Name", "OperationType","Image" });
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            _test.OperationType = Test.OperationTypeEnum.Add;
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            _test.OperationType = Test.OperationTypeEnum.Update;
        }
    }
}
