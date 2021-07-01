using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro;
using YoonFactory.Cognex.Result;
using YoonFactory.Image;
using System.Drawing;

namespace YoonFactory.Cognex
{
    public static class CogColorExtension
    {
        public static CogColorConstants ToCogColor(this Color pColor)
        {
            foreach (string strColor in Enum.GetNames(typeof(CogColorConstants)))
                if (pColor.Name == strColor)
                    return (CogColorConstants)Enum.Parse(typeof(CogColorConstants), strColor);
            return CogColorConstants.None;
        }
    }

    public static class YoonFigureExtension
    {
        public static CogRectangle ToCogRect(this IYoonRect pRect)
        {
            CogRectangle pCogRect = new CogRectangle();
            switch (pRect)
            {
                case YoonRect2N pRect2N:
                    pCogRect.X = pRect2N.TopLeft.X;
                    pCogRect.Y = pRect2N.TopLeft.Y;
                    pCogRect.Width = pRect2N.Width;
                    pCogRect.Height = pRect2N.Height;
                    break;
                case YoonRect2D pRect2D:
                    pCogRect.X = pRect2D.TopLeft.X;
                    pCogRect.Y = pRect2D.TopLeft.Y;
                    pCogRect.Width = pRect2D.Width;
                    pCogRect.Height = pRect2D.Height;
                    break;
                case YoonRectAffine2D pRectAffine:
                    pCogRect.X = pRectAffine.TopLeft.X;
                    pCogRect.Y = pRectAffine.TopLeft.Y;
                    pCogRect.Width = pRectAffine.Width;
                    pCogRect.Height = pRectAffine.Height;
                    break;
            }
            return pCogRect;
        }

        public static CogRectangleAffine ToCogRectAffine(this IYoonRect pRect)
        {
            CogRectangleAffine pCogRect = new CogRectangleAffine();
            switch (pRect)
            {
                case YoonRect2N pRect2N:
                    pCogRect.CenterX = pRect2N.CenterPos.X;
                    pCogRect.CenterY = pRect2N.CenterPos.Y;
                    pCogRect.SideXLength = pRect2N.Width;
                    pCogRect.SideYLength = pRect2N.Height;
                    pCogRect.Rotation = 0.0;
                    pCogRect.Skew = 0.0;
                    break;
                case YoonRect2D pRect2D:
                    pCogRect.CenterX = pRect2D.CenterPos.X;
                    pCogRect.CenterY = pRect2D.CenterPos.Y;
                    pCogRect.SideXLength = pRect2D.Width;
                    pCogRect.SideYLength = pRect2D.Height;
                    pCogRect.Rotation = 0.0;
                    pCogRect.Skew = 0.0;
                    break;
                case YoonRectAffine2D pRectAffine:
                    pCogRect.CenterX = pRectAffine.CenterPos.X;
                    pCogRect.CenterY = pRectAffine.CenterPos.Y;
                    pCogRect.SideXLength = pRectAffine.Width;
                    pCogRect.SideYLength = pRectAffine.Height;
                    pCogRect.Rotation = pRectAffine.Rotation;
                    pCogRect.Skew = 0.0;
                    break;
            }
            return pCogRect;
        }

        public static CogLineSegment ToCogLineSegment(this IYoonLine pLine)
        {
            CogLineSegment pCogLine = new CogLineSegment();
            switch (pLine)
            {
                case YoonLine2N pLine2N:
                    pCogLine.StartX = pLine2N.StartPos.X;
                    pCogLine.StartY = pLine2N.StartPos.Y;
                    pCogLine.EndX = pLine2N.EndPos.X;
                    pCogLine.EndY = pLine2N.EndPos.Y;
                    break;
                case YoonLine2D pLine2D:
                    pCogLine.StartX = pLine2D.StartPos.X;
                    pCogLine.StartY = pLine2D.StartPos.Y;
                    pCogLine.EndX = pLine2D.EndPos.X;
                    pCogLine.EndY = pLine2D.EndPos.Y;
                    break;
            }
            return pCogLine;
        }

        public static YoonRect2D ToYoonRect(this CogRectangle pRect)
        {
            YoonVector2D pPosCenter = new YoonVector2D(pRect.CenterX, pRect.CenterY);
            return new YoonRect2D(pPosCenter, pRect.Width, pRect.Height);
        }

        public static YoonRect2D ToYoonRect(this CogRectangleAffine pRect)
        {
            YoonVector2D pPosCenter = new YoonVector2D(pRect.CenterX, pRect.CenterY);
            return new YoonRect2D(pPosCenter, pRect.SideXLength, pRect.SideYLength);
        }

        public static YoonRectAffine2D ToYoonRectAffine(this CogRectangleAffine pRect)
        {
            YoonVector2D pPosCenter = new YoonVector2D(pRect.CenterX, pRect.CenterY);
            return new YoonRectAffine2D(pPosCenter, pRect.SideXLength, pRect.SideYLength, pRect.Rotation);
        }

        public static YoonLine2D ToYoonLine(this CogLineSegment pLine)
        {
            return new YoonLine2D(pLine.StartX, pLine.StartY, pLine.EndX, pLine.EndY);
        }
    }
}
