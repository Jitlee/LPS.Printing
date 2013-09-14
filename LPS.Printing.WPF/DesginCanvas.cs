using LPS.Printing.WPF.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LPS.Printing.WPF
{
    internal class DesginCanvas : Canvas
    {
        private readonly List<_Control> _selectedControls = new List<_Control>();
        private readonly Dictionary<int, ElementAdorner> _adorners = new Dictionary<int, ElementAdorner>();
        private readonly ZIndexComparer _zIndexComparer;

        private AdornerLayer _adornerLayer = null;
        public bool IsAutoAlign { get; set; }

        public List<_Control> SelectedControls { get { return _selectedControls; } }

        public Action<List<_Control>> SelectionChangedAction { get; set; }
        public Action<_Control, Rect> ElementMovingAction { get; set; }
        public Action<_Control, Rect> ElementMovedAction { get; set; }
        public Action<_Line, Point, Point> LineMovingAction { get; set; }
        public Func<_Line, Point, Point, Point> LineMovedAction { get; set; }

        public DesginCanvas()
        {
            Background = Brushes.Transparent;
            _zIndexComparer = new ZIndexComparer(this);
        }

        public void AddNewControl(_Control control)
        {
            this.Children.Add(control);

            CleanSelection();

            SetSelected(control);

            if (null != SelectionChangedAction)
            {
                SelectionChangedAction(_selectedControls);
            }

            OnElementMoved(control, new Rect(control.Left, control.Top, control.Width, control.Height));
        }

        private void SetSelected(_Control control)
        {
            _selectedControls.Add(control);

            if (null == _adornerLayer)
            {
                _adornerLayer = AdornerLayer.GetAdornerLayer(control);
            }
            int rootHasCode = control.GetHashCode();
            if (!_adorners.ContainsKey(rootHasCode))
            {
                if (control is _Line)
                {
                    _adorners.Add(rootHasCode, new LineAdorner(control as _Line));
                }
                else
                {
                    _adorners.Add(rootHasCode, new ElementAdorner(control));
                }

            }
            ElementAdorner rootAdorner = _adorners[rootHasCode];

            _adornerLayer.Add(rootAdorner);

            LoadAdorner(rootAdorner);
        }

        void Adorner_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                HandlerMouseButtonLeftDown(e, (sender as ElementAdorner).AdornedElement as _Control);
            }
        }

        private void DeactivatedAdorner()
        {
            foreach (_Control ui in _selectedControls)
            {
                int hasCode = ui.GetHashCode();
                ElementAdorner adorner = _adorners[hasCode];
                _adorners[hasCode].IsHitTestVisible = false;
            }
        }

        private void ActivatedAdorner()
        {
            foreach (_Control ui in _selectedControls)
            {
                int hasCode = ui.GetHashCode();
                ElementAdorner adorner = _adorners[hasCode];
                _adorners[hasCode].IsHitTestVisible = true;
            }
        }

        private void OnElementMoving(_Control element, Rect rect)
        {
            if (null != ElementMovingAction)
            {
                ElementMovingAction(element, rect);
            }
        }
        private void OnElementMoved(_Control element, Rect rect)
        {
            if (null != ElementMovedAction)
            {
                ElementMovedAction(element, rect);
            }
        }

        private void OnLineMoving(_Line line, Point a, Point b)
        {
            if (null != LineMovingAction)
            {
                LineMovingAction(line, a, b);
            }
        }

        private Point OnLineMoved(_Line line, Point a, Point b)
        {
            if (null != LineMovedAction)
            {
                return LineMovedAction(line, a, b);
            }
            return a;
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            _Control rootElement = GetCanvasRootChild(e.OriginalSource as DependencyObject) as _Control;
            HandlerMouseButtonLeftDown(e, rootElement);
        }

        private readonly Rectangle _marqueeRect = new Rectangle() { Stroke = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x7A, 0xCC)), StrokeThickness = 1d, Fill = new SolidColorBrush(Color.FromArgb(0x55, 0x00, 0x7A, 0xCC)) };
        private Point _originPoint;
        private void HandlerMouseButtonLeftDown(MouseButtonEventArgs e, _Control rootElement)
        {
            if (null != rootElement)
            {
                if (null == _adornerLayer)
                {
                    _adornerLayer = AdornerLayer.GetAdornerLayer(rootElement);
                }
                int rootHasCode = rootElement.GetHashCode();
                if (!_adorners.ContainsKey(rootHasCode))
                {
                    if (rootElement is _Line)
                    {
                        _adorners.Add(rootHasCode, new LineAdorner(rootElement as _Line));
                    }
                    else
                    {
                        _adorners.Add(rootHasCode, new ElementAdorner(rootElement));
                    }
                }
                ElementAdorner rootAdorner = _adorners[rootHasCode];
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    if (_selectedControls.Contains(rootElement))
                    {
                        _adornerLayer.Remove(rootAdorner);

                        UnLoadAdorner(rootAdorner);
                        _selectedControls.Remove(rootElement);
                    }
                    else
                    {
                        _adornerLayer.Add(rootAdorner);

                        LoadAdorner(rootAdorner);

                        _selectedControls.Add(rootElement);
                        rootAdorner.RaisePreviewMouseLeftButtonDown(e);
                    }
                }
                else
                {
                    CleanSelection();
                    _adornerLayer.Add(rootAdorner);

                    LoadAdorner(rootAdorner);
                    _selectedControls.Add(rootElement);
                    rootAdorner.RaisePreviewMouseLeftButtonDown(e);
                }
            }
            else
            {
                CleanSelection();
            }

            if (null != SelectionChangedAction)
            {
                SelectionChangedAction(_selectedControls);
            }
        }

        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonDown(e);

            this.CaptureMouse();
            DeactivatedAdorner();
            this.Children.Remove(_marqueeRect);
            this.Children.Add(_marqueeRect);
            _originPoint = e.GetPosition(this);
            this.PreviewMouseMove -= DesginCanvas_PreviewMouseMove;
            this.PreviewMouseRightButtonUp -= DesginCanvas_PreviewMouseRightButtonUp;

            this.PreviewMouseMove += DesginCanvas_PreviewMouseMove;
            this.PreviewMouseRightButtonUp += DesginCanvas_PreviewMouseRightButtonUp;
        }

        void DesginCanvas_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.PreviewMouseMove -= DesginCanvas_PreviewMouseMove;
            this.PreviewMouseRightButtonUp -= DesginCanvas_PreviewMouseRightButtonUp;
            this.ReleaseMouseCapture();

            CleanSelection();
            _marqueeRect.Width = 0d;
            _marqueeRect.Height = 0d;
            this.Children.Remove(_marqueeRect);
            Point point = e.GetPosition(this);
            Rect rect = new Rect(point, _originPoint);
            if (rect.Width > 10d && rect.Height > 10d)
            {
                TestSelection(rect);
            }

            if (null != SelectionChangedAction)
            {
                SelectionChangedAction(_selectedControls);
            }
        }

        private void TestSelection(Rect rect)
        {
            Point a = new Point(rect.Left, rect.Top);
            Point b = new Point(rect.Left + rect.Width, rect.Top);
            Point c = new Point(rect.Left + rect.Width, rect.Top + rect.Height);
            Point d = new Point(rect.Left, rect.Top + rect.Height);
            foreach (_Control control in this.Children.OfType<_Control>())
            {
                if (control is _Line)
                {
                    _Line line = control as _Line;
                    Point a1 = new Point(line.Line.X1, line.Line.Y1);
                    Point b1 = new Point(line.Line.X2, line.Line.Y2);
                    if (CheckTwoLineCrose(a1, b1, a, b) ||
                        CheckTwoLineCrose(a1, b1, b, c) ||
                        CheckTwoLineCrose(a1, b1, c, d) ||
                        CheckTwoLineCrose(a1, b1, d, a))
                    {
                        SetSelected(control);
                    }
                }
                else
                {
                    Rect r = new Rect(control.Left, control.Top, control.Width, control.Height);
                    if (rect.IntersectsWith(r))
                    {
                        SetSelected(control);
                    }
                }
            }
        }

        /// <summary>
        /// 判断直线2的两点是否在直线1的两边。
        /// </summary>
        /// <returns></returns>
        private bool CheckCrose(Point a1, Point b1, Point a2, Point b2)
        {
            Point v1 = new Point();
            Point v2 = new Point();
            Point v3 = new Point();

            v1.X = a2.X - b1.X;
            v1.Y = a2.Y - b1.Y;

            v2.X = b2.X - b1.X;
            v2.Y = b2.Y - b1.Y;

            v3.X = a1.X - b1.X;
            v3.Y = b1.Y - b1.Y;

            return (CrossMul(v1, v3) * CrossMul(v2, v3) <= 0);

        }
        /// <summary>
        /// 判断两条线段是否相交。
        /// </summary>
        /// <param name="line1">线段1</param>
        /// <param name="line2">线段2</param>
        /// <returns>相交返回真，否则返回假。</returns>
        private bool CheckTwoLineCrose(Point a1, Point b1, Point a2, Point b2)
        {
            return CheckCrose(a1, b1, a2, b2) && CheckCrose(a2, b2, a1, b1);
        }
        /// <summary>
        /// 计算两个向量的叉乘。
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        private double CrossMul(Point pt1, Point pt2)
        {
            return pt1.X * pt2.Y - pt1.Y * pt2.X;
        }

        void DesginCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(this);
            Rect rect = new Rect(point, _originPoint);
            Canvas.SetLeft(_marqueeRect, rect.Left);
            Canvas.SetTop(_marqueeRect, rect.Top);
            _marqueeRect.Width = rect.Width;
            _marqueeRect.Height = rect.Height;
        }

        private void LoadAdorner(ElementAdorner rootAdorner)
        {
            rootAdorner.OffsetAction = OnOffset;
            rootAdorner.ElementMovingAction = OnElementMoving;
            rootAdorner.ElementMovedAction = OnElementMoved;
            rootAdorner.PreviewMouseLeftButtonDown += Adorner_PreviewMouseLeftButtonDown;

            if (rootAdorner is LineAdorner)
            {
                LineAdorner lineAdorner = rootAdorner as LineAdorner;
                lineAdorner.LineMovingAction = OnLineMoving;
                lineAdorner.LineMovedAction = OnLineMoved;
            }
        }

        private void UnLoadAdorner(ElementAdorner rootAdorner)
        {
            rootAdorner.OffsetAction = null;
            rootAdorner.ElementMovingAction = null;
            rootAdorner.ElementMovedAction = null;
            rootAdorner.PreviewMouseLeftButtonDown -= Adorner_PreviewMouseLeftButtonDown;
            if (rootAdorner is LineAdorner)
            {
                LineAdorner lineAdorner = rootAdorner as LineAdorner;
                lineAdorner.LineMovingAction = null;
                lineAdorner.LineMovedAction = null;
            }
        }

        private void OnOffset(ElementAdorner sender, ElementAdorner.OffsetType type, double offsetX, double offsetY)
        {
            if (_selectedControls.Count > 1)
            {
                foreach (_Control ui in _selectedControls)
                {
                    int hasCode = ui.GetHashCode();
                    ElementAdorner adorner = _adorners[hasCode];
                    if (sender != adorner)
                    {
                        adorner.Offset(type, offsetX, offsetY);
                    }
                }
            }
        }

        public void CleanSelection()
        {
            foreach (_Control ui in _selectedControls)
            {
                int hasCode = ui.GetHashCode();
                ElementAdorner adorner = _adorners[hasCode];
                _adornerLayer.Remove(adorner);

                UnLoadAdorner(adorner);
            }
            _selectedControls.Clear();
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

        public void SetZIndex(LayoutType zIndexType)
        {
            switch (zIndexType)
            {
                case LayoutType.MoveTop:
                    MoveTop();
                    break;
                case LayoutType.MoveUp:
                    MoveUp();
                    break;
                case LayoutType.MoveDown:
                    MoveDown();
                    break;
                case LayoutType.MoveBottom:
                    MoveBottom();
                    break;
            }
        }

        private void SameSize()
        {
            
            List<_Control> controls = _selectedControls.Where(c => !(c is _Line)).ToList();
            if (controls.Count > 0)
            {
                double width = controls[0].Width;
                double height = controls[0].Height;
                foreach (_Control control in controls)
                {
                    control.Width = width;
                    control.Height = height;
                }
            }
        }

        private void SameHeight()
        {
            List<_Control> controls = _selectedControls.Where(c => !(c is _Line)).ToList();
            if (controls.Count > 0)
            {
                double height = controls[0].Height;
                foreach (_Control control in controls)
                {
                    control.Height = height;
                }
            }
        }

        private void SameWidth()
        {
            List<_Control> controls = _selectedControls.Where(c => !(c is _Line)).ToList();
            if (controls.Count > 0)
            {
                double width = controls[0].Width;
                foreach (_Control control in controls)
                {
                    control.Width = width;
                }
            }
        }

        private void AlignLeft()
        {
            if (_selectedControls.Count > 0)
            {
                double left = _selectedControls[0].Left;
                foreach (_Control control in _selectedControls)
                {
                    control.Left = left;
                }
            }
        }

        private void AlignTop()
        {
            if (_selectedControls.Count > 0)
            {
                double top = _selectedControls[0].Top;
                foreach (_Control control in _selectedControls)
                {
                    control.Top = top;
                }
            }
        }

        private void AlignRight()
        {
            if (_selectedControls.Count > 0)
            {
                double right = _selectedControls[0].Left + _selectedControls[0].Width;
                foreach (_Control control in _selectedControls)
                {
                    control.Left = right - control.Width;
                }
            }
        }

        private void AlignBottom()
        {
            if (_selectedControls.Count > 0)
            {
                double bottom = _selectedControls[0].Top + _selectedControls[0].Height;
                foreach (_Control control in _selectedControls)
                {
                    control.Top = bottom - control.Height;
                }
            }
        }

        private void MoveTop()
        {
            _selectedControls.Sort(_zIndexComparer);
            foreach (_Control control in _selectedControls)
            {
                this.Children.Remove(control);
                this.Children.Add(control);
            }
        }

        private void MoveUp()
        {
            _selectedControls.Sort(_zIndexComparer);
            foreach (_Control control in _selectedControls)
            {
                int index = this.Children.IndexOf(control);
                if (this.Children.Count - 1 > index)
                {
                    this.Children.RemoveAt(index);
                    this.Children.Insert(index + 1, control);
                }
            }
        }

        private void MoveDown()
        {
            _selectedControls.Sort(_zIndexComparer);
            for(int i = _selectedControls.Count - 1; i > -1; i--)
            {
                int index = this.Children.IndexOf(_selectedControls[i]);
                if (index > 0)
                {
                    this.Children.RemoveAt(index);
                    this.Children.Insert(index - 1, _selectedControls[i]);
                }
            }
        }

        private void MoveBottom()
        {
            _selectedControls.Sort(_zIndexComparer);
            for (int i = _selectedControls.Count - 1; i > -1; i--)
            {
                this.Children.Remove(_selectedControls[i]);
                this.Children.Insert(0, _selectedControls[i]);
            }
        }

        private class ZIndexComparer : IComparer<_Control>
        {
            private readonly Canvas _canvas;
            public ZIndexComparer(Canvas canvas)
            {
                _canvas = canvas;
            }

            //实现姓名升序
            public int Compare(_Control x, _Control y)
            {
                return _canvas.Children.IndexOf(x).CompareTo(_canvas.Children.IndexOf(y));
            }
        }

        public void Delete()
        {
            foreach (_Control ui in _selectedControls)
            {
                int hasCode = ui.GetHashCode();
                ElementAdorner adorner = _adorners[hasCode];
                _adornerLayer.Remove(adorner);

                UnLoadAdorner(adorner);
                this.Children.Remove(ui);
            }
            _selectedControls.Clear();
        }
    }

    public enum LayoutType
    {
        MoveTop,
        MoveUp,
        MoveDown,
        MoveBottom,

        AlignLeft,
        AlignTop,
        AlignRight,
        AlignBottom,
        
        SameSize,
        SameHeight,
        SameWidht,
    }
}
