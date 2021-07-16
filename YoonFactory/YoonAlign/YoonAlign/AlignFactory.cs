using System;
using System.Collections.Generic;

namespace YoonFactory.Align
{
    public static class AlignFactory
    {
        private const double INVALID_NUM = -1000.0;

        public static AlignResult Align1D(AlignObject pObject, double dScale = 1.0)
        {
            YoonVector2D pObjectVector = pObject.GetReferenceVector2D();
            YoonVector2D pOriginVector = pObject.GetOriginVector2D();
            double dThetaOrigin = pObject.GetOriginTheta();
            double dThetaObject = pObject.GetReferenceTheta();
            return CalculateAlign1D(pObjectVector, pOriginVector, dThetaObject, dThetaOrigin, dScale);
        }

        public static AlignResult Align2D(eYoonDir2D nDir, AlignObject pLeftObject, AlignObject pRightObject, double dScale = 1.0)
        {
            YoonVector2D pObjectVectorLeft = pLeftObject.GetReferenceVector2D();
            YoonVector2D pObjectVectorRight = pRightObject.GetReferenceVector2D();
            YoonVector2D pOriginVectorLeft = pLeftObject.GetOriginVector2D();
            YoonVector2D pOriginVectorRight = pRightObject.GetOriginVector2D();
            double dTheta = CalculateTheta2D(pObjectVectorLeft, pObjectVectorRight, pOriginVectorLeft, pOriginVectorRight, dScale);
            YoonVector2D pResultVector = new YoonVector2D();
            if (nDir == pLeftObject.Direction)
                pResultVector = CalculateXY2D(pObjectVectorLeft, pOriginVectorLeft, dTheta, dScale);
            else if (nDir == pRightObject.Direction)
                pResultVector = CalculateXY2D(pObjectVectorRight, pOriginVectorRight, dTheta, dScale);
            else
            {
                pResultVector.X = INVALID_NUM;
                pResultVector.Y = INVALID_NUM;
            }
            return new AlignResult()
            {
                X = pResultVector.X,
                Y = pResultVector.Y,
                Theta = dTheta
            };
        }

        public static AlignResult CalculateAlign1D(YoonVector2D pObjectVector, YoonVector2D pOriginVector, double dObjectTheta, double dOriginTheta, double dScale = 1.0)
        {
            YoonVector2D pResultVector = pOriginVector - pObjectVector;
            return new AlignResult()
            {
                X = pResultVector.X * dScale,
                Y = pResultVector.Y * dScale,
                Theta = (dOriginTheta - dObjectTheta) * dScale
            };
        }

        public static double CalculateTheta2D(YoonVector2D pObjectVectorLeft, YoonVector2D pObjectVectorRight, YoonVector2D pOriginVectorLeft, YoonVector2D pOriginVectorRight, double dScale = 1.0)
        {
            ////  회전중심(0,0) 기준 Origin이 "Y축과" 이루는 각도 계산
            double thetaObject = Math.Atan2(pOriginVectorRight.X - pOriginVectorLeft.X, pOriginVectorLeft.Y - pOriginVectorRight.Y);
            if (thetaObject == double.NaN) return INVALID_NUM;

            ////  회전중심(0,0) 기준 Mark가 "Y축과" 이루는 각도 계산 및 현재 값 삽입
            double thetaMark = Math.Atan2(pObjectVectorRight.X - pObjectVectorLeft.X, pObjectVectorLeft.Y - pObjectVectorRight.Y);
            if (thetaMark == double.NaN) return INVALID_NUM;

            return (thetaObject - thetaMark) * dScale;
        }

        public static YoonVector2D CalculateXY2D(YoonVector2D pObjectVector, YoonVector2D pOriginVector, double dTheta, double dScale = 1.0)
        {
            ////  예외 처리
            if (dTheta == INVALID_NUM)
                return new YoonVector2D(INVALID_NUM, INVALID_NUM);

            ////  Align 계산
            return new YoonVector2D()
            {
                X = (pOriginVector.X - (pObjectVector.X * Math.Cos(dTheta) - pObjectVector.Y * Math.Sin(dTheta))) * dScale,
                Y = (pOriginVector.Y - (pObjectVector.X * Math.Sin(dTheta) + pObjectVector.Y * Math.Cos(dTheta))) * dScale,
            };
        }
    }
}
