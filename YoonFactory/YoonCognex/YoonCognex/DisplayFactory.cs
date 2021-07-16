using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using System;
using System.Drawing;
using YoonFactory.Cognex.Result;
using YoonFactory.Cognex.Mapping;

namespace YoonFactory.Cognex
{
    public static class DisplayFactory
    {
        public static void GetMousePosition(this CogDisplay pDisplay, YoonVector2N pPosMouse, out YoonVector2D pPosImage) => Inform.GetMousePosition(pDisplay, pPosMouse, out pPosImage);
        public static void GetMousePosition(this CogDisplay pDisplay, int nX, int nY, out YoonVector2D pPosImage) => Inform.GetMousePosition(pDisplay, nX, nY, out pPosImage);
        public static void GetMousePosition(this CogDisplay pDisplay, int nX, int nY, out double dX, out double dY) => Inform.GetMousePosition(pDisplay, nX, nY, out dX, out dY);
        public static bool DrawGrid(this CogDisplay pDisplay, int nPartX, int nPartY) => Draw.DrawGrid(pDisplay, nPartX, nPartY);
        public static bool DrawROI(this CogDisplay pDisplay, YoonVector2D pPosCenter, double dWidth, double dHeight) => Draw.DrawROI(pDisplay, pPosCenter, dWidth, dHeight);
        public static bool DrawROI(this CogDisplay pDisplay, YoonRect2D pRect) => Draw.DrawROI(pDisplay, pRect);
        public static bool DrawBlobRect(this CogDisplay pDisplay, CognexResult pResult, int nMinimumPixelCount) => Draw.DrawBlobRect(pDisplay, pResult, nMinimumPixelCount);
        public static bool DrawPatternTrainROI(this CogDisplay pDisplay, YoonVector2D pPosStart, double dWidth, double dHeight) => Draw.DrawPatternTrainROI(pDisplay, pPosStart, dWidth, dHeight);
        public static bool DrawPatternTrainROI(this CogDisplay pDisplay, double dStartX, double dStartY, double dWidth, double dHeight) => Draw.DrawPatternTrainROI(pDisplay, dStartX, dStartY, dWidth, dHeight);
        public static bool DrawPatternMatchCross(this CogDisplay pDisplay, CognexResult pResult) => Draw.DrawPatternMatchCross(pDisplay, pResult);
        public static bool DrawPatternMatchRect(this CogDisplay pDisplay, CognexResult pResult) => Draw.DrawPatternMatchRect(pDisplay, pResult);
        public static bool DrawCalibrationMap(this CogDisplay pDisplay, CognexMapping pMapping) => Draw.DrawCalibrationMap(pDisplay, pMapping);
        public static bool DrawRuler(this CogDisplay pDisplay, YoonVector2D pPosStart, YoonVector2D pPosEnd, double dResolution) => Draw.DrawRuler(pDisplay, pPosStart, pPosEnd, dResolution);
        public static bool DrawRect(this CogDisplay pDisplay, Color nColor, YoonVector2D pPosCenter, double dWidth, double dHeight) => Draw.DrawRect(pDisplay, nColor, pPosCenter, dWidth, dHeight);
        public static bool DrawRect(this CogDisplay pDisplay, Color nColor, YoonRect2D pRect) => Draw.DrawRect(pDisplay, nColor, pRect);
        public static bool DrawLine(this CogDisplay pDisplay, Color nColor, YoonVector2D pPosStart, YoonVector2D pPosEnd) => Draw.DrawLine(pDisplay, nColor, pPosStart, pPosEnd);
        public static bool DrawLine(this CogDisplay pDisplay, Color nColor, YoonLine2D pLine) => Draw.DrawLine(pDisplay, nColor, pLine);
        public static void DrawCross(this CogDisplay pDisplay, Color nColor, YoonVector2D pPos, double dLineLength) => Draw.DrawCross(pDisplay, nColor, pPos, dLineLength);
        public static void DrawText(this CogDisplay pDisplay, Color nColor, YoonVector2D pPos, string strText) => Draw.DrawText(pDisplay, nColor, pPos, strText);

