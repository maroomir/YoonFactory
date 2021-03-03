using System;

namespace YoonFactory
{
    public static class MathFactory
    {
        //  상관계수 산출 공식.
        public static double GetCorrelationCoefficient(byte[] pBuffer1, byte[] pBuffer2, int width, int height)
        {
            int valueBuffer1, valueBuffer2;
            int diffBuffer1, diffBuffer2;
            double averageBuffer1, averageBuffer2;
            int sumBuffer1, sumBuffer2, sumDiff;
            double dx, dy, dxy;
            double coefficient;
            int size;
            //// Pattern의 평균값을 구한다.
            size = width * height;
            sumBuffer1 = 0;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    valueBuffer1 = pBuffer1[j * width + i];
                    sumBuffer1 += valueBuffer1;
                }
            }
            averageBuffer1 = (double)sumBuffer1 / (double)size;
            ////  현 위치의 평균값을 구한다.
            sumBuffer2 = 0;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    valueBuffer2 = pBuffer2[j * width + i];
                    sumBuffer2 += valueBuffer2;
                }
            }
            averageBuffer2 = (double)sumBuffer2 / (double)size;
            ////  상관계수 구하기.
            dx = 0;
            dy = 0;
            dxy = 0;
            sumDiff = 0;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    valueBuffer1 = pBuffer1[j * width + i];
                    valueBuffer2 = pBuffer2[j * width + i];
                    diffBuffer1 = valueBuffer1 - (int)averageBuffer1;
                    diffBuffer2 = valueBuffer2 - (int)averageBuffer2;
                    // 상관계수 산출식.
                    dx += diffBuffer1 * diffBuffer1;
                    dy += diffBuffer2 * diffBuffer2;
                    dxy += diffBuffer1 * diffBuffer2;
                    sumDiff += Math.Abs(diffBuffer1 - diffBuffer2);
                }
            }
            ////  상관계수 방정식 및 값 산출.
            double a = Math.Sqrt(dx * dy);
            double b = dxy;
            if (Math.Abs(a) < 1)
            {
                a = 1;
            }
            coefficient = b * 100.0 / a;
            return coefficient;
        }

        public static double GetCorrelationCoefficient(int[] pBuffer1, int[] pBuffer2, int width, int height)
        {
            int valueBuffer1, valueBuffer2;
            int diffBuffer1, diffBuffer2;
            double averageBuffer1, averageBuffer2;
            int sumBuffer1, sumBuffer2, sumDiff;
            double dx, dy, dxy;
            double coefficient;
            int size;
            //// Pattern의 평균값을 구한다.
            size = width * height;
            sumBuffer1 = 0;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    valueBuffer1 = pBuffer1[j * width + i];
                    sumBuffer1 += valueBuffer1;
                }
            }
            averageBuffer1 = (double)sumBuffer1 / (double)size;
            ////  현 위치의 평균값을 구한다.
            sumBuffer2 = 0;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    valueBuffer2 = pBuffer2[j * width + i];
                    sumBuffer2 += valueBuffer2;
                }
            }
            averageBuffer2 = (double)sumBuffer2 / (double)size;
            ////  상관계수 구하기.
            dx = 0;
            dy = 0;
            dxy = 0;
            sumDiff = 0;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    valueBuffer1 = pBuffer1[j * width + i];
                    valueBuffer2 = pBuffer2[j * width + i];
                    diffBuffer1 = valueBuffer1 - (int)averageBuffer1;
                    diffBuffer2 = valueBuffer2 - (int)averageBuffer2;
                    // 상관계수 산출식.
                    dx += diffBuffer1 * diffBuffer1;
                    dy += diffBuffer2 * diffBuffer2;
                    dxy += diffBuffer1 * diffBuffer2;
                    sumDiff += Math.Abs(diffBuffer1 - diffBuffer2);
                }
            }
            ////  상관계수 방정식 및 값 산출.
            double a = Math.Sqrt(dx * dy);
            double b = dxy;
            if (Math.Abs(a) < 1)
            {
                a = 1;
            }
            coefficient = b * 100.0 / a;
            return coefficient;
        }

        //  최소자승법.  (Y = AX + B)
        public static bool LeastSquare(ref double pA, ref double pB, int number, double[] pX, double[] pY)
        {
            bool isResult;
            double differX, differY;
            double sumX, sumX2, sumXY, sumY;
            double a, b, c;
            isResult = true;
            ////  Data가 비정상적인지 조사함.
            for (int i = 0; i < number; i++)
            {
                if (pX[i] > 10000 || pY[i] > 10000)
                {
                    isResult = false;
                    break;
                }
            }
            if (isResult == false)
            {
                pA = 0;
                pB = 0;
                return false;
            }
            if (number < 2 || number > 10)
            {
                pA = 0;
                pB = 0;
                return false;
            }
            ////  최소자승법 계산.
            if (number == 2)
            {
                differX = pX[1] - pX[0];
                differY = pY[1] - pY[0];
                if (Math.Abs(differX) < 0.0001)
                    differX = 0.0001;
                pB = differY / differX;
                pA = pY[0] - pB * pX[0];
            }
            ////  다차원을 가질 경우. (number가 10 이상)
            else
            {
                sumX = 0.00;
                sumX2 = 0.00;
                sumXY = 0.00;
                sumY = 0.00;
                for (int i = 0; i < number; i++)
                {
                    sumX += pX[i];
                    sumX2 += pX[i] * pX[i];
                    sumXY += pX[i] * pY[i];
                    sumY += pY[i];
                }
                c = sumX * sumX - (double)number * sumX2;
                if (c == 0.0)
                    return false;
                b = (sumX * sumY - sumXY * (double)number) / c;
                a = (sumY - b * sumX) / number;
                pA = a;
                pB = b;
            }
            return true;
        }

        //  N개의 점으로 N-1차 함수를 구하는 Lagrange 공식.
        //  A[0] + A[1]X + A[2]X^2 ...
        public static bool Lagrange(ref double[] pA, ref double[] pX, ref double[] pY, int number)
        {
            int i, j;
            double sign, differX;
            double coefficient, tempValue;
            for (i = 0; i < number; i++)
                pA[i] = 0.0;
            for (i = 0; i < number; i++)
            {
                if (pY[i] == 0.0) continue;
                coefficient = pY[i];
                for (j = 0; j < number; j++)
                {
                    if (i == j) continue;
                    if (pX[i] == pX[j]) return false;
                    differX = pX[i] - pX[j];
                    coefficient /= differX;
                }
                tempValue = pX[i];
                pX[i] = 0.0;
                for (j = 0; j < number; j++)
                {
                    if (j % 2 != 0) sign = -1.0;
                    else sign = 1.0;
                    pA[number - j - 1] += sign * coefficient * Combine(pX, pY, number, j);
                }
                pX[i] = tempValue;
            }
            return true;
        }

        //  세 점을 지나는 원형을 구하는 식.
        public static bool Get3PointToCircle(YoonVector2N[] pPoint, ref double centerX, ref double centerY, ref double radius)
        {
            YoonVector2N centerPoint1, centerPoint2;
            double differX1, differX2, differY1, differY2;
            double incline1, incline2, constant1, constant2;
            centerPoint1 = new YoonVector2N();
            centerPoint2 = new YoonVector2N();
            ////  Point 0과 Point 1, Point 0과 Point 2의 중점을 구한다.
            centerPoint1.X = (pPoint[0].X + pPoint[1].X) / 2;
            centerPoint1.Y = (pPoint[0].Y + pPoint[1].Y) / 2;
            centerPoint2.X = (pPoint[0].X + pPoint[2].X) / 2;
            centerPoint2.Y = (pPoint[0].Y + pPoint[2].Y) / 2;
            differX1 = pPoint[1].X - pPoint[0].X;
            differX2 = pPoint[2].X - pPoint[0].X;
            differY1 = pPoint[0].Y - pPoint[1].Y;
            differY2 = pPoint[0].Y - pPoint[2].Y;
            radius = 0;
            if (differY1 != 0)
            {
                //// 1번 수직이등분선의 기울기 및 식 산출.  (Y = a1X + b)
                incline1 = differX1 / differY1;
                constant1 = centerPoint1.Y - incline1 * centerPoint1.X;
                if (differY2 != 0)
                {
                    //// 2번 수직이등분선의 기울기 및 식 산출.   (Y = a2X + b)
                    incline2 = differX2 / differY2;
                    constant2 = centerPoint2.Y - incline2 * centerPoint2.X;
                    if (incline1 != incline2)
                        //////  교점 위치(a1X + b1 = a2X + b2)에 해당되는 X 값 산출.
                        centerX = (constant1 - constant2) / (incline2 - incline1);
                    else
                        return false;
                }
                else if (differX2 == 0)
                    return false;
                else
                    centerX = centerPoint2.X;
            }
            else if (differY2 != 0 && differX1 != 0)
            {
                incline1 = differX2 / differY2;
                constant1 = centerPoint2.Y - incline1 * centerPoint2.X;
                centerX = centerPoint1.X;
            }
            else
                return false;
            ////  수선의 교점(centerX, centerY)과 반지름 구하기.
            centerY = incline1 * centerX + constant1;
            radius = Math.Sqrt((pPoint[0].X - centerX) * (pPoint[0].X - centerX) + (pPoint[0].Y - centerY) * (pPoint[0].Y - centerY));
            return true;
        }

        public static bool Get3PointToCircle(YoonVector2D[] pPoint, ref double centerX, ref double centerY, ref double radius)
        {
            YoonVector2D centerPoint1, centerPoint2;
            double differX1, differX2, differY1, differY2;
            double incline1, incline2, constant1, constant2;
            centerPoint1 = new YoonVector2D();
            centerPoint2 = new YoonVector2D();
            ////  Point 0과 Point 1, Point 0과 Point 2의 중점을 구한다.
            centerPoint1.X = (pPoint[0].X + pPoint[1].X) / 2;
            centerPoint1.Y = (pPoint[0].Y + pPoint[1].Y) / 2;
            centerPoint2.X = (pPoint[0].X + pPoint[2].X) / 2;
            centerPoint2.Y = (pPoint[0].Y + pPoint[2].Y) / 2;
            differX1 = pPoint[1].X - pPoint[0].X;
            differX2 = pPoint[2].X - pPoint[0].X;
            differY1 = pPoint[0].Y - pPoint[1].Y;
            differY2 = pPoint[0].Y - pPoint[2].Y;
            radius = 0;
            if (differY1 != 0)
            {
                //// 1번 수직이등분선의 기울기 및 식 산출.  (Y = a1X + b)
                incline1 = differX1 / differY1;
                constant1 = centerPoint1.Y - incline1 * centerPoint1.X;
                if (differY2 != 0)
                {
                    //// 2번 수직이등분선의 기울기 및 식 산출.   (Y = a2X + b)
                    incline2 = differX2 / differY2;
                    constant2 = centerPoint2.Y - incline2 * centerPoint2.X;
                    if (incline1 != incline2)
                        //////  교점 위치(a1X + b1 = a2X + b2)에 해당되는 X 값 산출.
                        centerX = (constant1 - constant2) / (incline2 - incline1);
                    else
                        return false;
                }
                else if (differX2 == 0)
                    return false;
                else
                    centerX = centerPoint2.X;
            }
            else if (differY2 != 0 && differX1 != 0)
            {
                incline1 = differX2 / differY2;
                constant1 = centerPoint2.Y - incline1 * centerPoint2.X;
                centerX = centerPoint1.X;
            }
            else
                return false;
            ////  수선의 교점(centerX, centerY)과 반지름 구하기.
            centerY = incline1 * centerX + constant1;
            radius = Math.Sqrt((pPoint[0].X - centerX) * (pPoint[0].X - centerX) + (pPoint[0].Y - centerY) * (pPoint[0].Y - centerY));
            return true;
        }

        //  조합.  nCr.
        public static double Combine(double[] pX, double[] pY, int number, int rootNum)
        {
            int i;
            double result;
            result = 1.0;
            if (number <= 0 || rootNum < 0 || number < rootNum)
                return 0.0;
            ////  nCn 이므로 일종의 순열과 같다고 보면 됨.
            if (number == rootNum)
            {
                for (i = 0; i < number; i++)
                    result *= pX[i];
                return result;
            }
            ////  nC0은 1이 됨.
            if (rootNum == 0)
                return 1.0;
            ////  조합 공식.
            result = pX[number - 1] * Combine(pX, pY, number - 1, rootNum - 1) + Combine(pX, pY, number - 1, rootNum);
            return result;
        }

    }


}
