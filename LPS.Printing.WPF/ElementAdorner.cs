using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace LPS.Printing.WPF
{
    public class ElementAdorner : Adorner
    {
        public enum OffsetType
        {
            TopLeft,
            TopRight,
            BottomRight,
            BottomLeft,
            Move,
        }

        private readonly static SolidColorBrush _brush = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x7A, 0xCC));
        private readonly static Pen _pen = new Pen(_brush, 1d);
        private const double HANDLE_SIZE = 5d;
        private const double ELEMENT_MIN_SIZE = 10d;
        private Point _originPoint;

        public Action<ElementAdorner, OffsetType, double, double> OffsetAction { get; set; }

        public ElementAdorner(UIElement adornedElement)
            : base(adornedElement)
        {

        }

        public void Offset(OffsetType type, double offsetX, double offsetY)
        {
            switch (type)
            {
                case OffsetType.TopLeft:
                    OffsetTopLeft(offsetX, offsetY);
                    break;
                case OffsetType.TopRight:
                    OffsetTopRight(offsetX, offsetY);
                    break;
                case OffsetType.BottomRight:
                    OffsetBottomRight(offsetX, offsetY);
                    break;
                case OffsetType.BottomLeft:
                    OffsetBottomLeft(offsetX, offsetY);
                    break;
                case OffsetType.Move:
                    OffsetMove(offsetX, offsetY);
                    break;
            }
        }

        private void OnOffset(OffsetType type, double offsetX, double offsetY)
        {
            if (null != OffsetAction)
            {
                OffsetAction(this, type, offsetX, offsetY);
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Rect adornedElementRect = GetAdornedElementRect();
            drawingContext.DrawRectangle(Brushes.Transparent, _pen, adornedElementRect);
            drawingContext.DrawEllipse(_brush, null, adornedElementRect.TopLeft, HANDLE_SIZE, HANDLE_SIZE);
            drawingContext.DrawEllipse(_brush, null, adornedElementRect.TopRight, HANDLE_SIZE, HANDLE_SIZE);
            drawingContext.DrawEllipse(_brush, null, adornedElementRect.BottomRight, HANDLE_SIZE, HANDLE_SIZE);
            drawingContext.DrawEllipse(_brush, null, adornedElementRect.BottomLeft, HANDLE_SIZE, HANDLE_SIZE);
        }

        public void RaisePreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            OnPreviewMouseLeftButtonDown(e);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            this.CaptureMouse();

            Rect adornedElementRect = GetAdornedElementRect();

            UIElement element = this.AdornedElement;
            _originPoint = e.GetPosition(element);

            if (IsCloseTo(adornedElementRect.TopLeft, _originPoint))
            {
                this.PreviewMouseLeftButtonUp -= TopLeft_MouseLeftButtonUp;
                this.PreviewMouseMove -= TopLeft_PreviewMouseMove;

                this.PreviewMouseLeftButtonUp += TopLeft_MouseLeftButtonUp;
                this.PreviewMouseMove += TopLeft_PreviewMouseMove;
            }
            else if (IsCloseTo(adornedElementRect.TopRight, _originPoint))
            {
                this.PreviewMouseLeftButtonUp -= TopRight_MouseLeftButtonUp;
                this.PreviewMouseMove -= TopRight_PreviewMouseMove;

                this.PreviewMouseLeftButtonUp += TopRight_MouseLeftButtonUp;
                this.PreviewMouseMove += TopRight_PreviewMouseMove;
            }
            else if (IsCloseTo(adornedElementRect.BottomRight, _originPoint))
            {
                this.PreviewMouseLeftButtonUp -= BottomRight_MouseLeftButtonUp;
                this.PreviewMouseMove -= BottomRight_PreviewMouseMove;

                this.PreviewMouseLeftButtonUp += BottomRight_MouseLeftButtonUp;
                this.PreviewMouseMove += BottomRight_PreviewMouseMove;
            }
            else if (IsCloseTo(adornedElementRect.BottomLeft, _originPoint))
            {
                this.PreviewMouseLeftButtonUp -= BottomLeft_MouseLeftButtonUp;
                this.PreviewMouseMove -= BottomLeft_PreviewMouseMove;

                this.PreviewMouseLeftButtonUp += BottomLeft_MouseLeftButtonUp;
                this.PreviewMouseMove += BottomLeft_PreviewMouseMove;
            }
            else
            {
                this.PreviewMouseLeftButtonUp -= Move_MouseLeftButtonUp;
                this.PreviewMouseMove -= Move_PreviewMouseMove;

                this.PreviewMouseLeftButtonUp += Move_MouseLeftButtonUp;
                this.PreviewMouseMove += Move_PreviewMouseMove;
            }
        }

        private void TopLeft_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            UIElement element = this.AdornedElement;
            Point point = e.GetPosition(element);
            double offsetX = point.X - _originPoint.X;
            double offsetY = point.Y - _originPoint.Y;
            OffsetTopLeft(offsetX, offsetY);
            OnOffset(OffsetType.TopLeft, offsetX, offsetY);
        }

        private void OffsetTopLeft(double offsetX, double offsetY)
        {
            UIElement element = this.AdornedElement;
            double width = (double)element.GetValue(ActualWidthProperty);
            double height = (double)element.GetValue(ActualHeightProperty);
            double left = double.IsNaN((left = Canvas.GetLeft(element))) ? 0d : left;
            double top = double.IsNaN((top = Canvas.GetTop(element))) ? 0d : top;

            double targetWidth = width - offsetX;
            double targetHeight = height - offsetY;
            double targetLeft = left + offsetX;
            double targetTop = top + offsetY;
            if (targetWidth < ELEMENT_MIN_SIZE)
            {
                targetWidth = ELEMENT_MIN_SIZE;
                targetLeft = left + width - ELEMENT_MIN_SIZE;
            }
            if (targetHeight < ELEMENT_MIN_SIZE)
            {
                targetHeight = ELEMENT_MIN_SIZE;
                targetTop = top + height - ELEMENT_MIN_SIZE;
            }

            element.SetValue(FrameworkElement.WidthProperty, targetWidth);
            element.SetValue(FrameworkElement.HeightProperty, targetHeight);
            element.SetValue(Canvas.LeftProperty, targetLeft);
            element.SetValue(Canvas.TopProperty, targetTop);
        }

        private void TopLeft_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
            this.PreviewMouseLeftButtonUp -= TopLeft_MouseLeftButtonUp;
            this.PreviewMouseMove -= TopLeft_PreviewMouseMove;
        }

        private void TopRight_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            UIElement element = this.AdornedElement;
            Point point = e.GetPosition(element);
            double offsetX = point.X - _originPoint.X;
            double offsetY = point.Y - _originPoint.Y;
            OffsetTopRight(offsetX, offsetY);
            OnOffset(OffsetType.TopRight, offsetX, offsetY);
            _originPoint.X = point.X;
        }

        private void OffsetTopRight(double offsetX, double offsetY)
        {
            UIElement element = this.AdornedElement;
            double width = (double)element.GetValue(ActualWidthProperty);
            double height = (double)element.GetValue(ActualHeightProperty);
            double top = double.IsNaN((top = Canvas.GetTop(element))) ? 0d : top;

            double targetWidth = width + offsetX;
            double targetHeight = height - offsetY;
            double targetTop = top + offsetY;
            if (targetWidth < ELEMENT_MIN_SIZE)
            {
                targetWidth = ELEMENT_MIN_SIZE;
            }
            if (targetHeight < ELEMENT_MIN_SIZE)
            {
                targetHeight = ELEMENT_MIN_SIZE;
                targetTop = top + height - ELEMENT_MIN_SIZE;
            }

            element.SetValue(FrameworkElement.WidthProperty, targetWidth);
            element.SetValue(FrameworkElement.HeightProperty, targetHeight);
            element.SetValue(Canvas.TopProperty, targetTop);
        }

        private void TopRight_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
            this.PreviewMouseLeftButtonUp -= TopRight_MouseLeftButtonUp;
            this.PreviewMouseMove -= TopRight_PreviewMouseMove;
        }

        private void BottomRight_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            UIElement element = this.AdornedElement;
            Point point = e.GetPosition(element);
            double offsetX = point.X - _originPoint.X;
            double offsetY = point.Y - _originPoint.Y;
            OffsetBottomRight(offsetX, offsetY);
            OnOffset(OffsetType.BottomRight, offsetX, offsetY);
            _originPoint = point;
        }

        private void OffsetBottomRight(double offsetX, double offsetY)
        {
            UIElement element = this.AdornedElement;
            double width = (double)element.GetValue(ActualWidthProperty);
            double height = (double)element.GetValue(ActualHeightProperty);

            double targetWidth = width + offsetX;
            double targetHeight = height + offsetY;
            if (targetWidth < ELEMENT_MIN_SIZE)
            {
                targetWidth = ELEMENT_MIN_SIZE;
            }
            if (targetHeight < ELEMENT_MIN_SIZE)
            {
                targetHeight = ELEMENT_MIN_SIZE;
            }

            element.SetValue(FrameworkElement.WidthProperty, targetWidth);
            element.SetValue(FrameworkElement.HeightProperty, targetHeight);
        }

        private void BottomRight_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
            this.PreviewMouseLeftButtonUp -= BottomRight_MouseLeftButtonUp;
            this.PreviewMouseMove -= BottomRight_PreviewMouseMove;
        }

        private void BottomLeft_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            UIElement element = this.AdornedElement;
            Point point = e.GetPosition(element);
            double offsetX = point.X - _originPoint.X;
            double offsetY = point.Y - _originPoint.Y;
            OffsetBottomLeft(offsetX, offsetY);
            OnOffset(OffsetType.BottomLeft, offsetX, offsetY);
            _originPoint.Y = point.Y;
        }

        private void OffsetBottomLeft(double offsetX, double offsetY)
        {
            UIElement element = this.AdornedElement;
            double width = (double)element.GetValue(ActualWidthProperty);
            double height = (double)element.GetValue(ActualHeightProperty);
            double left = double.IsNaN((left = Canvas.GetLeft(element))) ? 0d : left;
            double top = double.IsNaN((top = Canvas.GetTop(element))) ? 0d : top;

            double targetWidth = width - offsetX;
            double targetHeight = height + offsetY;
            double targetLeft = left + offsetX;
            if (targetWidth < ELEMENT_MIN_SIZE)
            {
                targetWidth = ELEMENT_MIN_SIZE;
                targetLeft = left + width - ELEMENT_MIN_SIZE;
            }
            if (targetHeight < ELEMENT_MIN_SIZE)
            {
                targetHeight = ELEMENT_MIN_SIZE;
            }

            element.SetValue(FrameworkElement.WidthProperty, targetWidth);
            element.SetValue(FrameworkElement.HeightProperty, targetHeight);
            element.SetValue(Canvas.LeftProperty, targetLeft);
        }

        private void BottomLeft_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
            this.PreviewMouseLeftButtonUp -= BottomLeft_MouseLeftButtonUp;
            this.PreviewMouseMove -= BottomLeft_PreviewMouseMove;
        }

        private void Move_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            UIElement element = this.AdornedElement;
            Point point = e.GetPosition(element);
            double offsetX = point.X - _originPoint.X;
            double offsetY = point.Y - _originPoint.Y;
            OffsetMove(offsetX, offsetY);
            OnOffset(OffsetType.Move, offsetX, offsetY);
        }

        private void OffsetMove(double offsetX, double offsetY)
        {
            UIElement element = this.AdornedElement;
            double left = double.IsNaN((left = Canvas.GetLeft(element))) ? 0d : left;
            double top = double.IsNaN((top = Canvas.GetTop(element))) ? 0d : top;
            double targetLeft = left + offsetX;
            double targetTop = top + offsetY;
            element.SetValue(Canvas.LeftProperty, targetLeft);
            element.SetValue(Canvas.TopProperty, targetTop);
        }

        private void Move_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
            this.PreviewMouseLeftButtonUp -= Move_MouseLeftButtonUp;
            this.PreviewMouseMove -= Move_PreviewMouseMove;
        }

        private Rect GetAdornedElementRect()
        {
            return new Rect(-HANDLE_SIZE, -HANDLE_SIZE, this.AdornedElement.RenderSize.Width + 2d * HANDLE_SIZE, this.AdornedElement.RenderSize.Height + 2d * HANDLE_SIZE);
        }

        private bool IsCloseTo(Point point1, Point point2)
        {
            return (point1.X - point2.X)
                * (point1.X - point2.X) +
                (point1.Y - point2.Y)
                * (point1.Y - point2.Y) <= HANDLE_SIZE * HANDLE_SIZE;
        }
    }
}
