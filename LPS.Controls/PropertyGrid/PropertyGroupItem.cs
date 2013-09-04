using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace LPS.Controls._PropertyGrid.Parts
{
    public class PropertyGroupItem : ToggleButton
    {
        public PropertyGroupItem()
        {
            this.DefaultStyleKey = typeof(PropertyGroupItem);
            Items = new List<FrameworkElement>();
        }

        public List<FrameworkElement> Items { get; private set; }

        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);

            foreach (FrameworkElement element in Items)
            {
                element.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnUnchecked(RoutedEventArgs e)
        {
            base.OnUnchecked(e);

            foreach (FrameworkElement element in Items)
            {
                element.Visibility = Visibility.Visible;
            }
        }
    }
}
