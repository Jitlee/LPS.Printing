using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace LPS.Controls._PropertyGrid.Parts
{
    public class ColorValueEditor : ValueEditorBase
    {
        private readonly ColorPicker _colorPicker = new ColorPicker() { BorderBrush = null, BorderThickness = new Thickness(), };
        public ColorValueEditor(PropertyItem item)
            : base(item)
        {
            this.Content = _colorPicker;

            if (typeof(Brush).IsAssignableFrom(item.PropertyInfo.PropertyType))
            {
                _colorPicker.SelectedBrush = (SolidColorBrush)item.Value;
            }
            else if (typeof(Color).IsAssignableFrom(base.Item.PropertyInfo.PropertyType))
            {
                _colorPicker.SelectedBrush = new SolidColorBrush((Color)item.Value);
            }

            _colorPicker.SelectedBrushChanged += ColorPicker_SelectedBrushChanged;
        }

        void ColorPicker_SelectedBrushChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (base.Item.PropertyInfo.CanWrite)
            {
                if (typeof(Brush).IsAssignableFrom(base.Item.PropertyInfo.PropertyType))
                {
                    base.Item.Value = _colorPicker.SelectedBrush;
                }
                else if (typeof(Color).IsAssignableFrom(base.Item.PropertyInfo.PropertyType))
                {
                    base.Item.Value = _colorPicker.SelectedBrush.Color;
                }
            }
        }

        public override void OnValueChanged(object newValue)
        {
            _colorPicker.SelectedBrushChanged -= ColorPicker_SelectedBrushChanged;
            if (typeof(Brush).IsAssignableFrom(base.Item.PropertyInfo.PropertyType))
            {
                _colorPicker.SelectedBrush = (SolidColorBrush)newValue;
            }
            else if (typeof(Color).IsAssignableFrom(base.Item.PropertyInfo.PropertyType))
            {
                _colorPicker.SelectedBrush = new SolidColorBrush((Color)newValue);
            }
            _colorPicker.SelectedBrushChanged += ColorPicker_SelectedBrushChanged;
        }
    }
}
