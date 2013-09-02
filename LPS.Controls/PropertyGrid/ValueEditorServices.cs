using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPS.Controls.PropertyGrid.Parts
{
    public static class ValueEditorServices
    {
        public static ValueEditorBase CreateValueEdiorBase(PropertyItem item)
        {
            return new StringValueEditor(item);
        }
    }
}
