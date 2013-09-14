using LPS.Controls;
using LPS.Printing.WPF.Controls;
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

            DesginControl.SelectionChangedAction = OnSelectionChangedAction;
            this.PreviewMouseMove += DesginControl_PreviewMouseMove;
            DesginControl.EndAddAction = OnEndAddAction;
            this.PointRadioButton.Checked += PointRadioButton_Checked;
            this.ImageRadioButton.Checked += ImageRadioButton_Checked;
            this.LabelRadioButton.Checked += LabelRadioButton_Checked;
            this.LineRadioButton.Checked += LineRadioButton_Checked;
            this.RectangleRadioButton.Checked += RectangleRadioButton_Checked;
            this.EllipseRadioButton.Checked += EllipseRadioButton_Checked;
            this.DeleteButton.Click += DeleteButton_Click;
            this.TopButton.Click += TopButton_Click;
            this.UpButton.Click += UpButton_Click;
            this.DownButton.Click += DownButton_Click;
            this.BottomButton.Click += BottomButton_Click;

            MessageTextBlock.Text = "可以选择需要添加的元素，或者鼠标选取屏幕上元素删除、移动等操作";

            AutoAlignButton.Checked += AutoAlignButton_Checked;
            AutoAlignButton.Unchecked += AutoAlignButton_Unchecked;
        }

        void AutoAlignButton_Unchecked(object sender, RoutedEventArgs e)
        {
            DesginControl.SetAutoAlign(false);
        }

        void AutoAlignButton_Checked(object sender, RoutedEventArgs e)
        {
            DesginControl.SetAutoAlign(true);
        }

        void DesginControl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(DesginControl);
            XTextBlock.Text = string.Concat("X: ", point.X);
            YTextBlock.Text = string.Concat("Y: ", point.Y);
        }

        private void OnEndAddAction()
        {
            this.PointRadioButton.IsChecked = true;
        }

        private void OnSelectionChangedAction(List<_Control> selectedList)
        {

            if (selectedList.Count == 0)
            {
                PropertyGrid.Browse(DesginControl, null);
                MessageTextBlock.Text = "可以选择需要添加的元素，或者鼠标选取屏幕上元素删除、移动等操作";
            }
            else if (selectedList.Count == 1)
            {
                PropertyGrid.Browse(selectedList[0], selectedList[0].GetProperties());
                MessageTextBlock.Text = string.Format("选择了一个{0}对象", selectedList[0].ToString());
            }
            else
            {
                PropertyGrid.Browse(null, null);
                MessageTextBlock.Text = string.Format("选择了{0}个对象", selectedList.Count);
            }
        }

        private void PointRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            DesginControl.EndAdd();
            MessageTextBlock.Text = "可以选择需要添加的元素，或者鼠标选取屏幕上元素删除、移动等操作";
        }

        private void LineRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            DesginControl.EndAdd();
            DesginControl.BeginAdd(typeof(_Line));
            MessageTextBlock.Text = "在屏幕空白区域按下一次鼠标左键，然后移动到另一块空白区域再次按下鼠标左键可以绘制一条曲线";
        }

        private void LabelRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            DesginControl.EndAdd();
            DesginControl.BeginAdd(typeof(_Label));
            MessageTextBlock.Text = "在屏幕空白区域拖出一个矩形可以绘制一个标签对象";
        }

        private void ImageRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            DesginControl.EndAdd();
            DesginControl.BeginAdd(typeof(_Image));
            MessageTextBlock.Text = "在屏幕空白区域拖出一个矩形可以绘制一个图片对象";
        }

        private void RectangleRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            DesginControl.EndAdd();
            DesginControl.BeginAdd(typeof(_Rectangle));
            MessageTextBlock.Text = "在屏幕空白区域拖出一个矩形可以绘制一个矩形对象，如果同时按住Shift键可以绘制正方形";
        }

        private void EllipseRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            DesginControl.EndAdd();
            DesginControl.BeginAdd(typeof(_Ellipse));
            MessageTextBlock.Text = "在屏幕空白区域拖出一个矩形可以绘制一个圆形形对象，如果同时按住Shift键可以绘制正圆";
        }

        void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DesginControl.Delete();
        }

        void TopButton_Click(object sender, RoutedEventArgs e)
        {
            DesginControl.SetZIndex(LayoutType.MoveTop);
        }

        void UpButton_Click(object sender, RoutedEventArgs e)
        {
            DesginControl.SetZIndex(LayoutType.MoveUp);
        }

        void DownButton_Click(object sender, RoutedEventArgs e)
        {
            DesginControl.SetZIndex(LayoutType.MoveDown);
        }

        void BottomButton_Click(object sender, RoutedEventArgs e)
        {
            DesginControl.SetZIndex(LayoutType.MoveBottom);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (!(e.Source is TextBox))
            {
                if (Key.Delete == e.Key)
                {
                    DesginControl.Delete();
                }
            }
        }
    }
}
