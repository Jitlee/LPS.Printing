using LPS.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LPS.Printing.WPF.Controls
{
    public abstract class _Control : ContentControl, INotifyPropertyChanged
    {
        public abstract IEnumerable<Property> GetProperties();

        #region 绑定数据字段

        public static DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(Dictionary<object, object>), typeof(_Control));

        public Dictionary<object, object> ItemsSource
        {
            get { return (Dictionary<object, object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static DependencyProperty BindingProperty =
            DependencyProperty.Register("Binding", typeof(string), typeof(_Control));

        public string Binding
        {
            get { return (string)GetValue(BindingProperty); }
            set { SetValue(BindingProperty, value); }
        }

        protected readonly Dictionary<object, object> StrokeDashArraySource =
            new Dictionary<object, object>()
            {
                { new DoubleCollection(), "类型0" },
                { new DoubleCollection( new double[] { 2d, 2d } ), "类型1" },
                { new DoubleCollection( new double[] { 5d, 5d } ), "类型2" },
                { new DoubleCollection( new double[] { 2d, 5d, 2d } ), "类型3" },
                { new DoubleCollection( new double[] { 3d, 7d, 3d } ), "类型4" },
            };

        #endregion 绑定数据字段

        public new double Width
        {
            get
            {
                return (double)GetValue(WidthProperty);
            }
            set
            {
                this.SetValue(WidthProperty, value);
                RaisePropertyChanged("Width");
            }
        }

        public new double Height
        {
            get
            {
                return (double)GetValue(HeightProperty);
            }
            set
            {
                this.SetValue(HeightProperty, value);
                RaisePropertyChanged("Height");
            }
        }

        public double Left
        {
            get
            {
                double left = double.IsNaN(left = Canvas.GetLeft(this)) ? 0d : left;
                return left;
            }
            set
            {
                this.SetValue(Canvas.LeftProperty, value);
                RaisePropertyChanged("Left");
            }
        }

        public double Top
        {
            get
            {
                double top = double.IsNaN(top = Canvas.GetTop(this)) ? 0d : top;
                return top;
            }
            set
            {
                this.SetValue(Canvas.TopProperty, value);
                RaisePropertyChanged("Top");
            }
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return "基本控件";
        }
    }
}