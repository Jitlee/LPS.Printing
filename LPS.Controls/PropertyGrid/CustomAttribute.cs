using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPS.Controls._PropertyGrid.Parts
{
    public class CustomAttribute : Attribute
    {
        public string Method;
        public CustomAttribute(string method)
        {
            Method = method;
        }
    }
}
