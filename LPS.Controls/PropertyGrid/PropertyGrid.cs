using LPS.Controls._PropertyGrid.Parts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace LPS.Controls
{
    public class PropertyGrid : Control
    {
        private Grid _mainGrid;
        private GridSplitter _splitter;
        private Line _verticalLine;
        private readonly Dictionary<string, List<PropertyItem>> _categories = new Dictionary<string, List<PropertyItem>>();
        private readonly Dictionary<string, ValueEditorBase> _editors = new Dictionary<string, ValueEditorBase>();

        public PropertyGrid()
        {
            this.DefaultStyleKey = typeof(PropertyGrid);

            InitVerticalGridLine();
            InitGridSplitter();
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
            _editors.Clear();
            if (null != instance && null != properties)
            {
                Type type = instance.GetType();
                PropertyInfo[] propertyInfos = type.GetProperties();
                foreach (PropertyInfo info in propertyInfos)
                {
                    if(properties.Contains(info.Name))
                    {
                        PropertyItem item = new PropertyItem(instance, info);
                        if (!_categories.ContainsKey(item.Category))
                        {
                            _categories.Add(item.Category, new List<PropertyItem>());
                        }
                        _categories[item.Category].Add(item);
                    }
                }
                CreateItem();

                if (instance is INotifyPropertyChanged)
                {
                    (instance as INotifyPropertyChanged).PropertyChanged += PropertyGrid_PropertyChanged;
                }
            }
        }

        private void PropertyGrid_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_editors.ContainsKey(e.PropertyName))
            {
                ValueEditorBase editor = _editors[e.PropertyName];
                editor.Item.Value = editor.Item.PropertyInfo.GetValue(editor.Item.Instance, null);
                editor.OnValueChanged(editor.Item.Value);
            }
        }

        private void CreateItem()
        {
            if (_categories.Count > 0)
            {
                int rowIndex = 0;
                PropertyGroupItem groupItem = null;
                Line gridLine = null;
                PropertyGridLabel label = null;
                ValueEditorBase editor = null;
                foreach (KeyValuePair<string, List<PropertyItem>> keyValuePair in _categories)
                {
                    if (null != gridLine)
                    {
                        _mainGrid.Children.Remove(gridLine);
                    }
                    rowIndex = _mainGrid.RowDefinitions.Count;
                    groupItem = new PropertyGroupItem();
                    groupItem.Content = keyValuePair.Key;
                    Grid.SetRow(groupItem, rowIndex);
                    Grid.SetColumnSpan(groupItem, 3);
                    _mainGrid.Children.Add(groupItem);
                    _mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1d, GridUnitType.Auto) });
                    foreach (PropertyItem item in keyValuePair.Value)
                    {
                        rowIndex = _mainGrid.RowDefinitions.Count;
                        gridLine = CreateHorizontalGridLine(rowIndex, groupItem.Background);
                        label = CreateLabel(rowIndex, item);
                        editor = ValueEditorServices.CreateValueEdiorBase(item);
                        Grid.SetColumn(editor, 2);
                        Grid.SetRow(editor, _mainGrid.RowDefinitions.Count);

                        _editors.Add(item.Name, editor);
                        groupItem.Items.Add(label);
                        groupItem.Items.Add(editor);
                        groupItem.Items.Add(gridLine);
                        _mainGrid.Children.Add(label);
                        _mainGrid.Children.Add(editor);
                        _mainGrid.Children.Add(gridLine);

                        _mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1d, GridUnitType.Auto) });
                    }
                }

                Grid.SetRowSpan(_splitter, rowIndex + 1);
                Grid.SetRowSpan(_verticalLine, rowIndex + 1);
                _verticalLine.Stroke = groupItem.Background;
                _mainGrid.Children.Add(_splitter);
                _mainGrid.Children.Add(_verticalLine);

                _mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1d, GridUnitType.Star) });
            }
        }

        private void InitVerticalGridLine()
        {
            _verticalLine = new Line()
            {
                StrokeThickness = 1d,
                Y2 = 1d,
                Stretch = Stretch.Fill,
                HorizontalAlignment = HorizontalAlignment.Right,
                SnapsToDevicePixels = true,
            };

            Grid.SetZIndex(_verticalLine, 1000);
            Grid.SetColumn(_verticalLine, 1);
            _verticalLine.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
        }

        private Line CreateHorizontalGridLine(int rowIndex, Brush brush)
        {
            Line gridLine = new Line()
            {
                Stroke = brush,
                StrokeThickness = 1d,
                X2 = 1d,
                Stretch = Stretch.Fill,
                VerticalAlignment = VerticalAlignment.Bottom,
                SnapsToDevicePixels = true,
            };

            Grid.SetZIndex(gridLine, 1000);
            Grid.SetColumnSpan(gridLine, 3);
            Grid.SetRow(gridLine, rowIndex);
            gridLine.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            return gridLine;
        }

        private void InitGridSplitter()
        {
            _splitter = new GridSplitter()
            {
                Background = Brushes.Transparent,
                Width = 3,
                HorizontalAlignment = HorizontalAlignment.Right,
            };

            Grid.SetZIndex(_splitter, 1000);
            Grid.SetColumn(_splitter, 1);
        }

        private PropertyGridLabel CreateLabel(int rowIndex, PropertyItem item)
        {
            PropertyGridLabel label = new PropertyGridLabel() { Text = item.DisplayName };
            Grid.SetColumn(label, 1);
            Grid.SetRow(label, rowIndex);
            return label;
        }
    }
}
