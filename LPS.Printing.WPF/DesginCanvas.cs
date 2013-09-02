using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace LPS.Printing.WPF
{
    public class DesginCanvas : Canvas
    {
        private readonly List<UIElement> _selectedUIElements = new List<UIElement>();
        private readonly Dictionary<int, ElementAdorner> _adorners = new Dictionary<int, ElementAdorner>();
        private AdornerLayer _adornerLayer = null;
        private TextBox _focusableTextBox = new TextBox() { IsReadOnly = true, MinHeight = 0d, MinWidth = 0d, Width=  0d, Height = 0d };

        public DesginCanvas()
        {
            Background = Brushes.Transparent;
            this.Children.Add(_focusableTextBox);
            _focusableTextBox.PreviewKeyDown += Canvas_PreviewKeyDown;
        }

        private void Canvas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                _focusableTextBox.PreviewKeyUp -= Canvas_PreviewKeyUp;
                _focusableTextBox.PreviewKeyUp += Canvas_PreviewKeyUp;

                foreach (UIElement ui in _selectedUIElements)
                {
                    int hasCode = ui.GetHashCode();
                    ElementAdorner adorner = _adorners[hasCode];
                    adorner.IsHitTestVisible = false;
                }
            }
        }

        private void Canvas_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            _focusableTextBox.PreviewKeyUp -= Canvas_PreviewKeyUp;

            foreach (UIElement ui in _selectedUIElements)
            {
                int hasCode = ui.GetHashCode();
                ElementAdorner adorner = _adorners[hasCode];
                adorner.IsHitTestVisible = true;
            }
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            _focusableTextBox.Focus();
            UIElement rootElement = GetCanvasRootChild(e.OriginalSource as DependencyObject) as UIElement;
            if (null != rootElement)
            {
                if (null == _adornerLayer)
                {
                    _adornerLayer = AdornerLayer.GetAdornerLayer(rootElement);
                }
                int rootHasCode = rootElement.GetHashCode();
                if (!_adorners.ContainsKey(rootHasCode))
                {
                    _adorners.Add(rootHasCode, new ElementAdorner(rootElement));
                }
                ElementAdorner rootAdorner = _adorners[rootHasCode];
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    if (_selectedUIElements.Contains(rootElement))
                    {
                        _adornerLayer.Remove(rootAdorner);
                        rootAdorner.OffsetAction = null;
                        _selectedUIElements.Remove(rootElement);
                    }
                    else
                    {
                        _adornerLayer.Add(rootAdorner);
                        _selectedUIElements.Add(rootElement);
                        rootAdorner.OffsetAction = OnOffset;
                        rootAdorner.RaisePreviewMouseLeftButtonDown(e);
                    }
                }
                else
                {
                    CleanSelection();
                    _adornerLayer.Add(rootAdorner);
                    rootAdorner.OffsetAction = OnOffset;
                    _selectedUIElements.Add(rootElement);
                    rootAdorner.RaisePreviewMouseLeftButtonDown(e);
                }
            }
            else
            {
                CleanSelection();
            }
        }

        private void OnOffset(ElementAdorner sender, ElementAdorner.OffsetType type, double offsetX, double offsetY)
        {
            foreach (UIElement ui in _selectedUIElements)
            {
                int hasCode = ui.GetHashCode();
                ElementAdorner adorner = _adorners[hasCode];
                if (sender != adorner)
                {
                    adorner.Offset(type, offsetX, offsetY);
                }
            }
        }

        private void CleanSelection()
        {
            foreach (UIElement ui in _selectedUIElements)
            {
                int hasCode = ui.GetHashCode();
                ElementAdorner adorner = _adorners[hasCode];
                _adornerLayer.Remove(adorner);
                adorner.OffsetAction = null;
            }
            _selectedUIElements.Clear();
        }

        private DependencyObject GetCanvasRootChild(DependencyObject child)
        {
            if (this == child)
            {
                return null;
            }
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (null == parentObject)
            {
                return null;
            }
            else if (this == parentObject)
            {
                return child;
            }
            return GetCanvasRootChild(parentObject);
        }
    }
}
