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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PropertyGrid.Browse(this, new string[] { "Title" });
        }
    }
}
