using System.Drawing;

namespace YoonFactory
{
    public static class YoonFigureExtension
    {
        public static Point ToPoint(this IYoonVector pVector)
        {
            switch(pVector)
            {
                case YoonVector2N pVec2N:
                    return new Point(pVec2N.X, pVec2N.Y);
                case YoonVector2D pVec2D:
                    return new Point((int)pVec2D.X, (int)pVec2D.Y);
                default:
                    return new Point();
            };
        }

        public static PointF ToPointF(this IYoonVector pVector)
        {
            switch(pVector)
            {
                case YoonVector2N pVec2N:
                    return new PointF(pVec2N.X, pVec2N.Y);
                case YoonVector2D pVec2D:
                    return new PointF((float)pVec2D.X, (float)pVec2D.Y);
                default:
                    return new PointF();
            }
        }

        public static YoonVector2N ToYoonVector(this Point pPos)
        {
            return new YoonVector2N(pPos.X, pPos.Y);
        }

        public static YoonVector2D ToYoonVector(this PointF pPos)
        {
            return new YoonVector2D(pPos.X, pPos.Y);
        }

        public static Rectangle ToRectangle(this IYoonRect pRect)
        {
            switch (pRect)
            {
                case YoonRect2N pRect2N:
                    return new Rectangle(pRect2N.TopLeft.ToPoint(), new Size(pRect2N.Width, pRect2N.Height));
                case YoonRect2D pRect2D:
                    return new Rectangle(pRect2D.TopLeft.ToPoint(), new Size((int)pRect2D.Width, (int)pRect2D.Height));
                default:
                    return new Rectangle();
            }
        }

        public static RectangleF ToRectangleF(this IYoonRect pRect)
        {
            switch (pRect)
            {
                case YoonRect2N pRect2N:
                    return new RectangleF(pRect2N.TopLeft.ToPointF(), new SizeF(pRect2N.Width, pRect2N.Height));
                case YoonRect2D pRect2D:
                    return new RectangleF(pRect2D.TopLeft.ToPointF(), new SizeF((float)pRect2D.Width, (float)pRect2D.Height));
                default:
                    return new RectangleF();
            }
        }

        public static YoonRect2N ToYoonRect(this Rectangle pRect)
        {
            YoonVector2N pPosCenter = new YoonVector2N(pRect.X + pRect.Width / 2, pRect.Y + pRect.Height / 2);
            return new YoonRect2N(pPosCenter, pRect.Width, pRect.Height);
        }

        public static YoonRect2D ToYoonRect(this RectangleF pRect)
        {
            YoonVector2D pPosCenter = new YoonVector2D(pRect.X + pRect.Width / 2, pRect.Y + pRect.Height / 2);
            return new YoonRect2D(pPosCenter, pRect.Width, pRect.Height);
        }
    }
}
