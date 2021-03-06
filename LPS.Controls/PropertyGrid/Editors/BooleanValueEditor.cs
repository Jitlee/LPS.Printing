﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LPS.Controls._PropertyGrid.Parts
{
    public class BooleanValueEditor : BooleanBaseValueEditor
    {
        public BooleanValueEditor(PropertyItem item)
            : base(item)
        {
        }

        protected override bool IsChecked(object value)
        {
            return Convert.ToBoolean(value);
        }

        protected override object GetCheckedValue()
        {
            return true;
        }

        protected override object GetUncheckedValue()
        {
            return false;
        }
    }
}
