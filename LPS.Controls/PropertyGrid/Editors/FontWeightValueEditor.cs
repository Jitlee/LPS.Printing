using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LPS.Controls._PropertyGrid.Parts
{
    public class FontWeightValueEditor : BooleanBaseValueEditor
    {
        public FontWeightValueEditor(PropertyItem item)
            : base(item)
        {
        }

        protected override bool IsChecked(object value)
        {
            return FontWeights.Bold.Equals(value);
        }

        protected override object GetCheckedValue()
        {
            return FontWeights.Bold;
        }

        protected override object GetUncheckedValue()
        {
            return FontWeights.Normal;
        }
    }
}
