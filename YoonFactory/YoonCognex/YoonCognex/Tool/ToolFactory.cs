using System;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.ColorExtractor;
using Cognex.VisionPro.ColorSegmenter;
using YoonFactory.Cognex.Result;

namespace YoonFactory.Cognex.Tool
{
    public static class ToolFactory
    {
        public static ICogTool InitCognexTool(eYoonCognexType pType)
        {
            switch (pType)
            {
                case eYoonCognexType.Blob:
                    return new CogBlobTool();
                case eYoonCognexType.Calibration:
                    return new CogCalibCheckerboardTool();
                case eYoonCognexType.ColorExtract:
                    return new CogColorExtractorTool();
                case eYoonCognexType.ColorSegment:
                    return new CogColorSegmenterTool();
                case eYoonCognexType.LineFitting:
                    return new CogFindLineTool();
                case eYoonCognexType.Filtering:
                    return new CogIPOneImageTool();
                case eYoonCognexType.Convert:
                    return new CogImageConvertTool();
                case eYoonCognexType.Sharpness:
                    return new CogImageSharpnessTool();
                case eYoonCognexType.Sobel:
                    return new CogSobelEdgeTool();
                case eYoonCognexType.PMAlign:
                    return new CogPMAlignTool();
                case eYoonCognexType.ImageAdd:
                    return new CogIPTwoImageAddTool();
                case eYoonCognexType.ImageMinMax:
                    return new CogIPTwoImageMinMaxTool();
                case eYoonCognexType.ImageSubtract:
                    return new CogIPTwoImageSubtractTool();
                default:
                    break;
            }
            return null;
        }

        public static Type GetCognexToolType(eYoonCognexType pType)
        {
            switch (pType)
            {
                case eYoonCognexType.Blob:
                    return typeof(CogBlobTool);
                case eYoonCognexType.Calibration:
                    return typeof(CogCalibCheckerboardTool);
                case eYoonCognexType.ColorExtract:
                    return typeof(CogColorExtractorTool);
                case eYoonCognexType.ColorSegment:
                    return typeof(CogColorSegmenterTool);
                case eYoonCognexType.LineFitting:
                    return typeof(CogFindLineTool);
                case eYoonCognexType.Filtering:
                    return typeof(CogIPOneImageTool);
                case eYoonCognexType.Convert:
                    return typeof(CogImageConvertTool);
                case eYoonCognexType.Sharpness:
                    return typeof(CogImageSharpnessTool);
                case eYoonCognexType.Sobel:
                    return typeof(CogSobelEdgeTool);
                case eYoonCognexType.PMAlign:
                    return typeof(CogPMAlignTool);
                case eYoonCognexType.ImageAdd:
                    return typeof(CogIPTwoImageAddTool);
                case eYoonCognexType.ImageSubtract:
                    return typeof(CogIPTwoImageSubtractTool);
                case eYoonCognexType.ImageMinMax:
                    return typeof(CogIPTwoImageMinMaxTool);
                default:
                    break;
            }
            return typeof(object);
        }

