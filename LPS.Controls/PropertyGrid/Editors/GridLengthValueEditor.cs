using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace LPS.Controls._PropertyGrid.Parts
{
    public class GridLengthValueEditor :ValueEditorBase
    {
        private readonly GridLengthConverter _converter = new GridLengthConverter();
        private readonly ComboBox _comboBox = new ComboBox()
        {
            BorderBrush = null,
            Background = null,
            BorderThickness = new Thickness(),
            VerticalAlignment = VerticalAlignment.Center,
            IsEditable = true,
            IsReadOnly = false,
        };

        public GridLengthValueEditor(PropertyItem item)
            : base(item)
        {
            _comboBox.ItemsSource = new string[] { "自动", "*" };
            _comboBox.SelectedValue = item.Value;
            _comboBox.SelectionChanged += ComboBox_SelectionChanged;
            this.Content = _comboBox;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (base.Item.PropertyInfo.CanWrite)
            {
                if (_comboBox.Text == "自动")
                {
                    base.Item.Value = new GridLength(1d, GridUnitType.Auto);
                }
                Match match = Regex.Match(_comboBox.Text, @"^(([1-9])|([1-9][0-9]+)|([0-9]+\.[0-9]+))(?=\*)");
                if (match.Success)
                {
                    double length = double.Parse(match.Value);
                    base.Item.Value = new GridLength(length, GridUnitType.Star);
                }
                else
                {
                    double length = Converter.ToDouble(_comboBox.Text, double.NaN);
                    if (double.IsNaN(length))
                    {
                        base.Item.Value = new GridLength(1d, GridUnitType.Star);

                        _comboBox.SelectionChanged -= ComboBox_SelectionChanged;
                        _comboBox.SelectedValue = "*";
                        _comboBox.SelectionChanged += ComboBox_SelectionChanged;
                    }
                    else
                    {
                        base.Item.Value = new GridLength(length, GridUnitType.Pixel);
                    }
                }
            }
        }

        public override void OnValueChanged(object newValue)
        {
            _comboBox.SelectionChanged -= ComboBox_SelectionChanged;
            GridLength _gridLength = (GridLength)newValue;
            if (_gridLength.IsAuto)
            {
                _comboBox.Text = "自动";
            }
            else if (_gridLength.IsStar)
            {
                _comboBox.Text = string.Concat(_gridLength.Value, "*");
            }
            else
            {
                _comboBox.Text = _gridLength.Value.ToString();
            }
            _comboBox.SelectionChanged += ComboBox_SelectionChanged;
        }
    }
}
