using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LPS.Controls.PropertyGrid.Parts
{
    public abstract class ValueEditorBase : ContentControl
    {
        public PropertyItem Item { get; private set; }
        public ValueEditorBase(PropertyItem item)
        {
            Item = item;
        }

        public abstract void OnValueChanged(object newValue);

        protected void OnValueError(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
