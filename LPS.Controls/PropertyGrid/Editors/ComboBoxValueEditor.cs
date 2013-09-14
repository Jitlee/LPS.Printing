using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LPS.Controls._PropertyGrid.Parts
{
    public abstract class ComboBoxValueEditor : ValueEditorBase
    {
        private readonly ComboBox _comboBox = new ComboBox()
        {
            BorderBrush = null,
            Background = null,
            BorderThickness = new Thickness(),
            VerticalAlignment = VerticalAlignment.Center,
            IsEditable = true,
            IsReadOnly = true,
        };

        public ComboBoxValueEditor(PropertyItem item)
            : base(item)
        {
            _comboBox.DisplayMemberPath = GetDisplayMemberPath();
            _comboBox.SelectedValuePath = GetSelectValuePath();
            _comboBox.ItemsSource = GetItemSource();
            _comboBox.SelectedValue = item.Value;
            _comboBox.SelectionChanged += ComboBox_SelectionChanged;
            this.Content = _comboBox;
        }

        protected abstract string GetDisplayMemberPath();
        protected abstract string GetSelectValuePath();
        protected abstract IEnumerable GetItemSource();

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (base.Item.PropertyInfo.CanWrite)
            {
                base.Item.Value = _comboBox.SelectedValue;
            }
        }

        public override void OnValueChanged(object newValue)
        {
            _comboBox.SelectionChanged -= ComboBox_SelectionChanged;
            _comboBox.SelectedValue = base.Item.Value;
            _comboBox.SelectionChanged += ComboBox_SelectionChanged;
        }
    }
}
