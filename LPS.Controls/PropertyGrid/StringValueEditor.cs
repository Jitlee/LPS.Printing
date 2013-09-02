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

        public StringValueEditor(PropertyItem item)
            : base(item)
        {
            if (null != item.Value)
            {
                _textBox.Text = item.Value.ToString();
            }
            this.Content = _textBox;
        }
    }
}
