using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LPS.Controls._PropertyGrid.Parts
{
    public class StringValueEditor : ValueEditorBase
    {
        private readonly TextBox _textBox = new TextBox() { BorderBrush = null, BorderThickness =new Thickness(), VerticalAlignment = VerticalAlignment.Center};

        public StringValueEditor(PropertyItem item)
            : base(item)
        {
            _textBox.Text = null != item.Value ? item.Value.ToString() : string.Empty;
            _textBox.TextChanged += _textBox_TextChanged;
            _textBox.IsReadOnly = !item.PropertyInfo.CanWrite;
            this.Content = _textBox;
        }

        void _textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (base.Item.PropertyInfo.CanWrite)
            {
                base.Item.Value = _textBox.Text;
            }
        }

        public override void OnValueChanged(object newValue)
        {
            _textBox.TextChanged -= _textBox_TextChanged;
            _textBox.Text = null != newValue ? newValue.ToString() : string.Empty;
            _textBox.TextChanged += _textBox_TextChanged;
        }
    }
}
