using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace LPS.Controls._PropertyGrid.Parts
{
    public class FontFamilyValueEditor : ValueEditorBase
    {
        private readonly ComboBox _comboBox = new ComboBox() { IsEditable = true, BorderBrush = null, BorderThickness = new Thickness(), DisplayMemberPath = "DisplayName", SelectedValuePath = "FontFamily", };

        #region 字体

        private static IEnumerable<_FontFamily> _fontFamilies = GetFontFamilies().OrderBy(f => Regex.IsMatch(f.DisplayName, "[A-Za-z0-9]+") ? f.DisplayName : string.Concat(0, f.DisplayName));

        private static IEnumerable<_FontFamily> GetFontFamilies()
        {
            foreach (var fontFamily in System.Windows.Media.Fonts.SystemFontFamilies)
            {
                yield return new _FontFamily(fontFamily);
            }
        }

        public class _FontFamily
        {
            static XmlLanguage _enus = XmlLanguage.GetLanguage("en-US");
            static XmlLanguage _zhcn = XmlLanguage.GetLanguage("zh-CN");
            static string _fontName = string.Empty;

            public string DisplayName { get; set; }
            public FontFamily FontFamily { get; set; }

            public _FontFamily() { }

            public _FontFamily(FontFamily fontFamily)
                : this()
            {
                if (fontFamily.FamilyNames.ContainsKey(_zhcn))
                {
                    fontFamily.FamilyNames.TryGetValue(_zhcn, out _fontName);
                }
                else if (fontFamily.FamilyNames.ContainsKey(_enus))
                {
                    fontFamily.FamilyNames.TryGetValue(_enus, out _fontName);
                }
                DisplayName = _fontName;
                FontFamily = fontFamily;
            }

            public override string ToString()
            {
                return DisplayName;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj is _FontFamily)
                {
                    return (obj as _FontFamily).FontFamily.Equals(FontFamily);
                }
                return base.Equals(obj);
            }
        }

        #endregion 字体

        public FontFamilyValueEditor(PropertyItem item)
            : base(item)
        {
            _comboBox.ItemsSource = _fontFamilies;

            if (null != item.Value)
            {
                _comboBox.SelectedValue = item.Value;
            }

            _comboBox.SelectionChanged += ComboBox_SelectionChanged;

            this.Content = _comboBox;
        }

        void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (base.Item.PropertyInfo.CanWrite)
            {
                base.Item.Value = _comboBox.SelectedValue;
            }
        }

        public override void OnValueChanged(object newValue)
        {
            _comboBox.SelectionChanged -= ComboBox_SelectionChanged;
            _comboBox.SelectedValue = newValue;
            _comboBox.SelectionChanged += ComboBox_SelectionChanged;
        }
    }
}