        public static ICogTool LoadCognexToolFromVpp(string strFilePath)
        {
            try
            {
                ICogTool pCogTool = CogSerializer.LoadObjectFromFile(strFilePath) as ICogTool;
                return pCogTool;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public static bool SaveCognexToolToVpp(ICogTool pCogTool, string strFilePath)
        {
            try
            {
                CogSerializer.SaveObjectToFile(pCogTool, strFilePath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public static ICogImage InitCognexToolInputImage(eYoonCognexType pType)
        {
            switch (pType)
            {
                case eYoonCognexType.Calibration:
                    return new CogImage8Grey();
                case eYoonCognexType.Convert:
                case eYoonCognexType.ColorExtract:
                case eYoonCognexType.ColorSegment:
                case eYoonCognexType.Filtering:
                    return new CogImage24PlanarColor();
                case eYoonCognexType.Sobel:
                case eYoonCognexType.Sharpness:
                    return new CogImage8Grey();
                case eYoonCognexType.Blob:
                case eYoonCognexType.LineFitting:
                    return new CogImage8Grey();
                case eYoonCognexType.PMAlign:
                    return new CogImage8Grey();
                case eYoonCognexType.ImageAdd:
                case eYoonCognexType.ImageSubtract:
                case eYoonCognexType.ImageMinMax:
                    return new CogImage8Grey();
                default:
                    break;
            }
            return null;
        }

        public static bool RunCognexTool(ICogTool pCogTool, CognexImage pSourceImage, ref string strErrorMessage, ref CognexResult pResult, ref CognexImage pResultImage)
        {
            bool bResult = false;
            switch (pCogTool)
            {
                case CogImageConvertTool pCogToolConvert:
                    bResult = ImageConvert(pCogToolConvert, pSourceImage, ref strErrorMessage, ref pResult);
                    break;
                case CogIPOneImageTool pCogToolIP:
                    bResult = ImageFiltering(pCogToolIP, pSourceImage, ref strErrorMessage, ref pResult);
                    break;
                case CogSobelEdgeTool pCogToolSobel:
                    bResult = SobelEdge(pCogToolSobel, pSourceImage, ref strErrorMessage, ref pResult);
                    break;
                case CogImageSharpnessTool pCogToolSharpness:
                    bResult = ImageSharpness(pCogToolSharpness, pSourceImage, ref strErrorMessage, ref pResult);
                    break;
                case CogCalibCheckerboardTool pCogToolCheckerboard:
                    bResult = Undistort(pCogToolCheckerboard, pSourceImage, ref strErrorMessage, ref pResult);
                    break;
                case CogBlobTool pCogToolBlob:
                    bResult = Blob(pCogToolBlob, pSourceImage, ref strErrorMessage, ref pResult);
                    break;
                case CogColorSegmenterTool pCogToolSegment:
                    bResult = ColorSegment(pCogToolSegment, pSourceImage, ref strErrorMessage, ref pResult);
                    break;
                case CogColorExtractorTool pCogToolExtract:
                    bResult = ColorExtract(pCogToolExtract, pSourceImage, ref strErrorMessage, ref pResult);
                    break;
                case CogFindLineTool pCogToolFindLine:
                    bResult = FindLine(pCogToolFindLine, pSourceImage, ref strErrorMessage, ref pResult);
                    break;
                case CogPMAlignTool pCogToolPM:
                    bResult = PMAlign(pCogToolPM, pSourceImage, ref strErrorMessage, ref pResult);
                    break;
                default:
                    break;
            }
            if (pResult.ResultImage != null)
                pResultImage = pResult.ResultImage.Clone() as CognexImage;

            return bResult;
        }

        public static bool ImageConvert(ICogTool pCogTool, CognexImage pSourceImage, ref string strErrorMessage, ref CognexResult pResult)
        {
            if (!(pCogTool is CogImageConvertTool))
            {
                strErrorMessage = "Input Parameter Error";
                return false;
            }

            try
            {
                ////  초기화
                CogImageConvertTool pCogToolConvert = pCogTool as CogImageConvertTool;
                pCogToolConvert.InputImage = pSourceImage.CogImage;
                //// 동작
                pCogToolConvert.Run();
                //// 결과 확인
                if (pCogToolConvert.RunStatus.Result != CogToolResultConstants.Accept)
                {
                    strErrorMessage = pCogToolConvert.RunStatus.Message;
                    return false;
                }
                //// 결과 출력
                pResult = new CognexResult(eYoonCognexType.Convert, new CognexImage(pCogToolConvert.OutputImage));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                strErrorMessage = "Exception At Unknown Reason";
                return false;
            }
        }

        public static bool SobelEdge(ICogTool pCogTool, CognexImage pSourceImage, ref string strErrorMessage, ref CognexResult pResult)
        {
            if (!(pCogTool is CogSobelEdgeTool) || !(pSourceImage.Plane == 1))
            {
                strErrorMessage = "Input Parameter Error";
                return false;
            }

            try
            {
                ////  초기화
                CogSobelEdgeTool pCogToolSobel = pCogTool as CogSobelEdgeTool;
                pCogToolSobel.InputImage = pSourceImage.CogImage as CogImage8Grey;
                ////  동작
                pCogToolSobel.Run();
                ////  결과 확인
                if (pCogToolSobel.RunStatus.Result != CogToolResultConstants.Accept)
                {
                    strErrorMessage = pCogToolSobel.RunStatus.Message;
                    return false;
                }
                ////  결과 출력
                pResult = new CognexResult(eYoonCognexType.Sobel, new CognexImage(pCogToolSobel.Result.FinalMagnitudeImage));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                strErrorMessage = "Exception At Unknown Reason";
                return false;
            }
        }

        public static bool ImageSharpness(ICogTool pCogTool, CognexImage pSourceImage, ref string strErrorMessage, ref CognexResult pResult)
        {
            if (!(pCogTool is CogImageSharpnessTool) || !(pSourceImage.Plane == 1))
            {
                strErrorMessage = "Input Parameter Error";
                return false;
            }

            try
            {
                ////  초기화
                CogImageSharpnessTool pCogToolSharpness = pCogTool as CogImageSharpnessTool;
                pCogToolSharpness.InputImage = pSourceImage.CogImage as CogImage8Grey;
                ////  동작
                pCogToolSharpness.Run();
                ////  결과 확인
                if (pCogToolSharpness.RunStatus.Result != CogToolResultConstants.Accept)
                {
                    strErrorMessage = pCogToolSharpness.RunStatus.Message;
                    return false;
                }
                ////  결과 출력
                pResult = new CognexResult(eYoonCognexType.Sharpness, pSourceImage, pCogToolSharpness.Score);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                strErrorMessage = "Exception At Unknown Reason";
                return false;
            }
        }

        public static bool ImageFiltering(ICogTool pCogTool, CognexImage pSourceImage, ref string strErrorMessage, ref CognexResult pResult)
        {
            if (!(pCogTool is CogIPOneImageTool))  // Image Source, Result에 제약 없음
            {
                strErrorMessage = "Input Parameter Error";
                return false;
            }

            try
            {
                ////  초기화
                CogIPOneImageTool pCogToolIP = pCogTool as CogIPOneImageTool;
                pCogToolIP.InputImage = pSourceImage.CogImage;
                ////  동작
                pCogToolIP.Run();
                ////  결과 확인
                if (pCogToolIP.RunStatus.Result != CogToolResultConstants.Accept)
                {
                    strErrorMessage = pCogToolIP.RunStatus.Message;
                    return false;
                }
                ////  결과 출력 (Result Param 없음)
                pResult = new CognexResult(eYoonCognexType.Filtering, new CognexImage(pCogToolIP.OutputImage));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                strErrorMessage = "Exception At Unknown Reason";
                return false;
            }
        }

        public static bool Blob(ICogTool pCogTool, CognexImage pSourceImage, ref string strErrorMessage, ref CognexResult pResult)
        {
            if (!(pCogTool is CogBlobTool) || !(pSourceImage.Plane == 1))
            {
                strErrorMessage = "Input Parameter Error";
                return false;
            }

            try
            {
                ////  초기화
                CogBlobTool pCogToolBlob = pCogTool as CogBlobTool;
                pCogToolBlob.InputImage = pSourceImage.CogImage;
                ////  동작
                pCogToolBlob.Run();
                ////  결과 확인
                if (pCogToolBlob.RunStatus.Result != CogToolResultConstants.Accept)
                {
                    strErrorMessage = pCogToolBlob.RunStatus.Message;
                    return false;
                }
                ////  결과 출력
                if (pCogToolBlob.Results.GetBlobs().Count > 0)
                {
                    pResult = new CognexResult(new CognexImage(pCogToolBlob.Results.CreateBlobImage()), pCogToolBlob.Results.GetBlobs());
                    return true;
                }
                else
                {
                    strErrorMessage = "Blob Count is Zero";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                strErrorMessage = "Exception At Unknown Reason";
                return false;
            }
        }

        public static bool ColorSegment(ICogTool pCogTool, CognexImage pSourceImage, ref string strErrorMessage, ref CognexResult pResult)
        {
            if (!(pCogTool is CogColorSegmenterTool) || !(pSourceImage.Plane == 3))
            {
                strErrorMessage = "Input Parameter Error";
                return false;
            }

            try
            {
                ////  초기화
                CogColorSegmenterTool pCogToolSegment = pCogTool as CogColorSegmenterTool;
                pCogToolSegment.InputImage = pSourceImage.CogImage as CogImage24PlanarColor;
                ////  동작
                pCogToolSegment.Run();
                ////  결과 확인
                if (pCogToolSegment.RunStatus.Result != CogToolResultConstants.Accept)
                {
                    strErrorMessage = pCogToolSegment.RunStatus.Message;
                    return false;
                }
                ////  결과 출력 (Result Image 출력만 존재함)
                pResult = new CognexResult(eYoonCognexType.ColorSegment, new CognexImage(pCogToolSegment.Result));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                strErrorMessage = "Exception At Unknown Reason";
                return false;
            }
        }

        public static bool ColorExtract(ICogTool pCogTool, CognexImage pSourceImage, ref string strErrorMessage, ref CognexResult pResult)
        {
            if (!(pCogTool is CogColorExtractorTool) || !(pSourceImage.Plane == 3))
            {
                strErrorMessage = "Input Parameter Error";
                return false;
            }

            try
            {
                ////  초기화
                CogColorExtractorTool pCogToolExtract = pCogTool as CogColorExtractorTool;
                pCogToolExtract.InputImage = pSourceImage.CogImage as CogImage24PlanarColor;
                ////  동작
                pCogToolExtract.Run();
                ////  결과 확인
                if (pCogToolExtract.RunStatus.Result != CogToolResultConstants.Accept)
                {
                    strErrorMessage = pCogToolExtract.RunStatus.Message;
                    return false;
                }
                ////  결과 출력 (Result Image 출력만 존재함)
                pResult = new CognexResult(eYoonCognexType.ColorExtract, new CognexImage(pCogToolExtract.Results.OverallResult.GreyscaleImage));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                strErrorMessage = "Exception At Unknown Reason";
                return false;
            }
        }

        public static bool FindLine(ICogTool pCogTool, CognexImage pSourceImage, ref string strErrorMessage, ref CognexResult pResult)
        {
            if (!(pCogTool is CogFindLineTool) || !(pSourceImage.Plane == 1))
            {
                strErrorMessage = "Input Parameter Error";
                return false;
            }

            try
            {
                ////  초기화
                CogFindLineTool pCogToolFindLine = pCogTool as CogFindLineTool;
                pCogToolFindLine.InputImage = pSourceImage.CogImage as CogImage8Grey;
                ////  동작
                pCogToolFindLine.Run();
                ////  결과 확인
                if (pCogToolFindLine.RunStatus.Result != CogToolResultConstants.Accept)
                {
                    strErrorMessage = pCogToolFindLine.RunStatus.Message;
                    return false;
                }
                ////  결과 출력 (Result Image 출력 없음, Pattern 찾기 결과가 1개 이상임)
                if (pCogToolFindLine.Results.Count > 0)
                {
                    pResult = new CognexResult(pSourceImage.Clone() as CognexImage, pCogToolFindLine.Results.GetLineSegment());
                    return true;
                }
                else
                {
                    strErrorMessage = "Effective Caliper Count is Zero";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                strErrorMessage = "Exception At Unknown Reason";
                return false;
            }
        }

        public static bool PMAlign(ICogTool pCogTool, CognexImage pSourceImage, ref string strErrorMessage, ref CognexResult pResult)
        {
            if (!(pCogTool is CogPMAlignTool) || !(pSourceImage.Plane == 1))
            {
                strErrorMessage = "Input Parameter Error";
                return false;
            }

            try
            {
                ////  초기화
                CogPMAlignTool pCogToolPM = pCogTool as CogPMAlignTool;
                pCogToolPM.InputImage = pSourceImage.CogImage;
                ////  동작
                pCogToolPM.Run();
                ////  결과 확인
                if (pCogToolPM.RunStatus.Result != CogToolResultConstants.Accept)
                {
                    strErrorMessage = pCogToolPM.RunStatus.Message;
                    return false;
                }
                ////  결과 출력 (Result Image 출력 없음, Pattern 찾기 결과가 1개 이상임)
                if (pCogToolPM.Results.Count > 0)
                {
                    pResult = new CognexResult(pSourceImage.Clone() as CognexImage, pCogToolPM.Results[0].GetPose(), pCogToolPM.Pattern.TrainRegion, pCogToolPM.Results[0].Score);    // 대표적인 Pattern 1개만 송출
                    return true;
                }
                else
                {
                    strErrorMessage = "PM Result Count is Zero";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                strErrorMessage = "Exception At Unknown Reason";
                return false;
            }
        }

        public static bool TwoImageAdd(ICogTool pCogTool, CognexImage pSourceImageA, CognexImage pSourceImageB, ref string strErrorMessage, ref CognexResult pResult)
        {
            if (!(pCogTool is CogIPTwoImageAddTool))
            {
                strErrorMessage = "Input Parameter Error";
                return false;
            }

            try
            {
                ////  초기화
                CogIPTwoImageAddTool pCogToolIP = pCogTool as CogIPTwoImageAddTool;
                pCogToolIP.InputImageA = pSourceImageA.CogImage;
                pCogToolIP.InputImageB = pSourceImageB.CogImage;
                ////  동작
                pCogToolIP.Run();
                ////  결과 확인
                if (pCogToolIP.RunStatus.Result != CogToolResultConstants.Accept)
                {
                    strErrorMessage = pCogToolIP.RunStatus.Message;
                    return false;
                }
                ////  결과 출력
                pResult = new CognexResult(eYoonCognexType.ImageAdd, new CognexImage(pCogToolIP.OutputImage));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                strErrorMessage = "Exception At Unknown Reason";
                return false;
            }
        }

        public static bool TwoImageSubtract(ICogTool pCogTool, CognexImage pSourceImageA, CognexImage pSourceImageB, ref string strErrorMessage, ref CognexResult pResult)
        {
            if (!(pCogTool is CogIPTwoImageSubtractTool))
            {
                strErrorMessage = "Input Parameter Error";
                return false;
            }

            try
            {
                ////  초기화
                CogIPTwoImageSubtractTool pCogToolIP = pCogTool as CogIPTwoImageSubtractTool;
                pCogToolIP.InputImageA = pSourceImageA.CogImage;
                pCogToolIP.InputImageB = pSourceImageB.CogImage;
                ////  동작
                pCogToolIP.Run();
                ////  결과 확인
                if (pCogToolIP.RunStatus.Result != CogToolResultConstants.Accept)
                {
                    strErrorMessage = pCogToolIP.RunStatus.Message;
                    return false;
                }
                ////  결과 출력
                pResult = new CognexResult(eYoonCognexType.ImageSubtract, new CognexImage(pCogToolIP.OutputImage));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                strErrorMessage = "Exception At Unknown Reason";
                return false;
            }
        }

        public static bool TwoImageMinMax(ICogTool pCogTool, CognexImage pSourceImageA, CognexImage pSourceImageB, ref string strErrorMessage, ref CognexResult pResult)
        {
            if (!(pCogTool is CogIPTwoImageMinMaxTool))
            {
                strErrorMessage = "Input Parameter Error";
                return false;
            }

            try
            {
                ////  초기화
                CogIPTwoImageMinMaxTool pCogToolIP = pCogTool as CogIPTwoImageMinMaxTool;
                pCogToolIP.InputImageA = pSourceImageA.CogImage;
                pCogToolIP.InputImageB = pSourceImageB.CogImage;
                ////  동작
                pCogToolIP.Run();
                ////  결과 확인
                if (pCogToolIP.RunStatus.Result != CogToolResultConstants.Accept)
                {
                    strErrorMessage = pCogToolIP.RunStatus.Message;
                    return false;
                }
                ////  결과 출력
                pResult = new CognexResult(eYoonCognexType.ImageMinMax, new CognexImage(pCogToolIP.OutputImage));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                strErrorMessage = "Exception At Unknown Reason";
                return false;
            }
        }

        public static bool Undistort(ICogTool pCogTool, CognexImage pSourceImage, ref string strErrorMessage, ref CognexResult pResult)
        {
            if (!(pCogTool is CogCalibCheckerboardTool))
            {
                strErrorMessage = "Input Parameter Error";
                return false;
            }
            try
            {
                ////  초기화
                CogCalibCheckerboardTool pCogToolCalib = pCogTool as CogCalibCheckerboardTool;
                pCogToolCalib.InputImage = pSourceImage.CogImage;
                ////  동작
                pCogToolCalib.Run();
                ////  결과 확인
                if (pCogToolCalib.RunStatus.Result != CogToolResultConstants.Accept)
                {
                    strErrorMessage = pCogToolCalib.RunStatus.Message;
                    return false;
                }
                ////  결과 출력
                pResult = new CognexResult(eYoonCognexType.Calibration, new CognexImage(pCogToolCalib.OutputImage));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                strErrorMessage = "Exception At Unknown Reason";
                return false;
            }
        }
    }
}
