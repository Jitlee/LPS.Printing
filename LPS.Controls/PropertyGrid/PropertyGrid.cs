using LPS.Controls.PropertyGrid.Parts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls;

namespace LPS.Controls.PropertyGrid
{
    public class PropertyGrid : Control
    {
        private Grid _mainGrid;
        private readonly Dictionary<string, List<PropertyInfo>> _categories = new Dictionary<string, List<PropertyInfo>>();

        public PropertyGrid()
        {
            this.DefaultStyleKey = typeof(PropertyGrid);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _mainGrid = base.GetTemplateChild("PART_MainGrid") as Grid;
        }

        public void Browse(object instance, IEnumerable<string> properties)
        {
            _mainGrid.RowDefinitions.Clear();
            _mainGrid.Children.Clear();
            _categories.Clear();
            if (null != instance && null != properties)
            {
                Type type = instance.GetType();
                PropertyInfo[] propertyInfos = type.GetProperties();
                foreach (PropertyInfo info in propertyInfos)
                {
                    if(properties.Contains(info.Name))
                    {
                        string category = AttributeServices.GetCategory(info);
                        if (_categories.ContainsKey(category))
                        {
                            _categories.Add(category, new List<PropertyInfo>());
                        }
                        _categories[category].Add(info);
                    }
                }
                foreach (KeyValuePair<string, List<PropertyInfo>> keyValuePair in _categories)
                {
                    _mainGrid.RowDefinitions.Add(new RowDefinition());

                    PropertyGroupItem groupItem = new PropertyGroupItem();
                    groupItem.Content = keyValuePair.Key;
                    Grid.SetRow(groupItem, _mainGrid.RowDefinitions.Count);
                    _mainGrid.Children.Add(groupItem);
                    foreach (PropertyInfo info in keyValuePair.Value)
                    {
                        PropertyGridLabel label = new PropertyGridLabel();
                        label.Text = AttributeServices.GetDisplayName(info);
                        Grid.SetColumn(label, 1);
                        Grid.SetRow(label, _mainGrid.RowDefinitions.Count);


                    }
                }
            }
        }
    }
}
