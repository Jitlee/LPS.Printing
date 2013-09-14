using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace LPS.Controls._PropertyGrid.Parts
{
    public class CornerRadiusValueEditor : ValueEditorBase
    {
        private Border _border;
        private Popup _popup;
        private Button _button;
        private TextBox _allTextBox;
        private TextBox _topLeftTextBox;
        private TextBox _topRightTextBox;
        private TextBox _bottoRightTextBox;
        private TextBox _bottomLeftTextBox;

        public CornerRadiusValueEditor(PropertyItem item)
            : base(item)
        {
            this.DefaultStyleKey = typeof(CornerRadiusValueEditor);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _popup = base.GetTemplateChild("PART_Popup") as Popup;
            _border = base.GetTemplateChild("PART_Border") as Border;
            _button = base.GetTemplateChild("PART_Button") as Button;
            _allTextBox = base.GetTemplateChild("PART_All") as TextBox;
            _topLeftTextBox = base.GetTemplateChild("PART_TopLeft") as TextBox;
            _topRightTextBox = base.GetTemplateChild("PART_TopRight") as TextBox;
            _bottoRightTextBox = base.GetTemplateChild("PART_BottomRight") as TextBox;
            _bottomLeftTextBox = base.GetTemplateChild("PART_BottomLeft") as TextBox;

            _button.Click += Button_Click;

            _popup.Closed += Popup_Closed;
            SetContent((CornerRadius)base.Item.Value);

            _popup.PlacementTarget = this;
        }

        void Button_Click(object sender, RoutedEventArgs e)
        {
            _popup.IsOpen = true;
            _border.Width = this.ActualWidth;
            _popup.VerticalOffset = -this.ActualHeight;

            CornerRadius cornerRadius = (CornerRadius)base.Item.Value;
            SetAllText(cornerRadius);
            SetTopLeftText(cornerRadius);
            SetTopRightText(cornerRadius);
            SetBottomRightText(cornerRadius);
            SetBottomLeftText(cornerRadius);

            _allTextBox.KeyDown += AllTextBox_KeyDown;
            _allTextBox.LostFocus += AllTextBox_LostFocus;
            _topLeftTextBox.LostFocus += TopLeftTextBox_LostFocus;
            _topRightTextBox.LostFocus += TopRightTextBox_LostFocus;
            _bottoRightTextBox.LostFocus += BottomRightTextBox_LostFocus;
            _bottomLeftTextBox.LostFocus += BottomLeftTextBox_LostFocus;

            _allTextBox.Focus();
            _allTextBox.SelectAll();
        }

        void AllTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                _popup.IsOpen = false;
            }
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            CornerRadius cornerRadius = GetCornerRadius();
            SetContent(cornerRadius);
            base.Item.Value = cornerRadius;
        }

        void AllTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int value = Converter.ToInt32(_allTextBox.Text.Trim(), int.MinValue);
            if (value == int.MinValue)
            {
                SetAllClean();
            }
        }

        void TopLeftTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int value = Converter.ToInt32(_topLeftTextBox.Text.Trim(), int.MinValue);
            if (value == int.MinValue)
            {
                _topLeftTextBox.Text = ((CornerRadius)base.Item.Value).TopLeft.ToString();
            }
        }

        void TopRightTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int value = Converter.ToInt32(_topRightTextBox.Text.Trim(), int.MinValue);
            if (value == int.MinValue)
            {
                _topRightTextBox.Text = ((CornerRadius)base.Item.Value).TopRight.ToString();
            }
        }

        void BottomRightTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int value = Converter.ToInt32(_bottoRightTextBox.Text.Trim(), int.MinValue);
            if (value == int.MinValue)
            {
                _bottoRightTextBox.Text = ((CornerRadius)base.Item.Value).BottomRight.ToString();
            }
        }

        void BottomLeftTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int value = Converter.ToInt32(_bottomLeftTextBox.Text.Trim(), int.MinValue);
            if (value == int.MinValue)
            {
                _bottomLeftTextBox.Text = ((CornerRadius)base.Item.Value).BottomLeft.ToString();
            }
        }
        private void SetAllClean()
        {
            _allTextBox.TextChanged -= AllTextBox_TextChanged;
            _allTextBox.Text = string.Empty;
            _allTextBox.TextChanged += AllTextBox_TextChanged;
        }

        private void SetAllText(CornerRadius cornerRadius)
        {
            _allTextBox.TextChanged -= AllTextBox_TextChanged;
            if (cornerRadius.BottomLeft == cornerRadius.TopLeft &&
                cornerRadius.BottomRight == cornerRadius.TopLeft &&
                cornerRadius.TopRight == cornerRadius.TopLeft)
            {
                _allTextBox.Text = cornerRadius.TopLeft.ToString();
            }
            else
            {
                _allTextBox.Text = string.Empty;
            }
            _allTextBox.TextChanged += AllTextBox_TextChanged;
        }

        private void SetContent(CornerRadius cornerRadius)
        {
            if (cornerRadius.BottomLeft == cornerRadius.TopLeft &&
                cornerRadius.BottomRight == cornerRadius.TopLeft &&
                cornerRadius.TopRight == cornerRadius.TopLeft)
            {
                _button.Content = cornerRadius.TopLeft.ToString();
            }
            else if (cornerRadius.BottomLeft == cornerRadius.BottomRight &&
                cornerRadius.TopRight == cornerRadius.TopLeft)
            {
                _button.Content = string.Format("{0}, {1}", cornerRadius.TopLeft, cornerRadius.BottomLeft);
            }
            else
            {
                _button.Content = string.Format("{0}, {1}, {2}, {3}", cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomRight, cornerRadius.BottomLeft);
            }
        }

        private void SetTopLeftText(CornerRadius cornerRadius)
        {
            _topLeftTextBox.TextChanged -= TextBox_TextChanged;
            _topLeftTextBox.Text = cornerRadius.TopLeft.ToString();
            _topLeftTextBox.TextChanged += TextBox_TextChanged;
        }

        private void SetTopRightText(CornerRadius cornerRadius)
        {
            _topRightTextBox.TextChanged -= TextBox_TextChanged;
            _topRightTextBox.Text = cornerRadius.TopRight.ToString();
            _topRightTextBox.TextChanged += TextBox_TextChanged;
        }

        private void SetBottomRightText(CornerRadius cornerRadius)
        {
            _bottoRightTextBox.TextChanged -= TextBox_TextChanged;
            _bottoRightTextBox.Text = cornerRadius.BottomRight.ToString();
            _bottoRightTextBox.TextChanged += TextBox_TextChanged;
        }

        private void SetBottomLeftText(CornerRadius cornerRadius)
        {
            _bottomLeftTextBox.TextChanged -= TextBox_TextChanged;
            _bottomLeftTextBox.Text = cornerRadius.BottomLeft.ToString();
            _bottomLeftTextBox.TextChanged += TextBox_TextChanged;
        }

        private void AllTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int all = Converter.ToInt32(_allTextBox.Text.Trim(), int.MinValue);
            if (all > int.MinValue)
            {
                CornerRadius cornerRadius = new CornerRadius(all);
                SetTopLeftText(cornerRadius);
                SetTopRightText(cornerRadius);
                SetBottomRightText(cornerRadius);
                SetBottomLeftText(cornerRadius);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetAllText(GetCornerRadius());
        }

        private CornerRadius GetCornerRadius()
        {
            int left = Converter.ToInt32(_topLeftTextBox.Text.Trim());
            int right = Converter.ToInt32(_topRightTextBox.Text.Trim());
            int top = Converter.ToInt32(_bottoRightTextBox.Text.Trim());
            int bottom = Converter.ToInt32(_bottomLeftTextBox.Text.Trim());
            return new CornerRadius(left, top, right, bottom);
        }

        public override void OnValueChanged(object newValue)
        {
            CornerRadius cornerRadius = (CornerRadius)newValue;
            SetContent(cornerRadius);
            if (_popup.IsOpen)
            {
                SetAllText(cornerRadius);
                SetTopLeftText(cornerRadius);
                SetTopRightText(cornerRadius);
                SetBottomRightText(cornerRadius);
                SetBottomLeftText(cornerRadius);
            }
        }
    }
}
