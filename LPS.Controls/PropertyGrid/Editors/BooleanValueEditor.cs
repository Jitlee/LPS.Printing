using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LPS.Controls._PropertyGrid.Parts
{
    public class BooleanValueEditor : ValueEditorBase
    {
        private readonly CheckBox _checkBox = new CheckBox() { VerticalAlignment = VerticalAlignment.Center };

        public BooleanValueEditor(PropertyItem item)
            : base(item)
        {
            _checkBox.IsChecked = Convert.ToBoolean(item.Value);
            _checkBox.Checked += CheckBox_Checked;
            _checkBox.Unchecked += CheckBox_Unchecked;
            _checkBox.IsHitTestVisible = item.PropertyInfo.CanWrite;
            this.Content = _checkBox;
        }

        void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (base.Item.PropertyInfo.CanWrite)
            {
                base.Item.Value = true;
            }
        }

        void CheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (base.Item.PropertyInfo.CanWrite)
            {
                base.Item.Value = false;
            }
        }

        public override void OnValueChanged(object newValue)
        {
            _checkBox.Checked += CheckBox_Checked;
            _checkBox.Unchecked += CheckBox_Unchecked;
            _checkBox.IsChecked = Convert.ToBoolean(base.Item.Value);
            _checkBox.Checked -= CheckBox_Checked;
            _checkBox.Unchecked -= CheckBox_Unchecked;
        }
    }
}
