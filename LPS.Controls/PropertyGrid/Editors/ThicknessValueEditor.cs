using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace LPS.Controls._PropertyGrid.Parts
{
    public class ThicknessValueEditor : ValueEditorBase
    {
        private Border _border;
        private Popup _popup;
        private Button _button;
        private TextBox _allTextBox;
        private TextBox _leftTextBox;
        private TextBox _rightTextBox;
        private TextBox _topTextBox;
        private TextBox _bottomTextBox;

        public ThicknessValueEditor(PropertyItem item)
            : base(item)
        {
            this.DefaultStyleKey = typeof(ThicknessValueEditor);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _popup = base.GetTemplateChild("PART_Popup") as Popup;
            _border = base.GetTemplateChild("PART_Border") as Border;
            _button = base.GetTemplateChild("PART_Button") as Button;
            _allTextBox = base.GetTemplateChild("PART_All") as TextBox;
            _leftTextBox = base.GetTemplateChild("PART_Left") as TextBox;
            _rightTextBox = base.GetTemplateChild("PART_Right") as TextBox;
            _topTextBox = base.GetTemplateChild("PART_Top") as TextBox;
            _bottomTextBox = base.GetTemplateChild("PART_Bottom") as TextBox;

            _button.Click += Button_Click;

            _popup.Closed += Popup_Closed;
            SetContent((Thickness)base.Item.Value);

            _popup.PlacementTarget = this;
        }

        void Button_Click(object sender, RoutedEventArgs e)
        {
            _popup.IsOpen = true;
            _border.Width = this.ActualWidth;
            _popup.VerticalOffset = -this.ActualHeight;

            Thickness thickness = (Thickness)base.Item.Value;
            SetAllText(thickness);
            SetLeftText(thickness);
            SetRightText(thickness);
            SetTopText(thickness);
            SetBottomText(thickness);

            _allTextBox.LostFocus += AllTextBox_LostFocus;
            _leftTextBox.LostFocus += LeftTextBox_LostFocus;
            _rightTextBox.LostFocus += RightTextBox_LostFocus;
            _topTextBox.LostFocus += TopTextBox_LostFocus;
            _bottomTextBox.LostFocus += BottomTextBox_LostFocus;

            _allTextBox.Focus();
            _allTextBox.SelectAll();
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            base.Item.Value = GetThickness();
        }

        void AllTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int value = Converter.ToInt32(_allTextBox.Text.Trim(), int.MinValue);
            if (value == int.MinValue)
            {
                SetAllClean();
            }
        }

        void LeftTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int value = Converter.ToInt32(_leftTextBox.Text.Trim(), int.MinValue);
            if (value == int.MinValue)
            {
                _leftTextBox.Text = ((Thickness)base.Item.Value).Right.ToString();
            }
        }

        void RightTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int value = Converter.ToInt32(_rightTextBox.Text.Trim(), int.MinValue);
            if (value == int.MinValue)
            {
                _rightTextBox.Text = ((Thickness)base.Item.Value).Right.ToString();
            }
        }

        void TopTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int value = Converter.ToInt32(_topTextBox.Text.Trim(), int.MinValue);
            if (value == int.MinValue)
            {
                _topTextBox.Text = ((Thickness)base.Item.Value).Bottom.ToString();
            }
        }

        void BottomTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int value = Converter.ToInt32(_bottomTextBox.Text.Trim(), int.MinValue);
            if (value == int.MinValue)
            {
                _bottomTextBox.Text = ((Thickness)base.Item.Value).Bottom.ToString();
            }
        }
        private void SetAllClean()
        {
            _allTextBox.TextChanged -= AllTextBox_TextChanged;
            _allTextBox.Text = string.Empty;
            _allTextBox.TextChanged += AllTextBox_TextChanged;
        }

        private void SetAllText(Thickness thickness)
        {
            _allTextBox.TextChanged -= AllTextBox_TextChanged;
            if (thickness.Bottom == thickness.Left &&
                thickness.Bottom == thickness.Right &&
                thickness.Bottom == thickness.Top)
            {
                _allTextBox.Text = thickness.Bottom.ToString();
            }
            else
            {
                _allTextBox.Text = string.Empty;
            }
            _allTextBox.TextChanged += AllTextBox_TextChanged;
        }

        private void SetContent(Thickness thickness)
        {
            if (thickness.Bottom == thickness.Left &&
                thickness.Bottom == thickness.Right &&
                thickness.Bottom == thickness.Top)
            {
                _button.Content = thickness.Bottom.ToString();
            }
            else if (thickness.Bottom == thickness.Top &&
                thickness.Left == thickness.Top)
            {
                _button.Content = string.Format("{0}, {1}", thickness.Left, thickness.Top);
            }
            else
            {
                _button.Content = string.Format("{0}, {1}, {2}, {3}", thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);
            }
        }

        private void SetLeftText(Thickness thickness)
        {
            _leftTextBox.TextChanged -= TextBox_TextChanged;
            _leftTextBox.Text = thickness.Left.ToString();
            _leftTextBox.TextChanged += TextBox_TextChanged;
        }

        private void SetRightText(Thickness thickness)
        {
            _rightTextBox.TextChanged -= TextBox_TextChanged;
            _rightTextBox.Text = thickness.Right.ToString();
            _rightTextBox.TextChanged += TextBox_TextChanged;
        }

        private void SetTopText(Thickness thickness)
        {
            _topTextBox.TextChanged -= TextBox_TextChanged;
            _topTextBox.Text = thickness.Top.ToString();
            _topTextBox.TextChanged += TextBox_TextChanged;
        }

        private void SetBottomText(Thickness thickness)
        {
            _bottomTextBox.TextChanged -= TextBox_TextChanged;
            _bottomTextBox.Text = thickness.Bottom.ToString();
            _bottomTextBox.TextChanged += TextBox_TextChanged;
        }

        private void AllTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int all = Converter.ToInt32(_allTextBox.Text.Trim(), int.MinValue);
            if (all > int.MinValue)
            {
                Thickness thickness = new Thickness(all);
                SetLeftText(thickness);
                SetRightText(thickness);
                SetTopText(thickness);
                SetBottomText(thickness);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetAllText(GetThickness());
        }

        private Thickness GetThickness()
        {
            int left = Converter.ToInt32(_leftTextBox.Text.Trim());
            int right = Converter.ToInt32(_rightTextBox.Text.Trim());
            int top = Converter.ToInt32(_topTextBox.Text.Trim());
            int bottom = Converter.ToInt32(_bottomTextBox.Text.Trim());
            return new Thickness(left, top, right, bottom);
        }

        public override void OnValueChanged(object newValue)
        {
            Thickness thickness = (Thickness)newValue;
            SetContent(thickness);
            if (_popup.IsOpen)
            {
                SetAllText(thickness);
                SetLeftText(thickness);
                SetRightText(thickness);
                SetTopText(thickness);
                SetBottomText(thickness);
            }
        }
    }
}