        public static class Inform
        {
            public static void GetMousePosition(CogDisplay pDisplay, YoonVector2N pPosMouse, out YoonVector2D pPosImage)
            {
                GetMousePosition(pDisplay, pPosMouse.X, pPosMouse.Y, out pPosImage);
            }

            public static void GetMousePosition(CogDisplay pDisplay, int nX, int nY, out YoonVector2D pPosImage)
            {
                double dX, dY;
                GetMousePosition(pDisplay, nX, nY, out dX, out dY);
                pPosImage = new YoonVector2D(dX, dY);
            }

            public static void GetMousePosition(CogDisplay pDisplay, int nX, int nY, out double dX, out double dY)
            {
                CogDisplayPanAnchorConstants pAnchor;
                dX = 0.0;
                dY = 0.0;
                if (pDisplay != null && !pDisplay.IsDisposed)
                {

                    try
                    {
                        double dAnchorX, dAnchorY;
                        ////  Mouse Point(nX, nY)에는 이미 anchor가 합해짐.
                        pDisplay.GetImagePanAnchor(out dAnchorX, out dAnchorY, out pAnchor);
                        double dClientWidth = pDisplay.ClientRectangle.Width * dAnchorX;
                        double dClientHeight = pDisplay.ClientRectangle.Height * dAnchorY;
                        double dImageWidth = pDisplay.Image.Width * dAnchorX;
                        double dImageHeight = pDisplay.Image.Height * dAnchorY;
                        double dPanX = pDisplay.PanX;
                        double dPanY = pDisplay.PanY;
                        dX = ((double)nX - dClientWidth) / pDisplay.Zoom - dPanX + dImageWidth;    //  Client 좌표를 Image 좌표로 전환.
                        dY = ((double)nY - dClientHeight) / pDisplay.Zoom - dPanY + dImageHeight;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                    }
                }
            }
        }

        public static class Draw
        {
            public static bool DrawGrid(CogDisplay pDisplay, int nPartsX, int nPartsY, string strGroup = "")   // nPart가 2이면 Grid 선은 1개, nPart가 3이면 Grid 선은 2개
            {
                if (nPartsX <= 1 || nPartsY <= 1) return false;
                if (pDisplay == null || pDisplay.IsDisposed) return false;
                if (pDisplay.Width == 0 || pDisplay.Height == 0) return false;
                ////  Parts 간 폭 구하기
                double dPartWidth = pDisplay.Width / nPartsX;
                double dPartHeight = pDisplay.Height / nPartsY;
                ////  Grid 그리기
                for (int iPart = 1; iPart < nPartsX; iPart++)
                {
                    CogLineSegment cogLine_Vert = new CogLineSegment();
                    {
                        cogLine_Vert.Color = CogColorConstants.Yellow;
                        cogLine_Vert.SetStartEnd(dPartWidth * iPart, 0, dPartWidth * iPart, pDisplay.Height);
                        cogLine_Vert.LineWidthInScreenPixels = 1;
                        cogLine_Vert.Interactive = false;
                    }
                    pDisplay.InteractiveGraphics.Add(cogLine_Vert, strGroup, false);
                }
                for (int iPart = 1; iPart < nPartsY; iPart++)
                {
                    CogLineSegment cogLine_Horz = new CogLineSegment();
                    {
                        cogLine_Horz.Color = CogColorConstants.Yellow;
                        cogLine_Horz.SetStartEnd(0, dPartHeight * iPart, pDisplay.Width, dPartHeight * iPart);
                        cogLine_Horz.LineWidthInScreenPixels = 1;
                        cogLine_Horz.Interactive = false;
                    }
                    pDisplay.InteractiveGraphics.Add(cogLine_Horz, strGroup, false);
                }
                return true;
            }

