using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LPS.Controls.PropertyGrid.Parts
{
    public class PropertyGridLabel : TextBlock
    {
        public PropertyGridLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridLabel), new FrameworkPropertyMetadata(typeof(PropertyGridLabel)));
        }
    }
}
