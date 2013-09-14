using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LPS.Controls._PropertyGrid.Parts
{
    public class CutomValueEditor : ValueEditorBase
    {
        private readonly CustomAttribute _attribute;
        public CutomValueEditor(PropertyItem item, CustomAttribute attribute)
            : base(item)
        {
            _attribute = attribute;
            
        }

        private void Button_Click(object sender, EventArgs e)
        {
        }

        public override void OnValueChanged(object newValue)
        {
            
        }
    }
}
