using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro;
using YoonFactory.Cognex.Result;
using YoonFactory.Image;
using YoonFactory.Image.Result;
using System.Drawing;

namespace YoonFactory.Cognex
{
    public static class CogColorExtension
    {
        public static CogColorConstants ToCogColor(this Color pColor)
        {
            //
        }
    }

    public static class YoonResultExtension
    {
        public static ImageResult ToImageResult(this CognexResult pResult)
        {
            ObjectList pList = new ObjectList();
            foreach (IYoonObject pObject in pResult.ObjectDictionary.Values)
            {
                YoonObject<YoonRect2N> pTargetObject = new YoonObject<YoonRect2N>();
                int nCenterX = 0;
                int nCenterY = 0;
                int nWidth = 0;
                int nHeight = 0;
                switch (pObject)
                {
                    case YoonObject<YoonRect2N> pObjectRect2N:
                        pTargetObject = pObjectRect2N.Clone() as YoonObject<YoonRect2N>;
                        pList.Add(pTargetObject);
                        break;
                    case YoonObject<YoonRect2D> pObjectRect2D:
                        pTargetObject.Label = pObjectRect2D.Label;
                        nCenterX = (int)pObjectRect2D.Object.CenterPos.X;
                        nCenterY = (int)pObjectRect2D.Object.CenterPos.Y;
                        nWidth = (int)pObjectRect2D.Object.Width;
                        nHeight = (int)pObjectRect2D.Object.Height;
                        pTargetObject.Object = new YoonRect2N(nCenterX, nCenterY, nWidth, nHeight);
                        pTargetObject.ObjectImage = pObjectRect2D.ObjectImage?.Clone() as YoonImage;
                        pTargetObject.PixelCount = pObjectRect2D.PixelCount;
                        pTargetObject.Score = pObjectRect2D.Score;
                        pList.Add(pTargetObject);
                        break;
                    case YoonObject<YoonRectAffine2D> pObjectRectAffine:
                        pTargetObject.Label = pObjectRectAffine.Label;
                        nCenterX = (int)pObjectRectAffine.Object.CenterPos.X;
                        nCenterY = (int)pObjectRectAffine.Object.CenterPos.Y;
                        nWidth = (int)pObjectRectAffine.Object.Width;
                        nHeight = (int)pObjectRectAffine.Object.Height;
                        pTargetObject.Object = new YoonRect2N(nCenterX, nCenterY, nWidth, nHeight);
                        pTargetObject.ObjectImage = pObjectRectAffine.ObjectImage?.Clone() as YoonImage;
                        pTargetObject.PixelCount = pObjectRectAffine.PixelCount;
                        pTargetObject.Score = pObjectRectAffine.Score;
                        pList.Add(pTargetObject);
                        break;
                    default:
                        break;
                }
            }
            return new ImageResult(pResult.ResultImage, pList);
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
    }
}
