using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using System.Drawing;

namespace YoonFactory.CV
{
    public static class CvColorExtension
    {
        public static Scalar ToScalar(this Color pColor)
        {
            return Scalar.FromRgb(pColor.R, pColor.G, pColor.B);
        }
    }

    public static class YoonFigureExtension
    {
        public static OpenCvSharp.Point ToCVPoint(this IYoonVector pVector)
        {
            switch (pVector)
            {
                case YoonVector2N pVec2N:
                    return new OpenCvSharp.Point(pVec2N.X, pVec2N.Y);
                case YoonVector2D pVec2D:
                    return new OpenCvSharp.Point(pVec2D.X, pVec2D.Y);
                default:
                    return new OpenCvSharp.Point();
            }
        }

        public static Point2d ToCVPoint2D(this IYoonVector pVector)
        {
            switch (pVector)
            {
                case YoonVector2N pVec2N:
                    return new Point2d(pVec2N.X, pVec2N.Y);
                case YoonVector2D pVec2D:
                    return new Point2d(pVec2D.X, pVec2D.Y);
                default:
                    return new Point2d();
            }
        }

        public static YoonVector2N ToYoonVector(this OpenCvSharp.Point pPos)
        {
            return new YoonVector2N(pPos.X, pPos.Y);
        }

        public static YoonVector2D ToYoonVector(this Point2d pPos)
        {
            return new YoonVector2D(pPos.X, pPos.Y);
        }

        public static Rect ToCVRect(this IYoonRect pRect)
        {
            switch (pRect)
            {
                case YoonRect2N pRect2N:
                    return new Rect(pRect2N.TopLeft.ToCVPoint(), new OpenCvSharp.Size(pRect2N.Width, pRect2N.Height));
                case YoonRect2D pRect2D:
                    return new Rect(pRect2D.TopLeft.ToCVPoint(), new OpenCvSharp.Size(pRect2D.Width, pRect2D.Height));
                default:
                    return new Rect();
            }
        }

        public static Rect2d ToCVRect2D(this IYoonRect pRect)
        {
            switch (pRect)
            {
                case YoonRect2N pRect2N:
                    return new Rect2d(pRect2N.TopLeft.ToCVPoint2D(), new Size2d(pRect2N.Width, pRect2N.Height));
                case YoonRect2D pRect2D:
                    return new Rect2d(pRect2D.TopLeft.ToCVPoint2D(), new Size2d(pRect2D.Width, pRect2D.Height));
                default:
                    return new Rect2d();
            }
        }

        public static YoonRect2N ToYoonRect(this Rect pRect)
        {
            YoonVector2N pPosCenter = new YoonVector2N(pRect.X + pRect.Width / 2, pRect.Y + pRect.Height / 2);
            return new YoonRect2N(pPosCenter, pRect.Width, pRect.Height);
        }

        public static YoonRect2D ToYoonRect(this Rect2d pRect)
        {
            YoonVector2D pPosCenter = new YoonVector2D(pRect.X + pRect.Width / 2, pRect.Y + pRect.Height / 2);
            return new YoonRect2D(pPosCenter, pRect.Width, pRect.Height);
        }
    }
}