            public static bool DrawROI(CogDisplay pDisplay, YoonVector2D pPosCenter, double dWidth, double dHeight, string strGroup = "")
            {
                return DrawROI(pDisplay, pPosCenter.X, pPosCenter.Y, dWidth, dHeight, strGroup)?.ToYoonRect() == new YoonRect2D(pPosCenter, dWidth, dHeight);
            }

            public static bool DrawROI(CogDisplay pDisplay, YoonRect2D pRect, string strGroup = "")
            {
                return DrawROI(pDisplay, pRect.CenterPos.X, pRect.CenterPos.Y, pRect.Width, pRect.Height, strGroup)?.ToYoonRect() == pRect;
            }

            public static CogRectangle DrawROI(CogDisplay pDisplay, double dCenterX, double dCenterY, double dWidth, double dHeight, string strGroup = "")
            {
                if (pDisplay == null || pDisplay.IsDisposed)
                    return null;

                ////  ROI 사각형 그리기
                CogRectangle pRectROI = new CogRectangle();
                {
                    pRectROI.X = dCenterX;
                    pRectROI.Y = dCenterY;
                    pRectROI.Width = dWidth;
                    pRectROI.Height = dHeight;
                    pRectROI.Color = CogColorConstants.Red;
                    pRectROI.GraphicDOFEnable = CogRectangleDOFConstants.All;
                    pRectROI.TipText = "ROI";
                    pRectROI.Interactive = false; // 움직임 없음. 고정.
                }
                pDisplay.InteractiveGraphics.Add(pRectROI, strGroup, false);
                return pRectROI;
            }

            public static bool DrawPatternTrainROI(CogDisplay pDisplay, YoonVector2D pPosStart, double dWidth, double dHeight, string strGroup = "")
            {
                return DrawPatternTrainROI(pDisplay, pPosStart.X, pPosStart.Y, dWidth, dHeight, strGroup);
            }

            public static bool DrawPatternTrainROI(CogDisplay pDisplay, double dStartX, double dStartY, double dWidth, double dHeight, string strGroup = "")
            {
                if (pDisplay == null || pDisplay.IsDisposed) return false;

                ////  Mark ROI 사각형 아래에 Text 적기
                CogGraphicLabel pCogLabel = new CogGraphicLabel();
                {
                    pCogLabel.BackgroundColor = CogColorConstants.None;
                    pCogLabel.Color = CogColorConstants.Red;
                    pCogLabel.SetXYText(dStartX, dStartY, "Mark ROI");
                    pCogLabel.Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 8.0f, FontStyle.Regular);
                    pCogLabel.Interactive = false;
                    pCogLabel.Alignment = CogGraphicLabelAlignmentConstants.BottomLeft;
                }
                pDisplay.InteractiveGraphics.Add(pCogLabel, strGroup, false);

                ////  Mark ROI 안에 십자가 그리기
                double dCenterX = dStartX + dWidth / 2;
                double dCenterY = dStartY + dHeight / 2;
                double dLineLength = 5;
                CogLineSegment pCogLineHorz = new CogLineSegment();
                {
                    pCogLineHorz.Color = CogColorConstants.Yellow;
                    pCogLineHorz.SetStartEnd(dCenterX - dLineLength, dCenterY, dCenterX + dLineLength, dCenterY);
                    pCogLineHorz.LineWidthInScreenPixels = 1;
                    pCogLineHorz.Interactive = false;
                }
                CogLineSegment pCogLineVert = new CogLineSegment();
                {
                    pCogLineVert.Color = CogColorConstants.Yellow;
                    pCogLineVert.SetStartEnd(dCenterX, dCenterY - dLineLength, dCenterX, dCenterY + dLineLength);
                    pCogLineVert.LineWidthInScreenPixels = 1;
                    pCogLineVert.Interactive = false;
                }
                pDisplay.InteractiveGraphics.Add(pCogLineHorz, strGroup, false);
                pDisplay.InteractiveGraphics.Add(pCogLineVert, strGroup, false);
                return true;
            }

