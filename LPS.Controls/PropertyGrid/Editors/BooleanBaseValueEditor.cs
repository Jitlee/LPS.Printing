using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LPS.Controls._PropertyGrid.Parts
{
    public abstract class BooleanBaseValueEditor : ValueEditorBase
    {
        private readonly CheckBox _checkBox = new CheckBox() { VerticalAlignment = VerticalAlignment.Center };

        public BooleanBaseValueEditor(PropertyItem item)
            : base(item)
        {
            _checkBox.IsChecked = IsChecked(item.Value);
            _checkBox.Checked += CheckBox_Checked;
            _checkBox.Unchecked += CheckBox_Unchecked;
            _checkBox.IsHitTestVisible = item.PropertyInfo.CanWrite;
            this.Content = _checkBox;
        }

        void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (base.Item.PropertyInfo.CanWrite)
            {
                base.Item.Value = GetCheckedValue();
            }
        }

        void CheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (base.Item.PropertyInfo.CanWrite)
            {
                base.Item.Value = GetUncheckedValue();
            }
        }

        public override void OnValueChanged(object newValue)
        {
            _checkBox.Checked += CheckBox_Checked;
            _checkBox.Unchecked += CheckBox_Unchecked;
            _checkBox.IsChecked = IsChecked(base.Item.Value);
            _checkBox.Checked -= CheckBox_Checked;
            _checkBox.Unchecked -= CheckBox_Unchecked;
        }

        protected abstract bool IsChecked(object value);
        protected abstract object GetCheckedValue();
        protected abstract object GetUncheckedValue();
    }
}
