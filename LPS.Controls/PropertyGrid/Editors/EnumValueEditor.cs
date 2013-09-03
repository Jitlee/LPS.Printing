using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LPS.Controls.PropertyGrid.Parts
{
    public class EnumValueEditor : ValueEditorBase
    {
        private readonly ComboBox _comboBox = new ComboBox()
        {
            BorderBrush = null,
            Background = null,
            BorderThickness = new Thickness(),
            VerticalAlignment= VerticalAlignment.Center,
            DisplayMemberPath = "Value",
            SelectedValuePath = "Key",
        };
        
        public EnumValueEditor(PropertyItem item)
            : base(item)
        {
            this.Content = _comboBox;
            InitComboBoxSource();
        }

        private void InitComboBoxSource()
        {
            Type type = base.Item.PropertyInfo.PropertyType;
            string[] names = Enum.GetNames(type);
            Dictionary<string, string> items = new Dictionary<string, string>();
            foreach(string name in names)
            {
                DescriptionAttribute decriptionAttribute = AttributeServices.GetAttribute<DescriptionAttribute>(type.GetField(name));
                items.Add(name, null == decriptionAttribute ? name : decriptionAttribute.Description);
            }
            _comboBox.ItemsSource = items;

            if (null != base.Item.Value)
            {
                _comboBox.SelectedValue = base.Item.Value.ToString();
            }

            _comboBox.SelectionChanged += ComboBox_SelectionChanged;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(base.Item.PropertyInfo.CanWrite)
            {
                base.Item.Value = Enum.Parse(base.Item.PropertyInfo.PropertyType, _comboBox.SelectedValue.ToString(), true);
            }
        }



        public override void OnValueChanged(object newValue)
        {
            _comboBox.SelectionChanged -= ComboBox_SelectionChanged;
            _comboBox.SelectedValue = base.Item.Value.ToString();
            _comboBox.SelectionChanged += ComboBox_SelectionChanged;
        }
    }
}