            public static bool DrawBlobRect(CogDisplay pDisplay, CognexResult pResult, int nMinimumPixelCount, string strGroup = "", bool bInteractive = false, bool bDisplayText = false)
            {
                if (pDisplay == null || pDisplay.IsDisposed) return false;
                if (pResult == null || pResult.ToolType != eYoonCognexType.Blob) return false;
                if (pResult.CogShapeDictionary == null || pResult.CogShapeDictionary.Count == 0) return false;
                if (pResult.ObjectDataset == null || pResult.ObjectDataset.Count == 0) return false;

                foreach (int nID in pResult.CogShapeDictionary.Keys)
                {
                    if (pResult.ObjectDataset.Search(nID)?.PixelCount < nMinimumPixelCount) continue;

                    CogRectangleAffine cogRectAffine = pResult.CogShapeDictionary[nID] as CogRectangleAffine;
                    {
                        cogRectAffine.Color = CogColorConstants.DarkGreen;
                        cogRectAffine.LineWidthInScreenPixels = 1;
                        cogRectAffine.Interactive = false;
                    }
                    pDisplay.InteractiveGraphics.Add(cogRectAffine, strGroup, false);

                    if (bDisplayText == true)
                    {
                        string strTextID = string.Format("ID={0}", nID);
                        CogGraphicLabel cogLabel = new CogGraphicLabel();
                        {
                            cogLabel.BackgroundColor = CogColorConstants.None;
                            cogLabel.Color = CogColorConstants.Red;
                            cogLabel.SetXYText(cogRectAffine.CornerYX, cogRectAffine.CornerYY + 10, strTextID);
                            cogLabel.Font = new Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold);
                            cogLabel.Interactive = false;
                            cogLabel.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                        }
                        pDisplay.InteractiveGraphics.Add(cogLabel, strGroup, false);
                    }
                }
                return true;
            }

            public static bool DrawPatternMatchCross(CogDisplay pDisplay, CognexResult pResult, string strGroup = "", bool bInteractive = false, bool bDisplayText = false)
            {
                if (pResult == null || pResult.ToolType != eYoonCognexType.PMAlign)
                    return false;
                if (pResult.ObjectDataset[0].Feature is YoonRectAffine2D pPickArea)
                {
                    YoonVector2D pMatchPos = pPickArea.CenterPos as YoonVector2D;
                    if (pMatchPos.X == 0.0 && pMatchPos.Y == 0.0)
                        return false;
                    if (pPickArea.Width == 0.0 || pPickArea.Height == 0.0 || pResult.ObjectDataset[0].Score == 0)
                        return false;

                    double dLineLength = 2.0;
                    CogLineSegment cogLine_Horz = new CogLineSegment();
                    {
                        cogLine_Horz.Color = CogColorConstants.Green;
                        cogLine_Horz.SetStartEnd(pMatchPos.X - dLineLength, pMatchPos.Y, pMatchPos.X + dLineLength, pMatchPos.Y);
                        cogLine_Horz.LineWidthInScreenPixels = 2;
                        cogLine_Horz.Interactive = bInteractive;
                    }
                    CogLineSegment cogLine_Vert = new CogLineSegment();
                    {
                        cogLine_Vert.Color = CogColorConstants.Green;
                        cogLine_Vert.SetStartEnd(pMatchPos.X, pMatchPos.Y - dLineLength, pMatchPos.X, pMatchPos.Y + dLineLength);
                        cogLine_Vert.LineWidthInScreenPixels = 1;
                        cogLine_Vert.Interactive = bInteractive;
                    }
                    pDisplay.InteractiveGraphics.Add(cogLine_Horz, strGroup, false);
                    pDisplay.InteractiveGraphics.Add(cogLine_Vert, strGroup, false);

                    if (bDisplayText == true && pResult.ObjectDataset[0].Score > 0)
                    {
                        string strTextScore = string.Format("Mark={0:0.0}", pResult.ObjectDataset[0].Score);
                        CogGraphicLabel cogLabel = new CogGraphicLabel();
                        {
                            cogLabel.BackgroundColor = CogColorConstants.None;
                            cogLabel.Color = CogColorConstants.Red;
                            cogLabel.SetXYText(pMatchPos.X + dLineLength, pMatchPos.Y + dLineLength, strTextScore);
                            cogLabel.Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold);
                            cogLabel.Interactive = false;
                            cogLabel.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                        }
                        pDisplay.InteractiveGraphics.Add(cogLabel, strGroup, false);
                    }
                    return true;
                }

