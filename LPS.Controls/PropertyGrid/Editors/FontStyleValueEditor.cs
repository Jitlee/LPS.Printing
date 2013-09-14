using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LPS.Controls._PropertyGrid.Parts
{
    public class FontStyleValueEditor : BooleanBaseValueEditor
    {
        public FontStyleValueEditor(PropertyItem item)
            : base(item)
        {
        }
        protected override bool IsChecked(object value)
        {
            return !FontStyles.Normal.Equals(value);
        }

        protected override object GetCheckedValue()
        {
            return FontStyles.Italic;
        }

        protected override object GetUncheckedValue()
        {
            return FontStyles.Normal;
        }
    }
}
