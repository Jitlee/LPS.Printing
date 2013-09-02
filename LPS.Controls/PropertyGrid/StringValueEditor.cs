using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace LPS.Controls.PropertyGrid.Parts
{
    public class StringValueEditor : ValueEditorBase
    {
        private readonly TextBox _textBox = new TextBox();

        public StringValueEditor(PropertyGridLabel label, PropertyItem item)
            : base(label, item)
        {
            item.Value = item.Value;

        }
    }
}