                return false;
            }

            public static bool DrawPatternMatchRect(CogDisplay pDisplay, CognexResult pResult, string strGroup = "", bool bInteractive = false, bool bDisplayText = false)
            {
                if (pResult == null || pResult.ToolType != eYoonCognexType.PMAlign)
                    return false;

                if (pResult.ObjectDataset[0].Feature is YoonRectAffine2D pPickArea)
                {
                    YoonVector2D pMatchPos = pPickArea.CenterPos as YoonVector2D;
                    if (pMatchPos.X == 0.0 && pMatchPos.Y == 0.0)
                        return false;
                    if (pPickArea.Width == 0.0 || pPickArea.Height == 0.0 || pResult.ObjectDataset[0].Score == 0)
                        return false;
                    CogRectangle fCogRectMatchResult = new CogRectangle();
                    {
                        fCogRectMatchResult.SetCenterWidthHeight(pMatchPos.X, pMatchPos.Y, pPickArea.Width, pPickArea.Height);
                        fCogRectMatchResult.Color = CogColorConstants.Green;
                        fCogRectMatchResult.GraphicDOFEnable = CogRectangleDOFConstants.All;
                        fCogRectMatchResult.LineWidthInScreenPixels = 3;
                        fCogRectMatchResult.TipText = "Mark ROI";
                        fCogRectMatchResult.Interactive = bInteractive;
                    }
                    pDisplay.InteractiveGraphics.Add(fCogRectMatchResult, strGroup, false);

                    if (bDisplayText == true && pResult.ObjectDataset[0].Score > 0)
                    {
                        string strTextScore = string.Format("Mark={0:0.0}", pResult.ObjectDataset[0].Score);
                        CogGraphicLabel cogLabel = new CogGraphicLabel();
                        {
                            cogLabel.BackgroundColor = CogColorConstants.None;
                            cogLabel.Color = CogColorConstants.Red;
                            cogLabel.SetXYText(pMatchPos.X - pPickArea.Width / 2, pMatchPos.Y + pPickArea.Height / 2 + 10, strTextScore);
                            cogLabel.Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold);
                            cogLabel.Interactive = false;
                            cogLabel.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                        }
                        pDisplay.InteractiveGraphics.Add(cogLabel, strGroup, false);
                    }
                    return true;
                }

                return false;
            }

            public static bool DrawCalibrationMap(CogDisplay pDisplay, CognexMapping pMapping, string strGroup = "", bool bInteractive = false, bool bDisplayText = false)
            {
                if (pMapping == null) return false;
                if (pMapping.PixelPoints.Count == 0 || pMapping.RealPoints.Count == 0 || pMapping.PixelPoints.Count != pMapping.RealPoints.Count) return false;
                if (pMapping.Width == 0 || pMapping.Height == 0) return false;

                double dLineLength = 2.0;
                for (int iPos = 0; iPos < pMapping.PixelPoints.Count; iPos++)
                {
                    YoonVector2D pVecPixel = pMapping.PixelPoints[iPos] as YoonVector2D;
                    CogLineSegment cogLine_Horz = new CogLineSegment();
                    {
                        cogLine_Horz.Color = CogColorConstants.Cyan;
                        cogLine_Horz.SetStartEnd(pVecPixel.X - dLineLength, pVecPixel.Y, pVecPixel.X + dLineLength, pVecPixel.Y);
                        cogLine_Horz.LineWidthInScreenPixels = 2;
                        cogLine_Horz.Interactive = bInteractive;
                    }
                    CogLineSegment cogLine_Vert = new CogLineSegment();
                    {
                        cogLine_Vert.Color = CogColorConstants.Cyan;
                        cogLine_Vert.SetStartEnd(pVecPixel.X, pVecPixel.Y - dLineLength, pVecPixel.X, pVecPixel.Y + dLineLength);
                        cogLine_Vert.LineWidthInScreenPixels = 2;
                        cogLine_Vert.Interactive = bInteractive;
                    }
                    pDisplay.InteractiveGraphics.Add(cogLine_Horz, strGroup, false);
                    pDisplay.InteractiveGraphics.Add(cogLine_Vert, strGroup, false);

                    if (bDisplayText == true && pMapping.RealPoints[iPos] is YoonVector2D pVecReal)
                    {
                        string strTextPos = string.Format("X,Y=({0:0.0},{1:0.0})", pVecReal.X, pVecReal.Y);
                        CogGraphicLabel cogLabel = new CogGraphicLabel();
                        {
                            cogLabel.BackgroundColor = CogColorConstants.None;
                            cogLabel.Color = CogColorConstants.Cyan;
                            cogLabel.SetXYText(pVecPixel.X + dLineLength, pVecPixel.Y + dLineLength, strTextPos);
                            cogLabel.Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 9.0f, FontStyle.Bold);
                            cogLabel.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                        }
                        pDisplay.InteractiveGraphics.Add(cogLabel, strGroup, false);
                    }
                }
                return true;
            }

            public static bool DrawRuler(CogDisplay pDisplay, YoonVector2D pPosStart, YoonVector2D pPosEnd, double dResolution, string strGroup = "", bool bDisplayText = false)
            {
                return DrawRuler(pDisplay, pPosStart.X, pPosStart.Y, pPosEnd.X, pPosEnd.Y, dResolution, strGroup, bDisplayText);
            }

            public static bool DrawRuler(CogDisplay pDisplay, double dStartX, double dStartY, double dEndX, double dEndY, double dResolution, string strGroup = "", bool bDisplayText = false)
            {
                string strTextLength = "";

                ////  상부 측정 Point의 표시
                CogLineSegment cogLineStartCross_Horz = new CogLineSegment();
                {
                    cogLineStartCross_Horz.Color = CogColorConstants.Yellow;
                    cogLineStartCross_Horz.SetStartEnd(dStartX - 5.0, dStartY, dStartX + 5.0, dStartY);
                    cogLineStartCross_Horz.LineWidthInScreenPixels = 3;
                    cogLineStartCross_Horz.Interactive = false;
                }
                CogLineSegment cogLineStartCross_Vert = new CogLineSegment();
                {
                    cogLineStartCross_Vert.Color = CogColorConstants.Yellow;
                    cogLineStartCross_Vert.SetStartEnd(dStartX, dStartY - 5.0, dStartX, dStartY + 5.0);
                    cogLineStartCross_Vert.LineWidthInScreenPixels = 3;
                    cogLineStartCross_Vert.Interactive = false;
                }
                pDisplay.InteractiveGraphics.Add(cogLineStartCross_Horz, strGroup, false);
                pDisplay.InteractiveGraphics.Add(cogLineStartCross_Vert, strGroup, false);

                ////  실제 측정 Draw
                CogLineSegment fCogLine = new CogLineSegment();
                {
                    fCogLine.Color = CogColorConstants.Yellow;
                    fCogLine.SetStartEnd(dStartX, dStartY, dEndX, dEndY);
                    fCogLine.LineWidthInScreenPixels = 3;
                    fCogLine.Interactive = false;
                }
                pDisplay.InteractiveGraphics.Add(fCogLine, strGroup, false);

                ////  하부 측정 Point의 표시
                CogLineSegment cogLineEndCross_Horz = new CogLineSegment();
                {
                    cogLineEndCross_Horz.Color = CogColorConstants.Yellow;
                    cogLineEndCross_Horz.SetStartEnd(dEndX - 5.0, dEndY, dEndX + 5.0, dEndY);
                    cogLineEndCross_Horz.LineWidthInScreenPixels = 3;
                    cogLineEndCross_Horz.Interactive = false;
                }
                CogLineSegment cogLineEndCross_Vert = new CogLineSegment();
                {
                    cogLineEndCross_Vert.Color = CogColorConstants.Yellow;
                    cogLineEndCross_Vert.SetStartEnd(dEndX, dEndY - 5.0, dEndX, dEndY + 5.0);
                    cogLineEndCross_Vert.LineWidthInScreenPixels = 3;
                    cogLineEndCross_Vert.Interactive = false;
                }
                pDisplay.InteractiveGraphics.Add(cogLineEndCross_Horz, strGroup, false);
                pDisplay.InteractiveGraphics.Add(cogLineEndCross_Vert, strGroup, false);

                ////  실측정(mm) 정보 표기
                if (bDisplayText == true)
                {
                    double dPosLabelX = dEndX + 10.0;
                    double dPosLabelY = dEndY + 10.0;
                    double dLineLength = fCogLine.Length * dResolution;
                    strTextLength = string.Format("{0:F3} mm", dLineLength);
                    CogGraphicLabel cogLabel = new CogGraphicLabel();
                    {
                        cogLabel.BackgroundColor = CogColorConstants.None;
                        cogLabel.Color = CogColorConstants.Yellow;
                        cogLabel.SetXYText(dPosLabelX, dPosLabelY, strTextLength);
                        cogLabel.Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 14.0f, FontStyle.Bold);
                        cogLabel.Interactive = false;
                        cogLabel.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                    }
                    pDisplay.InteractiveGraphics.Add(cogLabel, strGroup, false);
                }
                return true;
            }

            public static bool DrawRect(CogDisplay pDisplay, Color nColor, YoonVector2D pPosCenter, double dWidth, double dHeight, bool bInteractive = false, string strGroup = "")
            {
                return DrawRect(pDisplay, nColor.ToCogColor(), pPosCenter.X, pPosCenter.Y, dWidth, dHeight, bInteractive, strGroup).ToYoonRect() == new YoonRect2D(pPosCenter, dWidth, dHeight);
            }

            public static bool DrawRect(CogDisplay pDisplay, Color nColor, YoonRect2D pRect, bool bInteractive = false, string strGroup = "")
            {
                return DrawRect(pDisplay, nColor.ToCogColor(), pRect.CenterPos.X, pRect.CenterPos.Y, pRect.Width, pRect.Height, bInteractive, strGroup).ToYoonRect() == pRect;
            }

            public static CogRectangle DrawRect(CogDisplay pDisplay, CogColorConstants nColor, double dCenterX, double dCenterY, double dWidth, double dHeight, bool bInteractive = false, string strGroup = "")
            {
                ////  ROI 사각형 그리기
                CogRectangle fCogRectROI = new CogRectangle();
                {
                    fCogRectROI.X = dCenterX;
                    fCogRectROI.Y = dCenterY;
                    fCogRectROI.Width = dWidth;
                    fCogRectROI.Height = dHeight;
                    fCogRectROI.Color = nColor;
                    fCogRectROI.GraphicDOFEnable = CogRectangleDOFConstants.All;
                    fCogRectROI.Interactive = bInteractive;
                }
                pDisplay.InteractiveGraphics.Add(fCogRectROI, strGroup, false);
                return fCogRectROI;
            }

            public static bool DrawLine(CogDisplay pDisplay, Color nColor, YoonVector2D pPosStart, YoonVector2D pPosEnd, bool bInteractive = false, string strGroup = "")
            {
                return DrawLine(pDisplay, nColor.ToCogColor(), pPosStart.X, pPosStart.Y, pPosEnd.X, pPosEnd.Y, bInteractive, strGroup).ToYoonLine() == new YoonLine2D(pPosStart, pPosEnd);
            }

            public static bool DrawLine(CogDisplay pDisplay, Color nColor, YoonLine2D pLine, bool bInteractive = false, string strGroup = "")
            {
                return DrawLine(pDisplay, nColor.ToCogColor(), pLine.StartPos.X, pLine.StartPos.Y, pLine.EndPos.X, pLine.EndPos.Y, bInteractive, strGroup).ToYoonLine() == pLine;
            }

            public static CogLineSegment DrawLine(CogDisplay pDisplay, CogColorConstants nColor, double dStartX, double dStartY, double dEndX, double dEndY, bool bInteractive = false, string strGroup = "")
            {
                CogLineSegment fCogLine = new CogLineSegment();
                {
                    fCogLine.Color = CogColorConstants.Blue;
                    fCogLine.SetStartEnd(dStartX, dStartY, dEndX, dEndY);
                    fCogLine.LineWidthInScreenPixels = 2;
                    fCogLine.Color = nColor;
                    fCogLine.Interactive = bInteractive;
                }
                pDisplay.InteractiveGraphics.Add(fCogLine, strGroup, false);
                return fCogLine;
            }

            public static void DrawCross(CogDisplay pDisplay, Color nColor, YoonVector2D pPos, double dLineLength, bool bInteractive = false, string strGroup = "")
            {
                DrawCross(pDisplay, nColor.ToCogColor(), pPos.X, pPos.Y, dLineLength, bInteractive, strGroup);
            }

            public static void DrawCross(CogDisplay fDisplay, CogColorConstants nColor, double dPointX, double dPointY, double dLineLength, bool bInteractive = false, string strGroup = "")
            {
                CogLineSegment cogLine_Horz = new CogLineSegment();
                {
                    cogLine_Horz.Color = CogColorConstants.Green;
                    cogLine_Horz.SetStartEnd(dPointX - dLineLength, dPointY, dPointX + dLineLength, dPointY);
                    cogLine_Horz.LineWidthInScreenPixels = 2;
                    cogLine_Horz.Color = nColor;
                    cogLine_Horz.Interactive = bInteractive;
                }
                CogLineSegment cogLine_Vert = new CogLineSegment();
                {
                    cogLine_Vert.Color = CogColorConstants.Green;
                    cogLine_Vert.SetStartEnd(dPointX, dPointY - dLineLength, dPointX, dPointY + dLineLength);
                    cogLine_Vert.LineWidthInScreenPixels = 1;
                    cogLine_Vert.Color = nColor;
                    cogLine_Vert.Interactive = bInteractive;
                }
                fDisplay.InteractiveGraphics.Add(cogLine_Horz, strGroup, false);
                fDisplay.InteractiveGraphics.Add(cogLine_Vert, strGroup, false);
            }

            public static void DrawText(CogDisplay pDisplay, Color nColor, YoonVector2D pPos, string strText, float dSizeFont = 14.0f, bool bInteractive = false, string strGroup = "")
            {
                DrawText(pDisplay, nColor.ToCogColor(), pPos.X, pPos.Y, strText, dSizeFont, bInteractive, strGroup);
            }

            public static CogGraphicLabel DrawText(CogDisplay fDisplay, CogColorConstants nColor, double dPosX, double dPosY, string strText, float dSizeFont = 14.0f, bool bInteractive = false, string strGroup = "")
            {
                CogGraphicLabel fCogLabel = new CogGraphicLabel();
                {
                    fCogLabel.BackgroundColor = CogColorConstants.None;
                    fCogLabel.Color = nColor;
                    fCogLabel.SetXYText(dPosX, dPosY, strText);
                    fCogLabel.Font = new System.Drawing.Font(FontFamily.GenericSansSerif, dSizeFont, FontStyle.Bold);
                    fCogLabel.Interactive = bInteractive;
                    fCogLabel.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                }
                fDisplay.InteractiveGraphics.Add(fCogLabel, strGroup, false);
                return fCogLabel;
            }
        }
    }
}